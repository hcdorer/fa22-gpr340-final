using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : GhostBrain {
    protected override Square getChaseTarget() {
        CharacterMovement pacman = FindObjectOfType<PacManInput>().GetComponent<CharacterMovement>();
        Vector2Int targetPosition = pacman.GridPosition;

        int squaresMoved = 0;
        Square current = pacman.square;
        Vector2Int currentDirection = pacman.Direction;
        Grid levelGrid = GetComponent<GridAligned>().LevelGrid;
        while(squaresMoved < 4) {
            if(canMoveIntoSquare(current, currentDirection)) {
                targetPosition += pacman.Direction;
                current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));
            } else if(canMoveIntoSquare(current, Vector2Int.up)) {
                targetPosition += Vector2Int.up;
                current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));
            } else if(canMoveIntoSquare(current, Vector2Int.right)) {
                targetPosition += Vector2Int.right;
                current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));
            } else if(canMoveIntoSquare(current, Vector2Int.down)) {
                targetPosition += Vector2Int.down;
                current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));
            } else if(canMoveIntoSquare(current, Vector2Int.left)) {
                targetPosition += Vector2Int.left;
                current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));
            }

            squaresMoved++;
        }

        return current;
    }

    private bool canMoveIntoSquare(Square square, Vector2Int delta) {
        if(delta.y > 0) {
            return !square.NorthWall;
        }
        if(delta.x > 0) {
            return !square.EastWall;
        }
        if(delta.y < 0) {
            return !square.SouthWall;
        }
        if(delta.x < 0) {
            return !square.WestWall;
        }

        return false;
    }
}
