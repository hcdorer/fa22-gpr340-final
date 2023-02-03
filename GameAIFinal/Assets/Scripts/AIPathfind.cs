using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPathfind : MonoBehaviour {
    private Path path;
    private int pathIndex;

    [SerializeField] private Vector2Int targetGridPosition;

    private CharacterMovement movement;

    public UnityEvent onTargetReached;
    [System.Serializable] public class TargetUpdatedEvent : UnityEvent<Vector3> { }
    public TargetUpdatedEvent onTargetUpdated;

    private void Awake() {
        movement = GetComponent<CharacterMovement>();
    }

    public void onNextSquareReached() {
        Square nextSquare;

        if(path == null || !path.isValid) {
            return;
        }

        if(pathIndex <= path.length - 2) {
            nextSquare = path[pathIndex + 1];
            if(movement.square != nextSquare) {
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
            Square nextSquare = path[pathIndex + 1];
            Vector2Int delta = nextSquare.GridAligned.GridPosition - movement.square.GridAligned.GridPosition;
            movement.Direction = delta;
        }
    }

    public void setPath(Square square) {
        path = new Path(movement.square, square);
        pathIndex = 0;
        setDirection();

        onTargetUpdated.Invoke(path.last.transform.position);
    }

    public void stop() {
        path = null;
    }
}
