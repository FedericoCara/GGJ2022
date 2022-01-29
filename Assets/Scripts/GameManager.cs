using Mimic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager> {
    [SerializeField]
    private List<Path> paths1;

    [SerializeField]
    private List<Path> paths2;

    [SerializeField]
    private List<Path> paths3;

    [SerializeField]
    private List<Path> paths4;

    [SerializeField]
    private List<Path> bombPaths;

    [SerializeField]
    private PlayerController playerPrefab;

    [SerializeField]
    private Ball niceBallPrefab;

    [SerializeField]
    private Ball enemyBallPrefab;

    [SerializeField]
    private Vector2 timeRangeBetweenSpawns = new Vector2(0.5f, 1);

    [SerializeField]
    private Vector2 timeRangeBetweenBombSpawns = new Vector2(5, 10);

    [SerializeField]
    private List<ScoreController> scores;

    public static int PlayersSelected { get; set; } = -1;

    public static List<Color> playerColors = new List<Color> { 
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    [SerializeField]
    private int playersCount = 4;

    [SerializeField]
    private List<PlayerController> players;

    [SerializeField]
    private int scorePerSuccess = 10;

    [SerializeField]
    private int scorePerExplosion = -50;

    [SerializeField]
    private float gameDurationSeconds = 120;

    private float timeLeft;
    public float TimeLeft {
        get => timeLeft;
        private set {
            timeLeft = value;
            timeLeftTxt.text = $"Tiempo restante: {value.ToString("##.#")}s";
        }
    }
    [SerializeField]
    private Text timeLeftTxt;

    private bool gameFinished = false;
    public bool GameFinished => gameFinished;

    private List<Path> bombPathsLeft = new List<Path>();

    void Start()
    {
        if (PlayersSelected > 0)
            playersCount = PlayersSelected;
        TurnOffAllPathsAndScores();
        TurnOnCorrectPath();
        for (int i = 0; i < playersCount; i++) {
            SpawnPlayer(i);
            StartCoroutine(SpawnBalls(i));
        }
        StartCoroutine(SpawnEnemyBalls());

        timeLeft = gameDurationSeconds;
    }

    private void Update() {
        if (!gameFinished) {
            TimeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                timeLeft = 0;
                FinishGame();
            }
        }
    }

    private void FinishGame() {
        Time.timeScale = 0;

    }

    private PlayerController SpawnPlayer(int playerIndex) {
        ScoreController score = scores[playerIndex];
        score.gameObject.SetActive(true);
        score.Name = $"Player {playerIndex + 1}";
        score.Score = 0;

        PlayerController newPlayer = Instantiate<PlayerController>(playerPrefab, GetPathList()[playerIndex].PlayerPosition);
        newPlayer.SpriteColor = playerColors[playerIndex];
        newPlayer.Score = score;
        newPlayer.Score.Color = newPlayer.SpriteColor;
        return newPlayer;
    }

    private void TurnOffAllPathsAndScores() {
        paths1.ForEach(path => path.gameObject.SetActive(false));
        paths2.ForEach(path => path.gameObject.SetActive(false));
        paths3.ForEach(path => path.gameObject.SetActive(false));
        paths4.ForEach(path => path.gameObject.SetActive(false));
        scores.ForEach(score => score.gameObject.SetActive(false));
    }

    private void TurnOnCorrectPath() {
        GetPathList().ForEach(path => path.gameObject.SetActive(true));
    }

    private IEnumerator SpawnBalls(int pathIndex) {
        while (true) {
            yield return new WaitForSeconds(Random.Range(timeRangeBetweenSpawns.x, timeRangeBetweenSpawns.y));
            Path path = GetPath(pathIndex);
            Ball newBall = Instantiate<Ball>(niceBallPrefab, path.transform);
            InitBall(path, newBall);
            newBall.SpriteColor = Color.yellow;
        }
    }

    private static void InitBall(Path path, Ball newBall) {
        newBall.transform.position = path.GetStartingPosition();
        newBall.Key = (KeyCode)Random.Range(97, 123);
        newBall.Path = path;
        newBall.Canvas.worldCamera = Camera.main;
    }

    private IEnumerator SpawnEnemyBalls() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(timeRangeBetweenBombSpawns.x, timeRangeBetweenBombSpawns.y));
            Path bombPath = GetBombPath();
            Ball newBomb = Instantiate<Ball>(enemyBallPrefab, bombPath.transform);
            InitBall(bombPath, newBomb);
        }
    }

    private Path GetBombPath() {
        if (bombPathsLeft.Count <= 0)
            bombPathsLeft = new List<Path>(bombPaths);
        return bombPathsLeft.RemoveElementAtRandom<Path>();
    }

    public Path GetPath(int pathIndex) {
        List<Path> pathsList = GetPathList();
        return pathsList[pathIndex];
    }

    private List<Path> GetPathList() {
        switch (playersCount) {
            case 1:
                return paths1;
            case 2:
                return paths2;
            case 3:
                return paths3;
            case 4:
                return paths4;
            default:
                Debug.LogError($"{playersCount} player amount not handled");
                return paths4;
        }
    }

    public void OnPlayerSuccess(PlayerController playerController) {
        playerController.Score.Score += scorePerSuccess;
    }

    public void OnPlayerExploded(PlayerController playerController) {
        playerController.Score.Score += scorePerExplosion;
    }
}
