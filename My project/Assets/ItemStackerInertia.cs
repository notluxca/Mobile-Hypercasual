using UnityEngine;
using System.Collections.Generic;

public class ItemStackerInertia : MonoBehaviour
{
    public Transform player;
    public float verticalOffset = 0.5f;
    public float smoothTime = 0.2f;
    public float rotationMultiplier = 10f;
    public float rotationSmooth = 5f;

    public List<Transform> stackedItems = new List<Transform>();
    private List<Vector3> velocity = new List<Vector3>();
    private List<Vector3> lastPositions = new List<Vector3>();

    private Vector3 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        Vector3 targetPos = player.position;

        for (int i = 0; i < stackedItems.Count; i++)
        {
            Transform item = stackedItems[i];
            targetPos += Vector3.up * verticalOffset;

            // Inicializa listas auxiliares, se necessário
            while (velocity.Count <= i)
                velocity.Add(Vector3.zero);
            while (lastPositions.Count <= i)
                lastPositions.Add(item.position);

            float adjustedSmoothTime = smoothTime + i * 0.05f;

            // Smooth posição
            Vector3 vel = velocity[i];
            item.position = Vector3.SmoothDamp(item.position, targetPos, ref vel, adjustedSmoothTime);
            velocity[i] = vel;

            // --- Rotação baseada no PRÓPRIO movimento do item ---
            Vector3 itemDelta = item.position - lastPositions[i];
            float lateralSpeed = itemDelta.x; // Use .z se seu movimento principal for no eixo Z
            float rotationZ = -lateralSpeed * (i + 1) * rotationMultiplier;

            Quaternion targetRot = Quaternion.Euler(0f, 0f, rotationZ);
            item.rotation = Quaternion.Lerp(item.rotation, targetRot, Time.deltaTime * rotationSmooth);

            // Atualiza última posição do item
            lastPositions[i] = item.position;
        }

        lastPlayerPosition = player.position;
    }

    public void AddItem(Transform item)
    {
        item.parent = null;
        stackedItems.Add(item);
        velocity.Add(Vector3.zero);
        lastPositions.Add(item.position);
    }
}
