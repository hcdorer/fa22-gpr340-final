using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfind : GridAligned
{
    Path path;
    int pathIndex;
    public Square square { get => Square.getSquareAt(transform.position); }

    [SerializeField] private Vector2Int targetGridPosition;

    CharacterMovement movement;

    private void Start() {
        path = new Path(square, Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0))));
        pathIndex = 0;

        movement = GetComponent<CharacterMovement>();
        movement.Direction = path.pathList[pathIndex + 1].Square.GridPosition - path.pathList[pathIndex].Square.GridPosition;
    }

    public void onTargetReached() {
        if(Square.getSquareAt(transform.position) != path.pathList[pathIndex + 1].Square) {
            return;
        }

        pathIndex++;
        Vector2Int delta = path.pathList[pathIndex + 1].Square.GridPosition - path.pathList[pathIndex].Square.GridPosition;
        movement.Direction = delta;
    }
}
