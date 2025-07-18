using UnityEngine;

public class MoneyPrompt : PoolableObject
{
    [Header("Money Prompt Settings")]
    public bool AddCash = true;
    public Transform Target;
    public float flyTime = 1f;
    public float arcHeight = 2f;
    public float destroyDistance = 0.5f;

    private Vector3 startPos;
    private float elapsedTime;

    private bool isFlying = false;
    private TrailRenderer trailRenderer;

    void OnEnable()
    {
        isFlying = false;
        elapsedTime = 0f;

        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();

        if (trailRenderer != null)
            trailRenderer.Clear();
    }

    void OnDisable()
    {
        isFlying = false;
        Target = null;
    }

    public void FlyFrom(Vector3 from)
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (Target == null)
            {
                Debug.LogWarning("FlyToPlayer: No player found!");
                ReturnToPool();
                return;
            }
        }

        transform.position = from;
        startPos = from;
        elapsedTime = 0f;
        isFlying = true;
    }

    void Update()
    {
        if (!isFlying || Target == null) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / flyTime);

        Vector3 currentTarget = Target.position;

        Vector3 flatPos = Vector3.Lerp(startPos, currentTarget, t);
        float heightOffset = arcHeight * Mathf.Sin(t * Mathf.PI);
        flatPos.y += heightOffset;

        transform.position = flatPos;

        if (Vector3.Distance(transform.position, currentTarget) <= destroyDistance)
        {
            if (AddCash)
                CurrencyManager.Instance.AddCash(1);

            ReturnToPool();
        }
    }
}
