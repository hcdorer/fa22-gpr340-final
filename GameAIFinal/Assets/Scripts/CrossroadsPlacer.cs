using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadsPlacer : MonoBehaviour {
    private GameObject crossroadsHolder;
    [SerializeField] private GameObject crossroadsPrefab;

    [ContextMenu("Place Crossroads")]
    private void placeCrossroads() {
        if(crossroadsHolder != null) {
            DestroyImmediate(crossroadsHolder);
        }

        Grid levelGrid = GetComponent<Grid>();
        GridBuilder gridBuilder = GetComponent<GridBuilder>();

        initCrossroadsHolder(levelGrid, gridBuilder);
        createCrossroads(levelGrid, gridBuilder);
    }

    private void initCrossroadsHolder(Grid levelGrid, GridBuilder gridBuilder) {
        crossroadsHolder = new GameObject();
        crossroadsHolder.name = "CrossroadsHolder";
        crossroadsHolder.transform.position = levelGrid.CellToWorld(new Vector3Int(gridBuilder.StartPosition.x, gridBuilder.StartPosition.y, 0));
    }

    private void createCrossroads(Grid levelGrid, GridBuilder gridBuilder) {
        foreach(Square square in FindObjectsOfType<Square>()) {
            if(square.NorthWall && square.SouthWall) {
                continue;
            }
            if(square.EastWall && square.WestWall) {
                continue;
            }

            Crossroads crossroads = Instantiate(crossroadsPrefab, crossroadsHolder.transform).GetComponent<Crossroads>();
            crossroads.name = "Crossroads_" + square.GridAligned.GridPosition.x + "_" + square.GridAligned.GridPosition.y;
            
            GridAligned crossroadsGridAligned = crossroads.GetComponent<GridAligned>();
            crossroadsGridAligned.LevelGrid = levelGrid;
            crossroadsGridAligned.GridPosition = square.GridAligned.GridPosition;
        }
    }
}
