using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour {
    private GameObject gridHolder;
    [SerializeField] private Square squarePrefab;
    [SerializeField] Vector2Int startPosition;
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [ContextMenu("Create Grid")]
    void setupGrid() {
        if(GameObject.Find("GridHolder")) {
            DestroyImmediate(GameObject.Find("GridHolder"));
        }

        initGridHolder();
        buildGrid();
    }

    void initGridHolder() {
        gridHolder = new GameObject();
        gridHolder.name = "GridHolder";
        gridHolder.transform.position = GetComponent<Grid>().CellToWorld(new Vector3Int(startPosition.x, startPosition.y, 0));
    }

    void buildGrid() {
        for(int i = startPosition.y; i < startPosition.y + rows; i++) {
            for(int j = startPosition.x; j < startPosition.x + columns; j++) {
                Square square = Instantiate(squarePrefab, gridHolder.transform);
                square.name = "Square_" + j + "_" + i;
                square.LevelGrid = GetComponent<Grid>();
                square.GridPosition = new Vector2Int(j, i);
            }
        }
    }
}
