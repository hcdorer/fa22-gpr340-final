using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShower : MonoBehaviour {
    [SerializeField] Transform targetShower;

    public void updateTargetShower(Vector3 position) {
        targetShower.position = position;
    }
}
