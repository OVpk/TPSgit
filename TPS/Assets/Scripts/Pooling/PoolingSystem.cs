using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    
    protected Queue<Projectile> pool = new Queue<Projectile>();
    
    protected void SpawnProjectile()
    {
        if (pool.Count == 0)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, transform);
            newProjectile.Init(this);
            AddToPool(newProjectile);
        }

        Projectile projectile = pool.Dequeue();
        projectile.transform.localPosition = Vector3.zero;
        projectile.gameObject.SetActive(true);

        ApplyTrajectory(projectile);
    }
    
    public void AddToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        pool.Enqueue(projectile);
    }
    
    protected abstract void ApplyTrajectory(Projectile projectile);
}
