using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [Header("Game Input")] 
    [SerializeField] private GameInput gameInput;
    private bool inputMeleeAttack;
    
    [Header("Player Attack Stats")] 
    [SerializeField] private int attackMeleePower = 35;
    [SerializeField] private int attackDashPower = 60;
    [SerializeField] private float dashAttackSpeedPower = 30f;
    private HeroMovement heroMovement;

    public int AttackDashPower
    {
        get { return attackDashPower; }
    }

    public float DashAttackSpeedPower
    {
        get { return dashAttackSpeedPower; }
    }

    [Header("Attack Mode")] 
    private bool attackMode = false;

    public bool AttackMode
    {
        get { return attackMode; }
        set { attackMode = value; }
    }
    
    [Header("Animator")] 
    private Animator heroAnimator;
    private const string ATTACKTRIGGER = "IsAttack";

    [Header("Attack Detection Point")] 
    [SerializeField] private Transform detectionPointMeleeAttack;
    [SerializeField] private Transform dashPointAttack;

    public Transform DashPointAttack
    {
        get { return dashPointAttack; }
    }

    [Header("Raycast Attack Parameter")] 
    [SerializeField] private float circleMeleeAttackCastRange = 0.5f;
    [SerializeField] private float circleDashAttackCastRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    public float CircleDashAttackCastRange
    {
        get { return circleDashAttackCastRange; }
    }
    
    public LayerMask EnemyLayer
    {
        get { return enemyLayer; }
    }

    // Fonctions de base
    private void Start()
    {
        heroAnimator = GetComponent<Animator>();
        heroMovement = GetComponent<HeroMovement>();
    }

    private void Update()
    {
        // Input from gameinput
        inputMeleeAttack = gameInput.GetInputMeleeAttack();
        
        MeleeAttack();
    }

    
    private void MeleeAttack()
    {
        if (attackMode && inputMeleeAttack)
        {
            heroAnimator.SetTrigger(ATTACKTRIGGER);
        
            // Detect ennemies around
            RaycastHit2D hitObject = Physics2D.CircleCast(
                detectionPointMeleeAttack.position, 
                circleMeleeAttackCastRange, 
                Vector2.right, 
                circleMeleeAttackCastRange, 
                enemyLayer);
            

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
    }

    
    private void OnDrawGizmos()
    {
        // Draw detection zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPointMeleeAttack.position, circleMeleeAttackCastRange);
        Gizmos.DrawWireSphere(dashPointAttack.position, circleDashAttackCastRange);
    }
}
