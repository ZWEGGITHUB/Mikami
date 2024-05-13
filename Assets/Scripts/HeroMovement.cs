using System;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    [Header("Game Input")] 
    [SerializeField] private GameInput gameInput;
    private float inputVector;
    private bool inputJump;
    private bool inputDash;
    
    [Header("Movement")] 
    [SerializeField] private float playerSpeed = 10f;
    private Rigidbody2D rigibodyPlayer;
    private bool isFacingRight = true;

    [Header("Jump")] 
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Double Jump")] 
    private bool canDoubleJump = false;

    [Header("Coyote Time")] 
    [SerializeField] private float coyoteTimeDuration = 0.2f;
    private float coyoteTimeCounter;
    
    [Header("Gravity when Jumping")]
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashInterval = 0.5f;
    private float dashTimer = 0f;
    private bool canDash;
    private float dashCooldownInterval = 0f;
    
    [Header("Attack Mode")]
    private HeroAttack heroAttack;
    
    [Header("Barre de Bug")] 
    private float buggingNumber;
    
    // Event to send bugging Number
    public event EventHandler<OnBuggingEventArgs> OnBugging;

    public class OnBuggingEventArgs : EventArgs
    {
        public float buggingNumberEvent;
    }
    
    // Fonctions de base
    private void Awake()
    {
        // Récupérer le composant Rigidbody du joueur sur lui même lors du lancement du jeu
        rigibodyPlayer = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        heroAttack = GetComponent<HeroAttack>();
    }

    private void Update()
    {
        // Tous les input de touche de clavier récupéré dans le GameInput
        inputVector = gameInput.GetInputMovement();
        inputJump = gameInput.GetInputJump();
        inputDash = gameInput.GetInputDash();
        
        // Les mouvements de base avec le skin qui se retourne
        Walk(inputVector);
        Flip(inputVector);
        
        // Le jump avec une redescente plus rapide pour faire un peu celest like
        Jump(inputJump);
        ApplyMoreGravityAfterJump();
        
        // Coyote time : un petit temps lorsque le joueur qui le sol pour qu'il puisse encore sauter
        // Histoire qu'il n'aille pas besoin de faire du perfect
        StartCoyoteTime();
        
        // Le dash
        StartDash(inputDash);
        Dash();
    }
    
    #region Functions Basic Movement
    
    private void Walk(float inputVector)
    {
        rigibodyPlayer.velocity = new Vector2(inputVector * playerSpeed, rigibodyPlayer.velocity.y);
    }
    
    private void Flip(float inputVector)
    {
        if (isFacingRight && inputVector < 0f || !isFacingRight && inputVector > 0f)
        {
            isFacingRight = !isFacingRight;
            
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    #endregion

    #region Functions Jump 
    
    private void Jump(bool inputJump)
    {
        if (inputJump && (IsTouchingGround() || coyoteTimeCounter > 0f)) 
        {
            ApplyVerticalVelocityAndDoubleJump(true);
        } 
        else if(inputJump && !IsTouchingGround())
        {
            if (canDoubleJump)
            {
                ApplyVerticalVelocityAndDoubleJump(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && rigibodyPlayer.velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }
    }

    private bool IsTouchingGround()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private void ApplyVerticalVelocityAndDoubleJump(bool doubleJump)
    {
        rigibodyPlayer.velocity = new Vector2(rigibodyPlayer.velocity.x, jumpForce);
        canDoubleJump = doubleJump;

        // barre de bug dans le monde normal
        if (!heroAttack.AttackMode)
        {
            if (buggingNumber < 1)
            {
                buggingNumber += 0.05f;
            }
        
            OnBugging?.Invoke(this, new OnBuggingEventArgs()
            {
                buggingNumberEvent = buggingNumber
            });   
        }
    }

    private void ApplyMoreGravityAfterJump()
    {
        if (rigibodyPlayer.velocity.y < 0)
        {
            rigibodyPlayer.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    #endregion

    #region Functions Coyote Time

    private void StartCoyoteTime()
    {
        if (IsTouchingGround())
        {
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
   
    #endregion
    
    #region Functions Dash

    private void StartDash(bool inputDash)
    {
        if (dashCooldownInterval <= 0f && inputDash && !canDash)
        {
            canDash = true;
            dashTimer = 0f;
            dashCooldownInterval = dashInterval;

            // barre de bug uniquement dans le monde normal
            if (!heroAttack.AttackMode)
            {
                if (buggingNumber < 1)
                {
                    buggingNumber += 0.05f;   
                }
            
                OnBugging?.Invoke(this, new OnBuggingEventArgs()
                {
                    buggingNumberEvent = buggingNumber
                });   
            }
        } 
        else if (dashCooldownInterval > 0f)
        {
            dashCooldownInterval -= Time.deltaTime;
        }
    }
    
    private void Dash()
    {
        Vector2 dir = new Vector2(1, 0);

        if (!isFacingRight)
        {
            dir = new Vector2(-1, 0);
        }
        
        if (canDash)
        {
            dashTimer += Time.deltaTime;

            if (dashTimer < dashDuration)
            {
                if (!heroAttack.AttackMode)
                {
                    rigibodyPlayer.velocity = new Vector2(dir.x * dashSpeed, rigibodyPlayer.velocity.y);
                }
                else
                {
                    rigibodyPlayer.velocity = new Vector2(dir.x * heroAttack.DashAttackSpeedPower, rigibodyPlayer.velocity.y);
                    
                    // Detect ennemis around
                    RaycastHit2D hitObject = Physics2D.CircleCast(
                        heroAttack.DashPointAttack.position, 
                        heroAttack.CircleDashAttackCastRange, 
                        Vector2.right, heroAttack.CircleDashAttackCastRange,
                        heroAttack.EnemyLayer);
                    
                    
                    if (hitObject.transform != null)
                    {
                        if (hitObject.transform.TryGetComponent(out EnemyLife enemyComponent))
                        {
                            // Damage them
                            Debug.Log("You hit an ennemy : " + enemyComponent);
                            enemyComponent.TakeDamage(heroAttack.AttackDashPower);
                        }
                    }
                }
            }
            else
            {
                rigibodyPlayer.velocity = new Vector2(dir.x * playerSpeed, rigibodyPlayer.velocity.y);
                canDash = false;
            }
        }
    }
    
    #endregion
}
