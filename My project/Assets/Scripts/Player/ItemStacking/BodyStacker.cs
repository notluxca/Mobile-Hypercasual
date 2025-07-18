using UnityEngine;
using System.Collections.Generic;
using System;

public class BodyStacker : MonoBehaviour
{
    [Header("Stack Configurations")]
    public Transform stackPosition;
    public float verticalOffset = 0.5f;
    public float smoothTime = 0.2f;
    public float rotationMultiplier = 10f;
    public float rotationSmooth = 5f;
    public float incrementalFollowDelay;

    [Header("Ragdolls in stack")]
    public List<RagdollController> preStackList = new List<RagdollController>();  // prestack is a Stack that count enemys from the moment they are punched - I use it for UI calculations
    private List<RagdollController> ragdollStackList = new List<RagdollController>(); // this stack only updates once the entitie ragdoll starts flying towards the stack
    private List<Transform> rootBones = new List<Transform>();
    public static Action<int> StackCountChanged;

    private List<Vector3> velocity = new List<Vector3>();
    private List<Vector3> lastPositions = new List<Vector3>();
    Vector3 _targetStackPos;

    void OnEnable()
    {
        EntityBase.EntityDied += AddToStack;
        EntityBase.EntityPunched += AddToPreStack;
    }

    void OnDisable()
    {
        EntityBase.EntityDied -= AddToStack;
        EntityBase.EntityPunched -= AddToPreStack;
    }

    private void AddToPreStack(RagdollController controller)
    {
        if (controller == null || controller.rootBone == null) return;
        if (ragdollStackList.Contains(controller) || preStackList.Contains(controller)) return;

        preStackList.Add(controller);
        StackCountChanged?.Invoke(preStackList.Count);

        // Debug.Log($"Ragdoll {controller.name} prestacked.");
    }

    private void AddToStack(RagdollController controller)
    {
        if (controller == null || controller.rootBone == null) return;
        if (ragdollStackList.Contains(controller)) return;

        ragdollStackList.Add(controller);
        rootBones.Add(controller.rootBone);
        velocity.Add(Vector3.zero);

        Vector3 targetStackPosition = stackPosition.position + Vector3.up * verticalOffset * (rootBones.Count + 1);
        lastPositions.Add(controller.rootBone.position - (targetStackPosition - controller.rootBone.position));

        Rigidbody rb = controller.rootBone.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Debug.Log($"Ragdoll {controller.name} added to stack.");
    }

    void Update()
    {
        _targetStackPos = stackPosition.position;

        for (int i = 0; i < rootBones.Count; i++)
        {
            Transform item = rootBones[i];
            _targetStackPos += Vector3.up * verticalOffset;

            float adjustedSmoothTime = smoothTime + i * incrementalFollowDelay;

            Vector3 vel = velocity[i];
            item.position = Vector3.SmoothDamp(item.position, _targetStackPos, ref vel, adjustedSmoothTime);
            velocity[i] = vel;

            Vector3 itemDelta = item.position - lastPositions[i];
            float lateralSpeed = itemDelta.x;
            float rotationZ = -lateralSpeed * (i + 1) * rotationMultiplier;

            Quaternion targetRot = Quaternion.Euler(0f, 0f, rotationZ);
            item.rotation = Quaternion.Lerp(item.rotation, targetRot, Time.deltaTime * rotationSmooth);

            lastPositions[i] = item.position;
        }
    }

    // Remove the last ragdoll from the stack
    public RagdollController PopLastRagdoll()
    {
        if (ragdollStackList.Count == 0) return null;

        int index = ragdollStackList.Count - 1;

        RagdollController ragdoll = ragdollStackList[index];
        ragdollStackList.RemoveAt(index);
        rootBones.RemoveAt(index);
        velocity.RemoveAt(index);
        lastPositions.RemoveAt(index);

        // Remove from prestack
        preStackList.Remove(ragdoll);
        StackCountChanged?.Invoke(preStackList.Count);

        return ragdoll;
    }
}
