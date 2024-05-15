using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Collision")] 
    private BoxCollider2D projectileCollider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out HeroMovement heroComponent))
        {
            Destroy(gameObject);
        }
    }
}
