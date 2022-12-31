using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : GhostBrain {
    protected override Square getChaseTarget() {
        CharacterMovement pacman = FindObjectOfType<PacManInput>().GetComponent<CharacterMovement>();
        Vector2Int targetPosition = pacman.GridPosition;

        Square current = pacman.square;
        Vector2Int currentDirection = pacman.Direction;
        Grid levelGrid = GetComponent<GridAligned>().LevelGrid;

        int squaresMoved = 0;
        while(squaresMoved < 4) {
            List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            if(directions.Contains(currentDirection)) {
                directions.Remove(currentDirection);
            }
            
            Vector2Int nextDirection = currentDirection;
            bool validDirection = canMoveIntoSquare(current, nextDirection);
            while(!validDirection) {
                int roll = Random.Range(0, directions.Count - 1);

                nextDirection = directions[roll];
                validDirection = canMoveIntoSquare(current, nextDirection);
                directions.Remove(directions[roll]);
            }

            currentDirection = nextDirection;
            targetPosition += currentDirection;
            current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));

            squaresMoved++;
        }

        return current;
    }

    private bool canMoveIntoSquare(Square square, Vector2Int delta) {
        if(delta == Vector2.up) {
            return !square.NorthWall;
        }
        if(delta == Vector2.right) {
            return !square.EastWall;
        }
        if(delta == Vector2.down) {
            return !square.SouthWall;
        }
        if(delta == Vector2.left) {
            return !square.WestWall;
        }

        return false;
    }
}
