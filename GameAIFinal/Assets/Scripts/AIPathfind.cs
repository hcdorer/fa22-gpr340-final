using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfind : GridAligned
{
    Path path;
    int pathIndex;
    public Square square { get { return Square.getSquareAt(transform.position); } }

    [SerializeField] private Vector2Int targetGridPosition;
    // [SerializeField] Square target;

    private void Start() {
        Debug.Log("pathfind start is being reached");
        path = new Path(square, Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0))));
        // path = new Path(square, target);
        Debug.Log("pathfinding is finished");
    }
}
