using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : GridAligned {
    private Vector2Int direction = Vector2Int.right;
    public Vector2Int Direction { get => direction; set => direction = value; }
    private Vector2Int targetGridPosition;
    [SerializeField] private float moveSpeed;

    public Square square { get => Square.getSquareAt(transform.position); }

    public UnityEvent onTargetReached;

    private void Start() {
        targetGridPosition = gridPosition + direction;
        setNextTarget();
    }

    private void Update() {
        if(lerp()) {
            gridPosition = targetGridPosition;
            setNextTarget();

            onTargetReached.Invoke();
        } else {
            setNextTarget();
        }
    }

    private void setNextTarget() {
        Vector2Int nextTarget = gridPosition + direction;
        if(canMoveIntoSquare(Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(nextTarget.x, nextTarget.y, 0))))) {
            targetGridPosition = nextTarget;
        }
    }

    private bool lerp() {
        Vector3 targetPosition = levelGrid.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0));

        if(!canMoveIntoSquare(Square.getSquareAt(targetPosition))) {
            // onLerpFail.Invoke();
            return false;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if(transform.position == targetPosition) {
            return true;
        }

        return false;
    }

    private bool canMoveIntoSquare(Square square) {
        if(square == null) {
            return false;
        }

        if(direction.y > 0) {
            return !square.SouthWall;
        }
        if(direction.x > 0) {
            return !square.WestWall;
        }
        if(direction.y < 0) {
            return !square.NorthWall;
        }
        if(direction.x < 0) {
            return !square.EastWall;
        }

        return false;
    }

    public void setRandomDirection() {
        List<Square> possibleMoves = new List<Square>();
        foreach(Square square in square.getNeighbors()) {
            if(square != null) {
                if(canMoveIntoSquare(square)) {
                    possibleMoves.Add(square);
                }
            }
        }

        int roll = Random.Range(0, possibleMoves.Count - 1);
        Square newTarget = possibleMoves[roll];
        Vector2Int delta = newTarget.GridPosition - square.GridPosition;
        direction = delta;
    }
}
