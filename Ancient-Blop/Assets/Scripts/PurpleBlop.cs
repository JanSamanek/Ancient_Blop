using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBlop : Blop
{
    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        BlopAnimator = GetComponent<Animator>();

        speed = 4f;
        BlopAttackPower = 40;
        BlopHealth = 200;
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
}
