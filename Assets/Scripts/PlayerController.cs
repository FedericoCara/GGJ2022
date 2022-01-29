using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField, Range(0,100)]
    private float percentageAcceptance = 50;
    private CircleCollider2D ownCollider;

    private float ownSize;

    [SerializeField]
    private List<Ball> ballsInAcceptanceArea;

    [SerializeField]
    private GameObject inAcceptanceGO;

    [SerializeField]
    private ScoreController score;
    public ScoreController Score {
        get => score;
        set => score = value;
    }

    public Color SpriteColor {
        get => inAcceptanceSprite.color;
        set => inAcceptanceSprite.color = value;
    }

    private SpriteRenderer inAcceptanceSprite;

    private List<Ball> ballsToRemove = new List<Ball>();

    private void Awake() {
        ownCollider = GetComponentInChildren<CircleCollider2D>();
        ownSize = ownCollider.radius * ownCollider.transform.lossyScale.magnitude;
        inAcceptanceSprite = inAcceptanceGO.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (GameManager.Instance.GameFinished)
            return;

        ballsInAcceptanceArea.ForEach(ball => {
            if (Input.GetKeyDown(ball.Key)) {
                PerformAction(ball);
            }
        });
        ballsToRemove.ForEach(ballToRemove=> RemoveBallInAcceptance(ballToRemove));
        ballsToRemove.Clear();
    }

    private void PerformAction(Ball ball) {
        ballsToRemove.Add(ball);
        if (ball.Good) {
            GameManager.Instance.OnPlayerSuccess(this);
        } else {
            GameManager.Instance.OnPlayerExploded(this);
        }
        Destroy(ball.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Ball ball = collision.GetComponent<Ball>();
        if (ball != null) {
            float distance = (collision.transform.position - ownCollider.transform.position).magnitude;
            float collidingPercentage = (1 - distance / ownSize) * 100;
            Debug.Log($"Colliding with {collision.gameObject} in {collidingPercentage}%");
            if (collidingPercentage > percentageAcceptance) {
                AddBallInAcceptance(ball);
            } else {
                RemoveBallInAcceptance(ball);
            }
        }
    }

    private void RemoveBallInAcceptance(Ball ball) {
        if (ballsInAcceptanceArea.Contains(ball))
            ballsInAcceptanceArea.Remove(ball);
        inAcceptanceGO.SetActive(ballsInAcceptanceArea.Count>0);
    }

    public void AddBallInAcceptance(Ball ball) {
        if (!ballsInAcceptanceArea.Contains(ball))
            ballsInAcceptanceArea.Add(ball);
        inAcceptanceGO.SetActive(true);
    }
}
