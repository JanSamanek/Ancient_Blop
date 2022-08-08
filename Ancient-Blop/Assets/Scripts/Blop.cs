using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blop : MonoBehaviour
{
    public delegate void BlopDied();
    public static event BlopDied BlopDiedEvent;

    [HideInInspector]
    public float speed;
    public int BlopAttackPower;
    public int BlopHealth;

    private string HURT_ANIMATION = "Hurt";
    private string DEATH_ANIMATION = "Death";
    private string PLAYER_TAG = "Player";

    public bool isHurt = false;
    public bool isKnockedback = false;

    public float slamDir;

    public Rigidbody2D myBody;
    public Animator BlopAnimator;
    HealthBar healthBar;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        BlopAnimator = GetComponent<Animator>();

        speed = 7f;
        BlopAttackPower = 20;
        BlopHealth = 100;
    }

    void Start()
    {

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
        if (BlopHealth > 0)
        {
            StartCoroutine(hurtAnimation());
        }
        if (BlopHealth <= 0)
        {
            if (BlopDiedEvent != null)
            {
                BlopDiedEvent();
            }
            StartCoroutine(deathAnimation());
        }
    }

    public void BlopMove(float speedBlop)
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
        if (!Player.isSlaming)
        {
            if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                StartCoroutine(collision.gameObject.GetComponent<Player>().PlayerTakeDamage(BlopAttackPower, transform));
            }
        }
    }

}
  
