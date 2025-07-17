using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Transform rootBone { get; private set; }

    private Rigidbody[] rigidbodies;
    private Animator animator;

    void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        if (animator != null)
            rootBone = animator.GetBoneTransform(HumanBodyBones.Hips);
        else
            rootBone = transform;

        // Start ragdoll offline
        foreach (var rb in rigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    [ContextMenu("Go Ragdoll")] // editor testing
    public void GoRagdoll()
    {
        if (animator != null)
            animator.enabled = false;

        foreach (var rb in rigidbodies)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        // Debug.Log("Ragdolled");
    }

    public void AttachToStack()
    {
        animator.Rebind();         // Reinicia os valores
        animator.Update(0);        // Garante que o rebind seja aplicado
        animator.enabled = false;
        foreach (var rb in rigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void DetachFromStack()
    {
        foreach (var rb in rigidbodies)
        {
            rb.useGravity = true;
        }
    }
}
