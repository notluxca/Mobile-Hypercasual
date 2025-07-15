using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;   // O que a câmera deve seguir
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f); // Offset da câmera em relação ao alvo
    [SerializeField] private float smoothSpeed = 5f; // Velocidade da suavização

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target); // (Opcional) mantém a câmera sempre olhando para o alvo
    }
}
