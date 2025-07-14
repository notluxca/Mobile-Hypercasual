using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]
    public InputActionAsset inputActions;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Animation")]
    public PlayerAnimatorController playerAnimatorController;

    private InputAction moveAction;
    private CharacterController characterController;
    private Camera mainCamera;

    private bool isAttacking = false;
    private float attackDuration = 0.8f; // tempo da animação de ataque

    void Awake()
    {
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
        characterController = GetComponent<CharacterController>();
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

    void Update()
    {
        // Bloqueia tudo enquanto ataca
        if (isAttacking)
            return;

        // Verifica se o jogador atacou (ex: tecla espaço)
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(StartAttack());
            return;
        }

        // Movimento e animação
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        float magnitude = move.magnitude;

        if (magnitude > 0.1f)
        {
            characterController.Move(move.normalized * speed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (magnitude < 0.8f)
                playerAnimatorController.Play(PlayerAnimations.Walk);
            else
                playerAnimatorController.Play(PlayerAnimations.Run);
        }
        else
        {
            playerAnimatorController.Play(PlayerAnimations.Idle);
        }
    }

    private IEnumerator StartAttack()
    {
        isAttacking = true;

        playerAnimatorController.Play(PlayerAnimations.Attack);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }
}
