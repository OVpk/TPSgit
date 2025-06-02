using UnityEngine;

public class PeePoolingSystem : PoolingSystem
{
    [SerializeField] public float upwardForce = -0.55f;
    [SerializeField] private float forwardForce = 3f;
    [SerializeField] private float gravityMultiplier = 3f;
    
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float spawnInterval = 0.01f;

    protected override void ApplyTrajectory(Projectile projectile)
    {
        Vector3 force = transform.forward * forwardForce + transform.up * upwardForce;
        projectile.Launch(force);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }
    
    public void StartGenerator()
    {
        InvokeRepeating(nameof(SpawnProjectile), startDelay, spawnInterval);
    }
    
    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnProjectile));
        foreach (var projectile in pool)
        {
            Destroy(projectile.gameObject);
        }
    }
}