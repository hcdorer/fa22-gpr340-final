using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    [SerializeField] SquareSprites sprites;

    [SerializeField] bool northWall = true, eastWall = true, southWall = true, westWall = true;
    public bool NorthWall { get => northWall; }
    public bool EastWall { get => eastWall; }
    public bool SouthWall { get => southWall; }
    public bool WestWall { get => westWall; }
    public bool walledOff { get => (northWall && eastWall && southWall && westWall); }
    public Square northNeighbor { get => getSquareAt(gridAligned.LevelGrid.CellToWorld(new Vector3Int(gridAligned.GridPosition.x, gridAligned.GridPosition.y + 1, 0))); }
    public Square eastNeighbor { get => getSquareAt(gridAligned.LevelGrid.CellToWorld(new Vector3Int(gridAligned.GridPosition.x + 1, gridAligned.GridPosition.y, 0))); }
    public Square southNeighbor { get => getSquareAt(gridAligned.LevelGrid.CellToWorld(new Vector3Int(gridAligned.GridPosition.x, gridAligned.GridPosition.y - 1, 0))); }
    public Square westNeighbor { get => getSquareAt(gridAligned.LevelGrid.CellToWorld(new Vector3Int(gridAligned.GridPosition.x - 1, gridAligned.GridPosition.y, 0))); }

    private GridAligned gridAligned;
    public GridAligned GridAligned { get => gridAligned; }

    private void OnValidate() {
        setSprite();
        setNeighborWalls();
    }

    private void Awake()
    {
        gridAligned = GetComponent<GridAligned>();
    }

    public override bool Equals(object otherObj) {
        if(otherObj == null || !this.GetType().Equals(otherObj.GetType())) {
            return false;
        } else { // we know it's actually a Square
            Square other = (Square)otherObj;
            return gridAligned.GridPosition == other.gridAligned.GridPosition && northWall == other.northWall && eastWall == other.eastWall && southWall == other.southWall && westWall == other.westWall;
        }
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

    public Square[] getNeighbors() {
        Square[] results = new Square[4];

        results[0] = northNeighbor;
        results[1] = eastNeighbor;
        results[2] = southNeighbor;
        results[3] = westNeighbor;

        return results;
    }

    public Square getNeighbor(Vector2Int direction) {
        if(direction == Vector2.up) {
            return northNeighbor;
        }
        if(direction == Vector2.right) {
            return eastNeighbor;
        }
        if(direction == Vector2.down) {
            return southNeighbor;
        }
        if(direction == Vector2.left) {
            return westNeighbor;
        }

        return null;
    }

    public static Square getSquareAt(Vector3 position) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach(Collider2D c in colliders) {
            if(c.gameObject.tag == "Square") {
                return c.GetComponent<Square>();
            }
        }

        return null;
    }

    public static float gridDistanceTo(Square first, Square second) {
        return Mathf.Sqrt(Mathf.Pow(second.gridAligned.GridPosition.x - first.gridAligned.GridPosition.x, 2.0f) + Mathf.Pow(second.gridAligned.GridPosition.y - first.gridAligned.GridPosition.y, 2.0f));
    }
}
