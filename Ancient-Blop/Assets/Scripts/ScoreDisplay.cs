using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    public static int score = 0;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void OnEnable()
    {
        Blop.BlopDiedEvent += BlopDiedReciever;
    }

    void OnDisable()
    {
        Blop.BlopDiedEvent -= BlopDiedReciever;
    }

    void BlopDiedReciever(GameObject BlopType)
    {
        if(BlopType.name == "Blop(Clone)")
        {
            score += 50;
        }
        else if (BlopType.name == "PurpleBlop(Clone)")
        {
            score += 100;
        }
    }
}
