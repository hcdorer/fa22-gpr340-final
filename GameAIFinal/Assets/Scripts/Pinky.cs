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
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            bool[] directionChecked = { false, false, false, false };
            for(int i = 0; i < directions.Length; i++) {
                if(directions[i] == pacman.Direction) {
                    directionChecked[i] = true;
                    break;
                }
            }
            
            Vector2Int nextDirection = currentDirection;
            bool validDirection = canMoveIntoSquare(current, nextDirection);
            while(!validDirection) {                
                int roll = Random.Range(0, directions.Length - 1);
                if(directionChecked[roll]) {
                    continue;
                }

                nextDirection = directions[roll];
                validDirection = canMoveIntoSquare(current, nextDirection);
                directionChecked[roll] = true;
            }

            currentDirection = nextDirection;
            targetPosition += currentDirection;
            current = Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0)));

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
