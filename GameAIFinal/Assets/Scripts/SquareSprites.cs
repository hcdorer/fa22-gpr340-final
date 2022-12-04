using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SquareSprites", menuName = "ScriptableObjects/SquareSprites", order = 1)]
public class SquareSprites : ScriptableObject {
    [SerializeField] Sprite[] sprites;
    [SerializeField] string[] spriteNames;

    public Sprite getSpriteByName(string name) {
        int index = Array.IndexOf(spriteNames, name);
        return sprites[index];
    }
}
