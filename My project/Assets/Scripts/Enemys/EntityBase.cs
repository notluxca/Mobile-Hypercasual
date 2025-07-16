using System;
using System.Collections;
using UnityEngine;

public class EntityBase : MonoBehaviour, IDamageable
{
    [Header("Entity Settings")]
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private float delayToStack;

    private RagdollController ragdollController;
    private Collider entityCollider;


    public static Action EntityPunched;
    public static Action<RagdollController> EntityDied;

    void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
        entityCollider = GetComponent<Collider>();
    }

    public void TakeDamage(Transform hitPosition, int damage, float knockbackForce)
    {
        // Ativa ragdoll
        ragdollController.GoRagdoll();
        entityCollider.enabled = false; // disable entity collision with player

        // Aplica knockback
        if (mainRigidbody != null && hitPosition != null)
        {
            Vector3 direction = (transform.position - hitPosition.position).normalized;
            mainRigidbody.AddForce(direction * knockbackForce, ForceMode.Impulse);
            EntityPunched?.Invoke();
            StartCoroutine(WarnDeath());
        }
    }

    private IEnumerator WarnDeath()
    {
        yield return new WaitForSeconds(delayToStack);
        EntityDied?.Invoke(ragdollController);
    }

}
