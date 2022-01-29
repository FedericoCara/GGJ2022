using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Path> paths1;

    [SerializeField]
    private List<Path> paths2;

    [SerializeField]
    private List<Path> paths3;

    [SerializeField]
    private List<Path> paths4;

    [SerializeField]
    private List<PlayerController> players;

    [SerializeField]
    private Ball niceBallPrefab;

    [SerializeField]
    private Vector2 timeRangeBetweenSpawns = new Vector2(0.5f,1);

    public static int PlayersSelected { get; set; } = -1;

    [SerializeField]
    private int playersCount = 4;

    void Start()
    {
        if (PlayersSelected > 0)
            playersCount = PlayersSelected;
        TurnOffAllPaths();
        TurnOnCorrectPath();
        for (int i = 0; i < playersCount; i++) {
            StartCoroutine(SpawnBalls(i));
        }
    }

    private void TurnOffAllPaths() {
        paths1.ForEach(path => path.gameObject.SetActive(false));
        paths2.ForEach(path => path.gameObject.SetActive(false));
        paths3.ForEach(path => path.gameObject.SetActive(false));
        paths4.ForEach(path => path.gameObject.SetActive(false));
    }

    private void TurnOnCorrectPath() {
        GetPathList().ForEach(path => path.gameObject.SetActive(true));
    }

    private IEnumerator SpawnBalls(int pathIndex) {
        while (true) {
            yield return new WaitForSeconds(Random.Range(timeRangeBetweenSpawns.x,timeRangeBetweenSpawns.y));
            Path path = GetPath(pathIndex);
            Ball newBall = Instantiate<Ball>(niceBallPrefab, path.transform);
            newBall.transform.position = path.GetStartingPosition();
            newBall.Key = (KeyCode) Random.Range(97,123);
            newBall.Path = path;
            newBall.Canvas.worldCamera = Camera.main;
            newBall.SpriteColor = Color.yellow;
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
}
