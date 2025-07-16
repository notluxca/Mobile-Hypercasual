using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class DynamicJoystick : MonoBehaviour
{
    public RectTransform joystickRoot; // The full joystick container
    public OnScreenStick onScreenStick; // The component
    public CanvasGroup canvasGroup; // For fading

    [Header("Fade Settings")]
    public float fadeDuration = 0.3f;
    public float holdAlpha = 1f;
    public float idleAlpha = 0f;

    private bool isTouching = false;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnTouchStart;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnTouchEnd;
    }

    void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnTouchStart;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnTouchEnd;
        EnhancedTouchSupport.Disable();
    }

    void OnTouchStart(Finger finger)
    {
        Debug.Log("touch");
        if (isTouching) return; // Only one touch
        isTouching = true;

        // Convert touch position to anchored position
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickRoot.parent as RectTransform,
            finger.screenPosition,
            null,
            out pos
        );

        joystickRoot.anchoredPosition = pos;
        canvasGroup.DOFade(holdAlpha, fadeDuration);
        joystickRoot.gameObject.SetActive(true);
    }

    void OnTouchEnd(Finger finger)
    {
        if (!isTouching) return;
        isTouching = false;

        canvasGroup.DOFade(idleAlpha, fadeDuration).OnComplete(() =>
        {
            joystickRoot.gameObject.SetActive(false);
        });
    }
}
