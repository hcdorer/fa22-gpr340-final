using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridAligned : MonoBehaviour {
    [SerializeField] protected Grid levelGrid; // should get this from a GameManager or something
    public Grid LevelGrid { set => levelGrid = value; }
    [SerializeField] protected Vector2Int gridPosition;
    public Vector2Int GridPosition { get => gridPosition; set => setGridPosition(value); }

    private void OnValidate() {
        snapToGrid();
        setOtherGridPositions();
    }

    private void Start() {
        snapToGrid();
    }

    [ContextMenu("Snap to Grid")]
    protected void snapToGrid() {
        transform.position = levelGrid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
    }

    private void setGridPosition(Vector2Int value) {
        gridPosition = value;
        snapToGrid();

        setOtherGridPositions();
    }

    private void setOtherGridPositions() {
        foreach(var g in GetComponents<GridAligned>().Where(g => g != this)) {
            g.gridPosition = gridPosition;
        }
    }
}
