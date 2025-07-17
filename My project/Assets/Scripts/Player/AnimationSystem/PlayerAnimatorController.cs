using UnityEngine;

// Controll all player animations based on a ENUM to list avaible animations
public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Animator Settings")]
    public Animator animator;
    public float transitionDuration = 0.1f;

    private PlayerAnimations currentState = PlayerAnimations.Idle;

    private string[] animationNames =
    {
        "Idle",   // PlayerAnimations.Idle
        "Walk",   // PlayerAnimations.Walk
        "Run",    // PlayerAnimations.Run
        "Attack"  // PlayerAnimations.Attack
    };

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Play(PlayerAnimations newState, float lockDuration = 0f)
    {
        if (currentState == newState) // prevent repeated animation calls
            return;

        string targetAnim = animationNames[(int)newState];
        animator.CrossFade(targetAnim, transitionDuration);
        currentState = newState;
    }

    public PlayerAnimations GetCurrentState() => currentState;
}
