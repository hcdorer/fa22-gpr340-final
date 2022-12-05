using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : GridAligned {
    [SerializeField] SquareSprites sprites;

    [SerializeField] bool north, east, south, west;

    private void OnValidate() {
        setSprite();
    }

    private void setSprite() {
        string spriteName = "Square" + (north ? "N" : "") + (east ? "E" : "") + (south ? "S" : "") + (west ? "W" : "");
        GetComponent<SpriteRenderer>().sprite = sprites.getSpriteByName(spriteName);
    }
}
