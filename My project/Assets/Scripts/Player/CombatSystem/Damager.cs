using UnityEngine;

public class Damager : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            // Debug.Log("Hit: " + other.name);
            damageable.TakeDamage(transform, 10, 50f);
        }
    }
}
