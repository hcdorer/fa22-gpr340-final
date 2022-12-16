using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour {
    private GameObject gridHolder;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Vector2Int startPosition;
    public Vector2Int StartPosition { get => startPosition; }
    [SerializeField] private int rows;
    public int Rows { get => rows; }
    [SerializeField] private int columns;
    public int Columns { get => columns; }

    [ContextMenu("Create Grid")]
    private void createGrid() {
        if(gridHolder != null) {
            DestroyImmediate(gridHolder);
        }

        Grid levelGrid = GetComponent<Grid>();
        initGridHolder(levelGrid);
        buildGrid(levelGrid);
    }

    private void initGridHolder(Grid levelGrid) {
        gridHolder = new GameObject();
        gridHolder.name = "GridHolder";
        gridHolder.transform.position = levelGrid.CellToWorld(new Vector3Int(startPosition.x, startPosition.y, 0));
    }

    private void buildGrid(Grid levelGrid) {
        for(int i = startPosition.y; i < startPosition.y + rows; i++) {
            for(int j = startPosition.x; j < startPosition.x + columns; j++) {
                Square square = Instantiate(squarePrefab, gridHolder.transform).GetComponent<Square>();
                square.name = "Square_" + j + "_" + i;
                square.LevelGrid = levelGrid;
                square.GridPosition = new Vector2Int(j, i);
            }
        }
    }
}
