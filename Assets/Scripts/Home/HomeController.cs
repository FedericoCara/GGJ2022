using Mimic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : Singleton<HomeController> { 
    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private GameObject howToPlay;

    [SerializeField]
    private List<Chair> chairs;

    [SerializeField]
    private Button playBtn;

    private static bool howToPlayShown = false;

    private int ChairCount => chairs.FindAll(chair => chair.IsOn).Count;

    private void Start() {
        for (int i = 0; i < 4; i++) {
            chairs[i].SetIsOnWithoutNotify(GameManager.PlayersSelected < 0 || i < GameManager.PlayersSelected);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        } else if (howToPlay.activeSelf && Input.GetKeyDown(KeyCode.Space)) {
            StartGame();
        }
    }

    public void OnPlay() {
        if (!howToPlayShown) {
            howToPlay.SetActive(true);
            howToPlayShown = true;
        } else {
            StartGame();
        }
    }

    public void OnCredits() {
        credits.SetActive(true);
    }

    public void StartGame() {
        GameManager.PlayersSelected = ChairCount;
        SceneManager.LoadScene("Game");
    }

    public void OnChairChanged() {
        playBtn.interactable = ChairCount > 0;
    }

}
