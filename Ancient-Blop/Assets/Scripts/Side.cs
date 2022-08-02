using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Side : MonoBehaviour
{
    private string PLAYER_TAG = "Player";
    private string ENEMY_TAG = "Enemy";

    GameObject onSideMonster;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag(ENEMY_TAG)) 
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                Destroy(collision.gameObject);
            }
            else
            {
                onSideMonster = collision.gameObject;
                onSideMonster.GetComponent<Blop>().speed = -onSideMonster.GetComponent<Blop>().speed;
                onSideMonster.transform.localScale = new Vector3(-onSideMonster.transform.localScale.x, 1f, 1f);
            }
        }

    }
}
