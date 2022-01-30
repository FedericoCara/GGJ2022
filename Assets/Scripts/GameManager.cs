using Mimic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Path> bombPaths1;

    [SerializeField]
    private List<Path> bombPaths2;

    [SerializeField]
    private List<Path> bombPaths3;

    [SerializeField]
    private List<Path> bombPaths4;

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

    [SerializeField]
    private List<Color> playerColors = new List<Color> {
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    [SerializeField]
    private int playersCount = 4;

    [SerializeField]
    private List<PlayerController> players = new List<PlayerController>();

    [SerializeField]
    private int scorePerSuccess = 10;

    [SerializeField]
    private int scorePerExplosion = -50;

    [SerializeField]
    private float gameDurationSeconds = 120;

    [SerializeField]
    private float speedMultiplierStartPhase = 0.5f;

    [SerializeField]
    private float speedMultiplierFinalPhase = 2f;

    private float timeLeft;
    public float TimeLeft {
        get => timeLeft;
        private set {
            timeLeft = value;
            timeLeftTxt.text = $"Tiempo restante: {value.ToString("##.0")}s";
        }
    }
    [SerializeField]
    private Text timeLeftTxt;

    [SerializeField]
    private EndGameController endGameController;

    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private List<AudioClip> gameMusics;

    [SerializeField]
    private AudioClip endGameMusic;

    private bool gameFinished = false;
    public bool GameFinished => gameFinished;

    private List<Path> bombPathsLeft = new List<Path>();

    private float elapsedTime = 0;
    public bool StartingPhase => elapsedTime < gameDurationSeconds / 5f;
    public bool FinalPhase => elapsedTime > gameDurationSeconds * 4 / 5f;

    private List<AudioClip> gameMusicsLeft = new List<AudioClip>();

    void Start() {
        if (PlayersSelected > 0) {
            playersCount = PlayersSelected;
        } else {
            PlayersSelected = playersCount;
        }
        TurnOffAllPathsAndScores();
        TurnOnCorrectPath();
        for (int i = 0; i < playersCount; i++) {
            players.Add(SpawnPlayer(i));
            StartCoroutine(SpawnBalls(i));
        }
        StartCoroutine(SpawnEnemyBalls());

        timeLeft = gameDurationSeconds;

        if (gameMusicsLeft.Count <= 0)
            gameMusicsLeft = new List<AudioClip>(gameMusics);
        musicAudioSource.clip = gameMusicsLeft.RemoveElementAtRandom();
        musicAudioSource.Play();
    }

    private void Update() {
        if (!gameFinished) {
            TimeLeft -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            if (timeLeft <= 0) {
                gameFinished = true;
                timeLeft = 0;
                FinishGame();
            }
        }
    }

    private void FinishGame() {
        Time.timeScale = 0;
        musicAudioSource.loop = false;
        musicAudioSource.Stop();
        musicAudioSource.clip = endGameMusic;
        musicAudioSource.Play();
        List<ScoreController> lastScores = new List<ScoreController>(playersCount);
        players.ForEach(player=> lastScores.Add(player.Score));
        lastScores = lastScores.OrderByDescending(score => score.Score).ToList();
        endGameController.Show(lastScores);
    }

    private PlayerController SpawnPlayer(int playerIndex) {
        ScoreController score = scores[playerIndex];
        score.gameObject.SetActive(true);
        score.Name = $"Player {playerIndex + 1}";
        score.Score = 0;

        PlayerController newPlayer = Instantiate<PlayerController>(playerPrefab, GetPathList()[playerIndex].PlayerPosition);
        newPlayer.SpriteColor = playerIndex;
        newPlayer.Score = score;
        newPlayer.Score.Color = playerColors[playerIndex];
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
        float waitTimeMultiplier = speedMultiplierStartPhase;
        while (true) {
            yield return new WaitForSeconds(Random.Range(timeRangeBetweenSpawns.x, timeRangeBetweenSpawns.y) / waitTimeMultiplier);
            Path path = GetPath(pathIndex);
            Ball newBall = Instantiate<Ball>(niceBallPrefab, path.transform);
            InitBall(path, newBall);
            newBall.SpriteColor = Color.yellow;
            if (FinalPhase) {
                waitTimeMultiplier = speedMultiplierFinalPhase;
            } else if (!StartingPhase) {
                waitTimeMultiplier = 1;
            }
        }
    }

    private static void InitBall(Path path, Ball newBall) {
        newBall.transform.position = path.GetStartingPosition();
        newBall.Key = (KeyCode)Random.Range(97, 123);
        newBall.Path = path;
        newBall.Canvas.worldCamera = Camera.main;
    }

    private IEnumerator SpawnEnemyBalls() {
        float waitTimeMultiplier = speedMultiplierStartPhase;
        float playerCountMultiplier = 1;
        switch (playersCount) {
            case 2:
                playerCountMultiplier = 1.5f;
                break;
            case 3:
                playerCountMultiplier = 5f / 4f;
                break;
        }
        while (true) {
            yield return new WaitForSeconds(Random.Range(timeRangeBetweenBombSpawns.x, timeRangeBetweenBombSpawns.y) / waitTimeMultiplier * playerCountMultiplier);
            Path bombPath = GetBombPath();
            Ball newBomb = Instantiate<Ball>(enemyBallPrefab, bombPath.transform);
            InitBall(bombPath, newBomb);

            if (FinalPhase) {
                waitTimeMultiplier = speedMultiplierFinalPhase;
            } else if (!StartingPhase) {
                waitTimeMultiplier = 1;
            }
        }
    }

    private Path GetBombPath() {
        if (bombPathsLeft.Count <= 0)
            bombPathsLeft = new List<Path>(GetBombPaths());
        return bombPathsLeft.RemoveElementAtRandom<Path>();
    }

    public List<Path> GetBombPaths() {
        switch (playersCount) {
            case 1:
                return bombPaths1;
            case 2:
                return bombPaths2;
            case 3:
                return bombPaths3;
            case 4:
                return bombPaths4;
            default:
                Debug.LogError($"{playersCount} player amount not handled");
                return bombPaths4;
        }
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
