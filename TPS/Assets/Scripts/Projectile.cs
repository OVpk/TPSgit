using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public PoolingSystem poolingSystem;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile") return;
        poolingSystem.AddToPool(this);
    }

    public void Launch(Vector3 force)
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
