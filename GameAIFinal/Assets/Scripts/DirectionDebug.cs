using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionDebug : MonoBehaviour {
    [SerializeField] CharacterMovement character;
    [SerializeField] Text dirText;
    [SerializeField] Text nextText;

    private void Update() {
        dirText.text = "Direction: " + character.Direction;
        nextText.text = "NextDir: " + character.NextDirection;
    }
}
