using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : GridAligned 
{
    private Vector2Int direction = -Vector2Int.right;
    private Vector2Int Direction { set => direction = value; }
    private Vector2Int targetGridPosition;
    [SerializeField] private float moveSpeed;

    private void Start() {
        targetGridPosition = gridPosition + direction;
    }

    private void Update() {
        if(lerp()) {
            gridPosition = targetGridPosition;
            targetGridPosition += direction;
        }
    }

    private bool lerp() {
        Vector3 targetPosition = levelGrid.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0));
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if(transform.position == targetPosition) {
            return true;
        }
        return false;
    }
}
