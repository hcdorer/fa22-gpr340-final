using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DotPlacer : MonoBehaviour {
    private GameObject dotHolder;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject powerDotPrefab;
    [SerializeField] private Vector2Int[] powerDotGridPositions;

    [ContextMenu("Place Dots")]
    private void placeDots() {
        if(dotHolder != null) {
            DestroyImmediate(dotHolder);
        }

        Grid levelGrid = GetComponent<Grid>();
        GridBuilder gridBuilder = GetComponent<GridBuilder>();

        initDotHolder(levelGrid, gridBuilder);
        createDots(levelGrid);
    }

    private void initDotHolder(Grid levelGrid, GridBuilder gridBuilder) {
        dotHolder = new GameObject();
        dotHolder.name = "DotHolder";
        dotHolder.transform.position = levelGrid.CellToWorld(new Vector3Int(gridBuilder.StartPosition.x, gridBuilder.StartPosition.y, 0));
    }

    private void createDots(Grid levelGrid) {
        foreach(Square square in FindObjectsOfType<Square>()) {
            if(!square.NorthWall || !square.EastWall || !square.SouthWall || !square.WestWall) {
                Dot dot;
                Vector2Int newPos = new Vector2Int(square.GridAligned.GridPosition.x, square.GridAligned.GridPosition.y);

                if(powerDotGridPositions.Contains(newPos)) {
                    dot = Instantiate(powerDotPrefab, dotHolder.transform).GetComponent<Dot>();
                    dot.name = "PowerDot_" + square.GridAligned.GridPosition.x + "_" +square.GridAligned.GridPosition.y;
                } else {
                    dot = Instantiate(dotPrefab, dotHolder.transform).GetComponent<Dot>();
                    dot.name = "Dot_" + square.GridAligned.GridPosition.x + "_" + square.GridAligned.GridPosition.y;
                }

                GridAligned dotGridAligned = dot.GetComponent<GridAligned>();
                dotGridAligned.LevelGrid = GetComponent<Grid>();
                dotGridAligned.GridPosition = new Vector2Int(square.GridAligned.GridPosition.x, square.GridAligned.GridPosition.y);
            }
        }
    }
}
