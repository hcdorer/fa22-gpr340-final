using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : GridAligned {
    private Vector2Int direction = Vector2Int.right;
    private Vector2Int nextDirection = Vector2Int.right;
    public Vector2Int Direction { set => direction = value; }
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
}
