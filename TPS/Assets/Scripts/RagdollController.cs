using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody[] ragdollRigidbodies;

    public bool isRagdollActive;

    void Start()
    {
        EnableRagdoll(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            EnableRagdoll(!isRagdollActive);
        }
    }

    public void EnableRagdoll(bool state)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !state;
        }

        animator.enabled = !state;
        isRagdollActive = state;
    }
}
