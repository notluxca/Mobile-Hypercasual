using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class CombatController : MonoBehaviour
{
    [Header("Combat Settings")]
    public float checkInterval = 1.0f;
    public float attackDistance = 3f;
    public float turnSpeed = 5f; // Add this for smooth turning
    public LayerMask enemyLayer;

    private SphereCollider attackRange;
    private PlayerController playerController;
    private Transform targetEnemy; // Store the current target

    private void Start()
    {
        attackRange = GetComponent<SphereCollider>();
        attackRange.isTrigger = true;

        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("CharacterController não encontrado!");

        StartCoroutine(CheckForEnemies());
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }
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
                targetEnemy = closestEnemy; // Set the target for smooth turning
                playerController.Attack();
            }
            else
            {
                targetEnemy = null; // No target to look at
            }
        }
    }
}
