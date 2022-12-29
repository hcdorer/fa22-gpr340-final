using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPathDebug : MonoBehaviour {
    [SerializeField] Transform targetPoint;
    [SerializeField] Transform nextPoint;

    public void updateTargetPoint(Vector3 position) {
        targetPoint.position = position;
    }

    public void updateNextPoint(Vector3 position) {
        nextPoint.position = position;
    }
}
