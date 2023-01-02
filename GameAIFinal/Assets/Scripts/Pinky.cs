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
            Vector2Int nextTargetPosition = targetPosition + nextDirection;
            Square nextSquare = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(nextTargetPosition.x, nextTargetPosition.y, 0)));
            bool validDirection = canMoveIntoSquare(current, nextSquare);
            while(!validDirection) {
                int roll = Random.Range(0, directions.Count - 1);

                nextDirection = directions[roll];
                nextTargetPosition = targetPosition + nextDirection;
                nextSquare = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(nextTargetPosition.x, nextTargetPosition.y, 0)));
                validDirection = canMoveIntoSquare(current, nextSquare);
                
                directions.Remove(directions[roll]);
            }

            currentDirection = nextDirection;
            targetPosition = nextTargetPosition;
            current = nextSquare;

            squaresMoved++;
        }

        return current;
    }

    private bool canMoveIntoSquare(Square current, Square next) {
        if(next == null) {
            return false;
        }

        Vector2Int delta = next.GridPosition - current.GridPosition;

        if(delta == Vector2.up) {
            return !next.SouthWall;
        }
        if(delta == Vector2.right) {
            return !next.WestWall;
        }
        if(delta == Vector2.down) {
            return !next.NorthWall;
        }
        if(delta == Vector2.left) {
            return !next.EastWall;
        }

        return false;
    }
}
