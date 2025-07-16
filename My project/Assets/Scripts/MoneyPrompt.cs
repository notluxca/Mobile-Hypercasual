using UnityEngine;

public class MoneyPrompt : MonoBehaviour
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

    void OnEnable()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (Target == null)
            {
                Debug.LogWarning("FlyToPlayer: No player found!");
                enabled = false;
                return;
            }
        }

        startPos = transform.position;
        elapsedTime = 0f;
        isFlying = true;
    }

    void Update()
    {
        if (!isFlying) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / flyTime);

        Vector3 currentTarget = Target.position;

        // Interpola horizontalmente
        Vector3 flatPos = Vector3.Lerp(startPos, currentTarget, t);

        // Adiciona arco vertical
        float heightOffset = arcHeight * Mathf.Sin(t * Mathf.PI);
        flatPos.y += heightOffset;

        transform.position = flatPos;

        // Chegou perto o suficiente?
        if (Vector3.Distance(transform.position, currentTarget) <= destroyDistance)
        {
            if (AddCash) CurrencyManager.Instance.AddCash(1);
            Destroy(gameObject);
        }
    }
}
