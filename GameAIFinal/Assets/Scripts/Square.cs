using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    [SerializeField] SquareSprites sprites;

    [SerializeField] bool north, east, south, west;

    private SpriteRenderer sRenderer;

    private void OnValidate() {
        setSprite();
    }

    private void Start() {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void setSprite() {
        string spriteName = "Square" + (north ? "N" : "") + (east ? "E" : "") + (south ? "S" : "") + (west ? "W" : "");
        sRenderer.sprite = sprites.getSpriteByName(spriteName);
    }
}
