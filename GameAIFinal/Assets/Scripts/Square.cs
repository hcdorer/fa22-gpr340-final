using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    private enum State {
        EMPTY,
        FILLED,
        X
    }

    [SerializeField] SquareSprites sprites;
    SpriteRenderer sRenderer;

    private State state;

    private void Start() {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(state == State.EMPTY) {
                state = State.FILLED;
            } else if(state == State.FILLED) {
                state = State.EMPTY;
            }

            setSprite();
        }
        if(Input.GetMouseButtonDown(1)) {
            if(state == State.EMPTY) {
                state = State.X;
            } else if(state == State.X) {
                state = State.EMPTY;
            }

            setSprite();
        }
    }

    private void setSprite() {
        switch(state) {
            case State.EMPTY:
                sRenderer.sprite = sprites.EmptySprite;
                break;
            case State.FILLED:
                sRenderer.sprite = sprites.FilledSprite;
                break;
            case State.X:
                sRenderer.sprite = sprites.XSprite;
                break;
        }
    }
}
