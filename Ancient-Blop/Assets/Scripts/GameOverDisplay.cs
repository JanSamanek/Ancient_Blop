using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField]
    Text gameOverText;

    void OnEnable()
    {
        Player.PlayerDiedEvent += PlayerDiedReciever;
    }

    void OnDisable()
    {
        Player.PlayerDiedEvent -= PlayerDiedReciever;
    }
    void PlayerDiedReciever()
    {
        gameOverText.text = "GAME OVER";
    }
}
