using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [Header("Enemy Life Parameter")] 
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EnemyDeath();
        }
    }
    
    private void EnemyDeath()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
