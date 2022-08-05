using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackTrigger : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        var BlopEnemy = collision.gameObject.GetComponent<Blop>();

        if (BlopEnemy != null)
        {
        }
    }
    
}
