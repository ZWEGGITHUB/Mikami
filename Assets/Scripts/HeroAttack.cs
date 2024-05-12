using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [Header("Player Stats")] 
    [SerializeField] private int attackMeleePower = 35;
    
    [Header("Animator")] 
    private Animator heroAnimator;
    private const string ATTACKTRIGGER = "IsAttack";

    [Header("Detection Point")] 
    [SerializeField] private Transform detectionPoint;

    [Header("Layer Mask")] 
    [SerializeField] private LayerMask ennemyLayer;

    [Header("Raycast Parameter")] 
    [SerializeField] private float circleCastRange = 0.5f;

    private void Start()
    {
        heroAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        // Play Attack animation
        heroAnimator.SetTrigger(ATTACKTRIGGER);
        
        // Detect ennemis around
        RaycastHit2D hitObject = Physics2D.CircleCast(detectionPoint.position, circleCastRange, Vector2.right, circleCastRange, ennemyLayer);

        if (hitObject.transform != null)
        {
            if (hitObject.transform.TryGetComponent(out EnemyLife enemyComponent))
            {
                // Damage them
                Debug.Log("You hit an ennemy : " + enemyComponent);
                enemyComponent.TakeDamage(attackMeleePower);
            }
        }
    }

    /* private void OnDrawGizmos()
    {
        // Draw detection zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, circleCastRange);
    } */
}
