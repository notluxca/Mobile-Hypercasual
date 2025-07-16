using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SellStackArea : MonoBehaviour
{
    public Transform targetTransform;
    public GameObject moneyPrompt;
    public float delayBetweenSales = 0.3f;
    public float arcHeight = 2f;

    private bool isSelling = false;
    private Coroutine sellRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var stacker = other.GetComponent<ItemStackerInertia>();
            if (stacker != null && !isSelling)
            {
                sellRoutine = StartCoroutine(SellRoutine(stacker));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isSelling)
        {
            StopCoroutine(sellRoutine);
            isSelling = false;
        }
    }

    private IEnumerator SellRoutine(ItemStackerInertia stacker)
    {
        isSelling = true;

        while (true)
        {
            var ragdoll = stacker.PopLastRagdoll();
            if (ragdoll == null) break;

            var root = ragdoll.rootBone;
            var start = root.position;
            var end = targetTransform.position;
            var mid = (start + end) / 2 + Vector3.up * arcHeight;

            // Desativa física
            var rb = root.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Arco com DOTween
            Sequence seq = DOTween.Sequence();
            seq.Append(root.DOMove(mid, delayBetweenSales / 2f).SetEase(Ease.OutQuad));
            seq.Append(root.DOMove(end, delayBetweenSales / 2f).SetEase(Ease.InQuad));

            yield return seq.WaitForCompletion();



            Destroy(ragdoll.gameObject); // fix: Object Pooling
            Instantiate(moneyPrompt, targetTransform.position, Quaternion.identity);
        }

        isSelling = false;
    }
}
