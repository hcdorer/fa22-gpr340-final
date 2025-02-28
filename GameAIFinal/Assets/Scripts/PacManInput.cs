using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManInput : MonoBehaviour {
    private CharacterMovement movement;
    private PacManStats stats;

    private void Awake() {
        movement = GetComponent<CharacterMovement>();
        stats = GetComponent<PacManStats>();
    }

    private void Update() {
        setDirection();
    }

    private void setDirection() {
        if(Input.GetKeyDown(KeyCode.W)) {
            movement.Direction = Vector2Int.up;
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            movement.Direction = Vector2Int.right;
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            movement.Direction = Vector2Int.down;
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            movement.Direction = Vector2Int.left;
        }
    }
}
