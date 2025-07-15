using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Transform hitPosition, int damage, float knockbackForce = 0f);
}
