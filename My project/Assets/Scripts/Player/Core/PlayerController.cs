using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]
    public InputActionAsset inputActions;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;
    private float currentSpeed = 0f;

    public int currentStackLimit;
    ItemStackerInertia itemStacker;


    private PlayerAnimatorController playerAnimatorController;
    private InputAction moveAction;
    private CharacterController characterController;
    private Camera mainCamera;
    private float attackDuration = 1.5f;
    private bool isAttacking = false;

    void Awake()
    {
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
        characterController = GetComponent<CharacterController>();
        // itemStacker.
        mainCamera = Camera.main;

        var actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update() // fix: make all movement frame independent and in other class  
    {
        // Block all actions while playing attacks
        if (isAttacking)
            return;
        // if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     StartCoroutine(StartAttack());
        //     return;
        // }


        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        float magnitude = move.magnitude;

        if (magnitude > 0.1f)
        {
            characterController.Move(move.normalized * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (magnitude < 0.8f)
            {
                currentSpeed = walkSpeed;
                playerAnimatorController.Play(PlayerAnimations.Walk);
            }
            else
            {
                currentSpeed = runningSpeed;
                playerAnimatorController.Play(PlayerAnimations.Run);
            }
        }
        else
        {
            playerAnimatorController.Play(PlayerAnimations.Idle);
        }
    }

    public void Attack()
    {
        if (isAttacking)
            return;

        StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        isAttacking = true;

        playerAnimatorController.Play(PlayerAnimations.Attack);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }
}
