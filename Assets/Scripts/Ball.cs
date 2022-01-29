using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    [SerializeField]
    private bool good = true;
    public bool Good => good;

    [SerializeField]
    private KeyCode key;
    public KeyCode Key {
        get => key;
        set => SetKey(value);
    }

    public LineRenderer Line => path.Line;

    private Path path;
    public Path Path {
        get => path;
        set => path = value;
    }

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Canvas canvas;
    public Canvas Canvas => canvas;

    [SerializeField]
    private Text keyText;

    [SerializeField]
    private Color spriteColor;
    public Color SpriteColor {
        get => spriteColor;
        set {
            this.spriteColor = value;
            sprite.color = value;
        }
    }

    private Vector3[] positions;

    //Set speed
    public float speed;
    //Set speed
    public float horizontalMultiplier = 5;
    //Increasing value for lerp
    float moveSpeed;
    //Linerenderer's position index
    int indexNum;

    bool pathFinished = false;

    private void Start() {
        positions = new Vector3[Line.positionCount];
        Line.GetPositions(positions);
    }

    void Update() {
        if(!pathFinished)
            FollowPath();
    }

    private void FollowPath() {
        //round lerp value down to int
        indexNum = Mathf.FloorToInt(moveSpeed);
        if (indexNum > positions.Length - 1)
            pathFinished = true;

        Vector3 difBetweenVectors = positions[indexNum] - positions[indexNum + 1];
        bool horizontal = Mathf.Abs(difBetweenVectors.x / difBetweenVectors.y) > 10;
        float distanceToNextPoint = Vector3.Distance(positions[indexNum], positions[indexNum + 1]);

        //increase lerp value relative to the distance between points to keep the speed consistent.
        moveSpeed += (horizontal? horizontalMultiplier : 1)* speed / distanceToNextPoint * Time.deltaTime;

        //and lerp
        transform.position = Vector3.Lerp(positions[indexNum], positions[indexNum + 1], moveSpeed - indexNum);

        if ((transform.position - positions[positions.Length - 1]).sqrMagnitude < 0.5f) {
            OnPathFinished();
        }
    }

    private void SetKey(KeyCode key) {
        this.key = key;
        keyText.text = key.ToString();
    }

    public void OnPathFinished() {
        pathFinished = true;
        PutBackInPool(this);
    }

    private void PutBackInPool(Ball ball) {
        Destroy(ball.gameObject);
    }
}