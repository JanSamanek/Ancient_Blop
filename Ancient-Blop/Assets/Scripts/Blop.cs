using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blop : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    private int BlopAttackPower = 20;
    private int BlopHealth = 100;

    private string HURT_ANIMATION = "Hurt";
    private string DEATH_ANIMATION = "Death";
    private string PLAYER_TAG = "Player";

    private bool isHurt = false;
    private bool isKnockedback = false;

    private float slamDir;

    Rigidbody2D myBody;
    Animator BlopAnimator;
    HealthBar healthBar;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        BlopAnimator = GetComponent<Animator>();
        healthBar = GetComponent<HealthBar>();

        speed = 7f;
    }

    void Start()
    {
       // healthBar.setMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHurt)
        {
            BlopMove(0);
        }
        else if (isKnockedback)
        {
            BlopMove(slamDir * 20f);
        }
        else
        {
        BlopMove(speed);
        }
    }

    private IEnumerator deathAnimation()
    {
        BlopAnimator.SetBool(DEATH_ANIMATION, true);
        speed = 0f;
        Destroy(myBody);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.75f);

        Destroy(gameObject);
    }

    private IEnumerator hurtAnimation()
    {
        isHurt = true;
        BlopAnimator.SetBool(HURT_ANIMATION, true);
        yield return new WaitForSeconds(0.15f);
        BlopAnimator.SetBool(HURT_ANIMATION, false);
        isHurt=false;
    }

    public void BlopTakeDamage(int PlayerAttackDamage)
    {
        BlopHealth -= PlayerAttackDamage;
        //healthBar.setHealth(BlopHealth);
        if (BlopHealth > 0)
        {
            StartCoroutine(hurtAnimation());
        }
        if (BlopHealth <= 0)
        {
            StartCoroutine(deathAnimation());
        }
    }

    void BlopMove(float speedBlop)
    {
        transform.position += new Vector3(1f, 0f, 0f) * speedBlop * Time.deltaTime;
    }

    public IEnumerator BlopKnockback(Transform t)
    {
        slamDir = (transform.position - t.position).normalized.x;

        isKnockedback = true;
        yield return new WaitForSeconds(0.3f);
        isKnockedback = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            collision.gameObject.GetComponent<Player>().PlayerTakeDamage(BlopAttackPower);
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 0f), ForceMode2D.Impulse);
        }
    }

}
  
