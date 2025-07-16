using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Faz o objeto olhar para a câmera
            transform.LookAt(mainCamera.transform);

            // Corrige rotação para manter a frente voltada corretamente (inverte se necessário)
            transform.forward = -transform.forward;
        }
    }
}
