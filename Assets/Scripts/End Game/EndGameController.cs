using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    [SerializeField]
    private List<ScoreController> finalScores;

    public void Show(List<ScoreController> playersScore) {
        gameObject.SetActive(true);
        for (int i = 0; i < 4; i++) {
            if (i < playersScore.Count) {
                finalScores[i].SetValues(playersScore[i]);
            } else {
                finalScores[i].gameObject.SetActive(false);
            }
        }
    }

    public void GoHome() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

}
