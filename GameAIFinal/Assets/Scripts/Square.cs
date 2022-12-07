using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : GridAligned {
    [SerializeField] SquareSprites sprites;

    [SerializeField] bool northWall = true, eastWall = true, southWall = true, westWall = true;
    private Square northNeighbor { get => getSquareAt(levelGrid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y + 1, 0))); }
    private Square eastNeighbor { get => getSquareAt(levelGrid.CellToWorld(new Vector3Int(gridPosition.x + 1, gridPosition.y, 0))); }
    private Square southNeighbor { get => getSquareAt(levelGrid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y - 1, 0))); }
    private Square westNeighbor { get => getSquareAt(levelGrid.CellToWorld(new Vector3Int(gridPosition.x - 1, gridPosition.y, 0))); }

    private void OnValidate() {
        setSprite();
        setNeighborWalls();
    }

    private void setSprite() {
        string spriteName = "Square" + (northWall ? "N" : "") + (eastWall ? "E" : "") + (southWall ? "S" : "") + (westWall ? "W" : "");
        GetComponent<SpriteRenderer>().sprite = sprites.getSpriteByName(spriteName);
    }

    private void setNeighborWalls() {
        if(northNeighbor != null) {
            northNeighbor.southWall = northWall;
            northNeighbor.setSprite();
        }

        if(eastNeighbor != null) {
            eastNeighbor.westWall = eastWall;
            eastNeighbor.setSprite();
        }

        if(southNeighbor != null) {
            southNeighbor.northWall = southWall;
            southNeighbor.setSprite();
        }

        if(westNeighbor != null) {
            westNeighbor.eastWall = westWall;
            westNeighbor.setSprite();
        }
    }

    private static Square getSquareAt(Vector3 position) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach(Collider2D c in colliders) {
            if(c.gameObject.tag == "Square") {
                return c.GetComponent<Square>();
            }
        }

        return null;
    }
}
