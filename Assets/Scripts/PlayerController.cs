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

    private List<Ball> ballsToRemove = new List<Ball>();

    private void Awake() {
        ownCollider = GetComponentInChildren<CircleCollider2D>();
        ownSize = ownCollider.radius * ownCollider.transform.lossyScale.magnitude;
    }

    private void Update() {
        ballsInAcceptanceArea.ForEach(ball => {
            if (Input.GetKeyDown(ball.Key)) {
                PerformAction(ball);
            }
        });
        ballsToRemove.ForEach(ballToRemove=> {
            RemoveBallInAcceptance(ballToRemove);

            Destroy(ballToRemove.gameObject);
        });
        ballsToRemove.Clear();
    }

    private void PerformAction(Ball ball) {
        ballsToRemove.Add(ball);
        
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
