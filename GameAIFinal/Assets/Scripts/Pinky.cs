using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : GhostBrain {
    [SerializeField] CharacterMovement pacman;

    protected override Square getChaseTarget() {
        return moveForward(pacman.square, pacman.Direction, 4);
    }

    private Square moveForward(Square current, Vector2Int direction, int maxDepth, int depth = 0) {
        if(depth >= maxDepth) {
            return current;
        }
        
        Square next = current.getNeighbor(direction);
        if(CharacterMovement.canMoveIntoSquare(next, direction)) {
            return moveForward(next, direction, maxDepth, depth + 1);
        }

        List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        if(directions.Contains(direction)) {
            directions.Remove(direction);
        }

        while(directions.Count > 0) {
            int roll = Random.Range(0, directions.Count);
            next = current.getNeighbor(directions[roll]);
            if(CharacterMovement.canMoveIntoSquare(next, directions[roll])) {
                return moveForward(next, directions[roll], maxDepth, depth + 1);
            }
            directions.Remove(directions[roll]);
        }

        return current;
    }
}
