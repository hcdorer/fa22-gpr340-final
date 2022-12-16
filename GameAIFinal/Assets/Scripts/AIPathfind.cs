using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPathfind : GridAligned
{
    Path path;
    int pathIndex;
    public Square square { get => Square.getSquareAt(transform.position); }

    [SerializeField] private Vector2Int targetGridPosition;

    private CharacterMovement movement;
    public CharacterMovement Movement { get => movement; }

    public UnityEvent onTargetReached;

    private void Start() {
        movement = GetComponent<CharacterMovement>();
    }

    public void onNextSquareReached() {
        Square nextSquare;
        if(pathIndex <= path.length - 2) {
            nextSquare = path.pathList[pathIndex + 1].Square;
            if(square != nextSquare) {
                return;
            }
        }

        if(pathIndex == path.length - 1) {
            onTargetReached.Invoke();
        }

        pathIndex++;
        setDirection();
    }

    private void setDirection() {
        if(pathIndex <= path.length - 2) {
            Square nextSquare = path.pathList[pathIndex + 1].Square;
            Vector2Int delta = nextSquare.GridPosition - square.GridPosition;
            if(movement != null) {
                movement.Direction = delta;
            }
        }
    }

    public void setPath(Square square) {
        Debug.Log(gameObject.name + " pathfinding to " + square.name);
        path = new Path(this.square, square);
        pathIndex = 0;
        setDirection();
    }
}
