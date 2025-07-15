using UnityEngine;

public class CombatController : MonoBehaviour
{
    // quando um trigger de seu Damager é detectado ele aplica dano no hitObject se ele for IDamageable
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log("Hit: " + other.name);
            damageable.TakeDamage(transform, 10, 50f);
        }
    }
}
