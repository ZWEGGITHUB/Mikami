using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Enemy Projectile")] 
    [SerializeField] private Transform baseShootPoint;
    [SerializeField] private GameObject projectilePrefab;
    private GameObject instanceProjectilePrefab;
    private int projectileSpeed = 12;
    private float shootInterval = 0.8f;
    [SerializeField] private float projectileArcHeight = 2.0f;

    [Header("Enemy Entity")]
    private EnemyMovement enemyMovement;

    [Header("Hero Entity")] 
    [SerializeField] private GameObject heroTransform;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void AttackRange()
    {
        StartCoroutine(ShootRoutine());
    }
    
    public void AttackMelee()
    {
        
    }
    
    private IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(shootInterval);
        
        Vector2 directionToPlayer = (heroTransform.transform.position - baseShootPoint.position).normalized;
        
        RaycastHit2D hit = Physics2D.Raycast(baseShootPoint.position, directionToPlayer, Mathf.Infinity, enemyMovement.PlayerLayer);
        
        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out HeroMovement heroComponent))
            {
                FireProjectile(directionToPlayer);   
            }
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        instanceProjectilePrefab = Instantiate(projectilePrefab, baseShootPoint.position, Quaternion.identity);
        
        Rigidbody2D projectileRigidbody = instanceProjectilePrefab.GetComponent<Rigidbody2D>();
        
        if (projectileRigidbody != null)
        {
            float gravity = Physics2D.gravity.magnitude;
            float verticalSpeed = Mathf.Sqrt(2 * gravity * projectileArcHeight);
            
            Vector2 horizontalVelocity = direction.normalized * projectileSpeed;
            Vector2 verticalVelocity = Vector2.up * verticalSpeed;
            projectileRigidbody.velocity = horizontalVelocity + verticalVelocity;
        }
    }
}
