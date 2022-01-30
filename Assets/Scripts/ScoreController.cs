using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    [SerializeField]
    private Text playerNameTxt;
    public string Name {
        get => playerNameTxt.text;
        set => playerNameTxt.text = value;
    }
    [SerializeField]
    private Text playerScoreTxt;

    private int score;
    public int Score {
        get => score;
        set {
            score = value;
            playerScoreTxt.text = value.ToString();
        }
    }

    public void SetValues(ScoreController scoreController) {
        Name = scoreController.Name;
        Score = scoreController.Score;
        Color = scoreController.Color;
    }

    public Color Color {
        get => playerNameTxt.color;
        set {
            playerNameTxt.color = value;
            playerScoreTxt.color = value;
        }
    }
}
