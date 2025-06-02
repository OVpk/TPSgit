using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PoolingSystem poolingSystem;
    private Rigidbody rb;

    public void Init(PoolingSystem pool)
    {
        rb = GetComponent<Rigidbody>();
        poolingSystem = pool;
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
