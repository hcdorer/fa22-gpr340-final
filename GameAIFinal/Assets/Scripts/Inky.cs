using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : GhostBrain {
    [SerializeField] CharacterMovement pacman;
    [SerializeField] CharacterMovement pinky;
    [SerializeField] GridBuilder builder;

    protected override Square getChaseTarget() {
        Vector2Int targetPosition = pinky.GridPosition + (pacman.GridPosition - pinky.GridPosition) * 2;
        targetPosition = new Vector2Int(Mathf.Clamp(targetPosition.x, 0, builder.Columns - 1), Mathf.Clamp(targetPosition.y, 0, builder.Rows - 1));

        Square current = Square.getSquareAt(Movement.LevelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));
        while(current.walledOff) {
            foreach(Square neighbor in current.getNeighbors()) {
                if(neighbor != null) {
                    if(!neighbor.walledOff) {
                        return neighbor;
                    }
                }
            }
            if(current.northNeighbor != null) {
                current = current.northNeighbor;
            } else if(current.eastNeighbor != null) {
                current = current.eastNeighbor;
            } else if(current.southNeighbor != null) {
                current = current.southNeighbor;
            } else if(current.westNeighbor != null) {
                current = current.westNeighbor;
            }
        }

        return LastKnownCrossroads;
    }
}
