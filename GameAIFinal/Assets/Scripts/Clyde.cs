using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : GhostBrain {
    [SerializeField] private CharacterMovement pacman;
    [SerializeField] private float fleeDistance = 8f;
    
    protected override Square getChaseTarget() {
        if(Square.gridDistanceTo(Movement.square, pacman.square) <= fleeDistance) {
            return ScatterCorner;
        }
        return LastKnownCrossroads;
    }
}
