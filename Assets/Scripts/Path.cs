using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Path : MonoBehaviour {

    [SerializeField]
    private LineRenderer lineRenderer;
    public LineRenderer Line => lineRenderer;

    [SerializeField]
    private Transform playerPosition;
    public Transform PlayerPosition => playerPosition;

    public Vector3 GetStartingPosition() {
        return lineRenderer.GetPosition(0);
    }
}
