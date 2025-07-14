using UnityEngine;
using System.Collections.Generic;

public class ItemStackerInertia : MonoBehaviour
{
    public Transform player;
    public float verticalOffset = 0.5f;
    public float smoothTime = 0.2f;
    public float rotationMultiplier = 10f;
    public float rotationSmooth = 5f;

    [Header("Ragdolls na stack")]
    public List<RagdollController> ragdollStackList = new List<RagdollController>();

    private List<Transform> rootBones = new List<Transform>();
    private List<Vector3> velocity = new List<Vector3>();
    private List<Vector3> lastPositions = new List<Vector3>();

    private Vector3 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = player.position;

        // Inicializa os ragdolls definidos via inspector
        for (int i = 0; i < ragdollStackList.Count; i++)
        {
            var ragdoll = ragdollStackList[i];
            if (ragdoll != null && ragdoll.rootBone != null)
            {
                ragdoll.AttachToStack();

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

            float adjustedSmoothTime = smoothTime + i * 0.05f;

            // Smooth posição
            Vector3 vel = velocity[i];
            item.position = Vector3.SmoothDamp(item.position, targetPos, ref vel, adjustedSmoothTime);
            velocity[i] = vel;

            // Rotação baseada na movimentação lateral
            Vector3 itemDelta = item.position - lastPositions[i];
            float lateralSpeed = itemDelta.x; // ou .z se movimento principal for Z
            float rotationZ = -lateralSpeed * (i + 1) * rotationMultiplier;

            Quaternion targetRot = Quaternion.Euler(0f, 0f, rotationZ);
            item.rotation = Quaternion.Lerp(item.rotation, targetRot, Time.deltaTime * rotationSmooth);

            lastPositions[i] = item.position;
        }

        lastPlayerPosition = player.position;
    }
}
