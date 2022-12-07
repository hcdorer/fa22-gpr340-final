using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotPlacer : MonoBehaviour {
    private GameObject dotHolder;
    [SerializeField] private Dot dotPrefab;

    [ContextMenu("Place Dots")]
    void placeDots() {
        if(dotHolder != null) {
            DestroyImmediate(dotHolder);
        }

        Grid levelGrid = GetComponent<Grid>();
        GridBuilder gridBuilder = GetComponent<GridBuilder>();

        initDotHolder(levelGrid, gridBuilder);
        createDots(levelGrid, gridBuilder);
    }

    private void initDotHolder(Grid levelGrid, GridBuilder gridBuilder) {
        dotHolder = new GameObject();
        dotHolder.name = "DotHolder";
        dotHolder.transform.position = levelGrid.CellToWorld(new Vector3Int(gridBuilder.StartPosition.x, gridBuilder.StartPosition.y, 0));
    }

    private void createDots(Grid levelGrid, GridBuilder gridBuilder) {
        for(int i = gridBuilder.StartPosition.y; i < gridBuilder.StartPosition.y + gridBuilder.Rows; i++) {
            for(int j = gridBuilder.StartPosition.x; j < gridBuilder.StartPosition.x + gridBuilder.Columns; j++) {
                Square current = Square.getSquareAt(GetComponent<Grid>().CellToWorld(new Vector3Int(j, i, 0)));
                if(!current.NorthWall || !current.EastWall || !current.SouthWall || !current.WestWall) {
                    Dot dot = Instantiate(dotPrefab, dotHolder.transform);
                    dot.name = "Dot_" + j + "_" + i;
                    dot.LevelGrid = GetComponent<Grid>();
                    dot.GridPosition = new Vector2Int(j, i);
                }
            }
        }
    }
}
