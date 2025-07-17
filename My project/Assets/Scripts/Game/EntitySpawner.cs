using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntitySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform EntitiesParent;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private LayerMask groundLayer;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnRoutine;

    void OnEnable()
    {
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Clean Destroyed enemies from
            activeEnemies.RemoveAll(enemy => enemy == null);

            if (activeEnemies.Count < maxEnemies)
            {
                Vector3 spawnPosition = GetRandomPointOnGround();

                if (spawnPosition != Vector3.zero)
                {
                    Quaternion randomYRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, randomYRotation, EntitiesParent);
                    activeEnemies.Add(newEnemy);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPointOnGround()
    {
        const int maxAttempts = 5;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPos = new Vector3(
                player.position.x + randomCircle.x,
                player.position.y + 20f,
                player.position.z + randomCircle.y
            );

            Debug.DrawRay(randomPos, Vector3.down * 100f, Color.yellow, 1f);

            if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 100f))
            {
                int hitLayer = hit.collider.gameObject.layer;
                // Debug.Log($"Tentativa {attempt + 1}: Acertou '{hit.collider.name}' na layer '{LayerMask.LayerToName(hitLayer)}'");

                if (((1 << hitLayer) & groundLayer) != 0)
                {
                    return hit.point;
                }
            }
        }

        return Vector3.zero;
    }
}
