using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Enemy Information")] 
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private GameObject[] enemyPrefab;
    private GameObject enemyInstancePrefab;
    private List<GameObject> enemySpawned = new List<GameObject>();

    [Header("Player Information")] 
    [SerializeField] private HeroAttack heroAttack;

    [Header("Timer Spawner")] 
    [SerializeField] private float spawnTimer = 2f;
    private float currentSpawnTimer = 0f;
    private int enemyIndex = 0;
    
    private void Update()
    {
        EnemySpawner();
    }

    private void EnemySpawner()
    {
        if (heroAttack.AttackMode)
        {
            InstantiateEnemyPrefab();
        }
    }
    
    private void InstantiateEnemyPrefab()
    {
        currentSpawnTimer += Time.deltaTime;
        
        if (currentSpawnTimer >= spawnTimer && enemySpawned.Count <= 1)
        {
            enemyInstancePrefab = Instantiate(enemyPrefab[enemyIndex], enemyPosition.position, Quaternion.identity);
            enemySpawned.Add(enemyInstancePrefab);
                
            currentSpawnTimer = 0f;
            enemyIndex++;
        }
    }
}
