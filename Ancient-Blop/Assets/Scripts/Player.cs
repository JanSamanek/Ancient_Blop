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
    //private string ENEMY_TAG = "Enemy";
    private string WALK_ANIMATION = "Walk";
    private string JUMP_ANIMATION = "Jump";
    private string ATTACK_ANIMATION = "Attack";

    private bool isGrounded = true;

    private Animator animator;
    private SpriteRenderer sp;
    private Rigidbody2D playerBody;

    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private Vector2 boxSize;
    private float angle = 0f;
    [SerializeField]
    private LayerMask enemyLayers;

    private int PlayerHealth = 100;
    private int PlayerAttackDamage = 50;
    private float oneAttackTime = 0.3f;
    private float nextAttackTime = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveViaKeyboard();
        PlayerAnimate();
        PlayerJump();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(PlayerAttack());
                nextAttackTime = Time.time + oneAttackTime;
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
            Debug.Log("hit" + enemy);
            enemy.GetComponent<Blop>().BlopTakeDamage(PlayerAttackDamage);
        }
    }

    public void PlayerTakeDamage(int enemyAttackPower)
    {
        PlayerHealth -= enemyAttackPower;
        Debug.Log("player health " + PlayerHealth);
        if (PlayerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // unity calls the function if the object that the script is attached to collides with anothers object collider
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            isGrounded = true;
        }
    }

    void OnDrawGizmos()
    {
        //To draw the box of the attack
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, boxSize);
    }
}
