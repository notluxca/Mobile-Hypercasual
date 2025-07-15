using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class CombatController : MonoBehaviour
{
    [Header("Combat Settings")]
    public float checkInterval = 1.0f;
    public float attackDistance = 3f;
    public LayerMask enemyLayer;

    private SphereCollider attackRange;
    private PlayerController playerController;

    private void Start()
    {
        attackRange = GetComponent<SphereCollider>();
        attackRange.isTrigger = true;

        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("CharacterController não encontrado!");

        StartCoroutine(CheckForEnemies());
    }

    private IEnumerator CheckForEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange.radius, enemyLayer);
            Transform closestEnemy = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (var hitCollider in hitColliders)
            {
                float distanceSqr = (hitCollider.transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = hitCollider.transform;
                }
            }

            if (closestEnemy != null && Mathf.Sqrt(closestDistanceSqr) <= attackDistance)
            {
                // Olha para o inimigo
                Vector3 direction = (closestEnemy.position - transform.position).normalized;
                direction.y = 0f; // Mantém o olhar no plano horizontal
                transform.rotation = Quaternion.LookRotation(direction);
                Debug.Log("Olhou para o inimigo: " + closestEnemy.name);
                // Ataca
                playerController.Attack();
            }
        }
    }
}
