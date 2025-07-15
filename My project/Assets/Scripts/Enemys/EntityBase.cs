using UnityEngine;

public class EntityBase : MonoBehaviour, IDamageable
{
    private RagdollController ragdollController;
    [SerializeField] private Rigidbody mainRigidbody;

    private Animator animator;

    void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(Transform hitPosition, int damage, float knockbackForce)
    {
        // Ativa ragdoll
        ragdollController.GoRagdoll();

        // Aplica knockback
        if (mainRigidbody != null && hitPosition != null)
        {
            Vector3 direction = (transform.position - hitPosition.position).normalized;
            mainRigidbody.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }

    // Sobrecarga para ContextMenu
    [ContextMenu("die")]
    public void Die()
    {
        TakeDamage(transform, 0, 5f); // valor simbólico de knockback
    }
}
