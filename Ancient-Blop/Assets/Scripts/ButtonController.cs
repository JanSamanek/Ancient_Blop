using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void HomeButtone()
    {
        SceneManager.LoadScene("MainMenu");
        ScoreDisplay.score = 0;
    }

    public void ResetButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreDisplay.score = 0;
    }
}
