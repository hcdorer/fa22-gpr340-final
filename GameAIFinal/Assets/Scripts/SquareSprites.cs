using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SquareSprites", menuName = "ScriptableObjects/SquareSprites", order = 1)]
public class SquareSprites : ScriptableObject {
    [SerializeField] private Sprite emptySprite;
    public Sprite EmptySprite { get => emptySprite; }
    [SerializeField] private Sprite filledSprite;
    public Sprite FilledSprite { get => filledSprite; }
    [SerializeField] Sprite xSprite;
    public Sprite XSprite { get => xSprite; }
}
