using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    private float movement_x;

    private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";
    private string WALK_ANIMATION = "Walk";
    private string JUMP_ANIMATION = "Jump";
    private string ATTACK_ANIMATION = "Attack";
    private string SHIELD_SLAM_ANIMATION = "ShieldSlam";
    private string HURT_ANIMATION = "Hurt";

    private bool isGrounded = true;
    private bool isSlaming = false;
    private bool isHurt = false;

    private Animator animator;
    private SpriteRenderer sp;
    private Rigidbody2D playerBody;
    private HealthBar healthBar;

    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private Transform shieldAttackPoint;
    [SerializeField]
    private Vector2 boxSize;
    [SerializeField]
    private Vector2 shieldBoxSize;
    private float angle = 0f;
    [SerializeField]
    private LayerMask enemyLayers;

    private int PlayerHealth = 100;
    private int PlayerAttackDamage = 50;
    private float oneAttackTime = 0.3f;
    private float nextAttackTime = 0f;

    private float BlopSpeed;

    private float hurtDir;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerBody = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBar>();
    }

    void Start()
    {
        healthBar.setMaxHealth(100);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!isSlaming && !isHurt)
        {
        MoveViaKeyboard();
        PlayerJump();
        }
        
        if (isHurt)
        {
            PlayerMove(hurtDir * (5f + BlopSpeed));
        }

        PlayerAnimate();

        if (isGrounded)
        {
                if (Time.time >= nextAttackTime)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        StartCoroutine(PlayerAttack());
                        nextAttackTime = Time.time + oneAttackTime;
                    }

                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        StartCoroutine(PlayerShieldSlam());
                        nextAttackTime = Time.time + oneAttackTime;
                    }
                }
            }

        }

    // should use fixed update for jump, but encountered problems

    /*void FixedUpdate()
    {   
        PlayerJump();
    }*/

    void MoveViaKeyboard()
    {
        movement_x = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movement_x, 0f, 0f) * speed * Time.deltaTime;
    }

    void PlayerMove(float PlayerSpeed)
    {
        transform.position += new Vector3(1f, 0f, 0f) * PlayerSpeed * Time.deltaTime;
    }

    
    void PlayerAnimate()
    {
        if (movement_x > 0)
        {
            // Player goes right
            playerBody.transform.localScale = new Vector3(1f, 1f, 1f);
            if (!isGrounded)
            {
                animator.SetBool(JUMP_ANIMATION, true);
            }
            else
            {
                animator.SetBool(JUMP_ANIMATION, false);
                animator.SetBool(WALK_ANIMATION, true);
            }
        }
        else if (movement_x < 0)
        {
            // Player goes left
            playerBody.transform.localScale = new Vector3(-1f, 1f, 1f);
            if (!isGrounded)
            {
                animator.SetBool(JUMP_ANIMATION, true);
            }
            else
            {
                animator.SetBool(JUMP_ANIMATION, false);
                animator.SetBool(WALK_ANIMATION, true);
            }
        }
        else
        {
            if (isGrounded)
            {
                animator.SetBool(JUMP_ANIMATION, false);
                animator.SetBool(WALK_ANIMATION, false);
            }
            else
            {
                animator.SetBool(WALK_ANIMATION, false);
                animator.SetBool(JUMP_ANIMATION, true);

            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool(ATTACK_ANIMATION, true);
        }
        else
        {
            animator.SetBool(ATTACK_ANIMATION, false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool(SHIELD_SLAM_ANIMATION, true);
        }
        else
        {
            animator.SetBool(SHIELD_SLAM_ANIMATION, false);
        }

        if (isHurt)
        {
            animator.SetBool(JUMP_ANIMATION, false);
            animator.SetBool(HURT_ANIMATION, true);
        }
        else
        {
            animator.SetBool(HURT_ANIMATION, false);
        }

    }

    void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {   
            isGrounded = false;
            playerBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    IEnumerator PlayerAttack()
    { 

        yield return new WaitForSeconds(0.15f);

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, angle, enemyLayers);
          
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("hit " + enemy);
            enemy.GetComponent<Blop>().BlopTakeDamage(PlayerAttackDamage);
        }
    }

    IEnumerator PlayerShieldSlam()
    {
        isSlaming = true;

        yield return new WaitForSeconds(0.3f);

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(shieldAttackPoint.position, shieldBoxSize, angle, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Shield hit " + enemy);
            StartCoroutine(enemy.GetComponent<Blop>().BlopKnockback(transform));
        }
        isSlaming = false;   
    }

    public IEnumerator PlayerTakeDamage(int enemyAttackPower, Transform t)
    {
        hurtDir = (transform.position - t.position).normalized.x;
        isHurt = true;
        PlayerHealth -= enemyAttackPower;
        healthBar.setHealth(PlayerHealth);
        if (PlayerHealth <= 0)
        {
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(0.7f);
        isHurt = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // unity calls the function if the object that the script is attached to collides with anothers object collider
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag(ENEMY_TAG))
        {
            BlopSpeed = collision.gameObject.GetComponent<Blop>().speed;
        }
    }

    void OnDrawGizmos()
    {
        //To draw the box of the attack
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, boxSize);
        Gizmos.DrawWireCube(shieldAttackPoint.position, shieldBoxSize);
    }
}
