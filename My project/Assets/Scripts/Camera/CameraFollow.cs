using UnityEngine;
using DG.Tweening; // Make sure DOTween is imported

public class CameraFollow : MonoBehaviour
{
    [Header("Main Camera Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.5f;
    private int shakeVibrato = 10;
    private float shakeRandomness = 90f;
    private bool fadeOut = true;
    private bool snapping = false;

    private void OnEnable()
    {
        EntityBase.EntityPunched += ShakeCamera;
    }

    private void OnDisable()
    {
        EntityBase.EntityPunched -= ShakeCamera;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }

    private void ShakeCamera(RagdollController _)
    {
        transform.DOShakePosition(
            shakeDuration,
            shakeStrength,
            shakeVibrato,
            shakeRandomness,
            snapping,
            fadeOut
        );
    }
}
