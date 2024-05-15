using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float enemySpeed = 1.5f;
    private Rigidbody2D rigidbodyEnemy;
    private float direction = -1f;

    [Header("Enemy Detection Player")] 
    [SerializeField] private Transform enemyDetectionPoint;
    [SerializeField] private float circleEnemyDetectionCastRange;
    [SerializeField] private LayerMask playerLayer;
    private EnemyState currentState = EnemyState.Patrol;

    public LayerMask PlayerLayer
    {
        get { return playerLayer; }
    }

    [Header("Enemy Attack")] 
    private EnemyAttack enemyAttack;
    
    private enum EnemyState
    {
        Patrol,
        AttackMelee,
        AttackRange
    }
    
    private void Start()
    {
        rigidbodyEnemy = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
    }
    
    private void Update()
    {
        CheckForPlayer();
        
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.AttackRange:
                enemyAttack.AttackRange();
                break;
            case EnemyState.AttackMelee:
                enemyAttack.AttackMelee();
                break;
            default:
                break;
        }
    }

    private void Patrol()
    {
        rigidbodyEnemy.velocity = new Vector2(direction * enemySpeed, rigidbodyEnemy.velocity.y);
    }

    private void CheckForPlayer()
    {
        RaycastHit2D hitObject = Physics2D.CircleCast(
            enemyDetectionPoint.position, 
            circleEnemyDetectionCastRange, 
            Vector2.right,
            circleEnemyDetectionCastRange, 
            playerLayer);
        
        if (hitObject.transform != null)
        {
            if (hitObject.transform.TryGetComponent(out HeroAttack heroComponent))
            {
                if (circleEnemyDetectionCastRange >= 10)
                {
                    currentState = EnemyState.AttackRange;
                }
                else if(circleEnemyDetectionCastRange <= 0.8f)
                {
                    currentState = EnemyState.AttackMelee;
                }
            }
            else
            {
                currentState = EnemyState.Patrol;
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw detection zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyDetectionPoint.position, circleEnemyDetectionCastRange);
    }
}
