using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemStackerInertia : MonoBehaviour
{
    [Header("Stack Configurations")]
    public Transform player;
    public float verticalOffset = 0.5f;
    public float smoothTime = 0.2f;
    public float rotationMultiplier = 10f;
    public float rotationSmooth = 5f;
    public float incrementalFollowDelay;

    [Header("Ragdolls in stack")]
    public List<RagdollController> preStackList = new List<RagdollController>();
    public List<RagdollController> ragdollStackList = new List<RagdollController>();
    public List<Transform> rootBones = new List<Transform>();

    private List<Vector3> velocity = new List<Vector3>();
    private List<Vector3> lastPositions = new List<Vector3>();

    private Vector3 lastPlayerPosition;

    void OnEnable()
    {
        EntityBase.EntityDied += AddToStack;
        EntityBase.EntityPunched += PreStack;
    }

    void OnDisable()
    {
        EntityBase.EntityDied -= AddToStack;
        EntityBase.EntityPunched -= PreStack;
    }

    private void PreStack(RagdollController controller)
    {
        if (controller == null || controller.rootBone == null) return;
        if (ragdollStackList.Contains(controller) || preStackList.Contains(controller)) return;

        preStackList.Add(controller);

        Debug.Log($"Ragdoll {controller.name} prestacked.");
    }

    private void AddToStack(RagdollController controller)
    {
        if (controller == null || controller.rootBone == null) return;
        if (ragdollStackList.Contains(controller)) return;

        ragdollStackList.Add(controller);
        rootBones.Add(controller.rootBone);
        velocity.Add(Vector3.zero);

        Vector3 targetStackPosition = player.position + Vector3.up * verticalOffset * (rootBones.Count + 1);
        lastPositions.Add(controller.rootBone.position - (targetStackPosition - controller.rootBone.position));

        Rigidbody rb = controller.rootBone.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Debug.Log($"Ragdoll {controller.name} added to stack.");
    }

    void Start()
    {
        lastPlayerPosition = player.position;

        // Inicializa os ragdolls definidos via inspector
        for (int i = 0; i < ragdollStackList.Count; i++)
        {
            var ragdoll = ragdollStackList[i];
            if (ragdoll != null && ragdoll.rootBone != null)
            {
                rootBones.Add(ragdoll.rootBone);
                velocity.Add(Vector3.zero);
                lastPositions.Add(ragdoll.rootBone.position);
            }
        }
    }

    void Update()
    {
        Vector3 targetPos = player.position;

        for (int i = 0; i < rootBones.Count; i++)
        {
            Transform item = rootBones[i];
            targetPos += Vector3.up * verticalOffset;

            float adjustedSmoothTime = smoothTime + i * incrementalFollowDelay;

            Vector3 vel = velocity[i];
            item.position = Vector3.SmoothDamp(item.position, targetPos, ref vel, adjustedSmoothTime);
            velocity[i] = vel;

            Vector3 itemDelta = item.position - lastPositions[i];
            float lateralSpeed = itemDelta.x;
            float rotationZ = -lateralSpeed * (i + 1) * rotationMultiplier;

            Quaternion targetRot = Quaternion.Euler(0f, 0f, rotationZ);
            item.rotation = Quaternion.Lerp(item.rotation, targetRot, Time.deltaTime * rotationSmooth);

            lastPositions[i] = item.position;
        }

        lastPlayerPosition = player.position;
    }

    // Remove the last ragdoll from the stack
    public RagdollController PopLastRagdoll()
    {
        if (ragdollStackList.Count == 0) return null;

        int index = ragdollStackList.Count - 1;

        var ragdoll = ragdollStackList[index];
        ragdollStackList.RemoveAt(index);
        rootBones.RemoveAt(index);
        velocity.RemoveAt(index);
        lastPositions.RemoveAt(index);

        // Se foi prestacado, remova da prestack
        if (preStackList.Contains(ragdoll))
            preStackList.Remove(ragdoll);

        return ragdoll;
    }
}
