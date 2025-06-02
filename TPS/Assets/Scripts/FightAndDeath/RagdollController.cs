using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;

    private void Start()
    {
        EnableRagdoll(false);
    }

    public void EnableRagdoll(bool state)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !state;
        }
        animator.enabled = !state;
    }
}
