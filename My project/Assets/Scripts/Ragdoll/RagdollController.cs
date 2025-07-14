using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Transform rootBone { get; private set; }

    private Rigidbody[] rigidbodies;

    void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        var animator = GetComponent<Animator>();
        if (animator != null)
            rootBone = animator.GetBoneTransform(HumanBodyBones.Hips);
        else
            rootBone = transform; // fallback
    }

    public void AttachToStack()
    {
        foreach (var rb in rigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = false;
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
