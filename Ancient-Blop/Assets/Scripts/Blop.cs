using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blop : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    private int BlopAttackPower = 20;
    private int BlopHealth = 100;

    private string DEATH_ANIMATION = "Death";
    private string PLAYER_TAG = "Player";

    Rigidbody2D myBody;
    Animator BlopAnimator;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        BlopAnimator = GetComponent<Animator>();

        speed = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (myBody == null)
            return;

        myBody.velocity = new Vector2(speed, myBody.velocity.y);
    }

    private IEnumerator deathAnimation()
    {
        BlopAnimator.SetBool(DEATH_ANIMATION, true);
        speed = 0f;
        Destroy(myBody);
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.75f);

        Destroy(gameObject);
    }

    public void BlopTakeDamage(int PlayerAttackDamage)
    {
        BlopHealth -= PlayerAttackDamage;
        if (BlopHealth <= 0)
        {
            StartCoroutine(deathAnimation());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            collision.gameObject.GetComponent<Player>().PlayerTakeDamage(BlopAttackPower);
            myBody.AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
        }
    }

}
  
