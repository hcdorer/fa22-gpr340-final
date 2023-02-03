using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadsReachedEventArgs {
    public Square square { get; private set; }

    public CrossroadsReachedEventArgs(Square square) {
        this.square = square;
    }
}

public class Crossroads : MonoBehaviour {
    public Square square { get => Square.getSquareAt(transform.position); }

    public delegate void CrossroadsReachedEventHandler(CrossroadsReachedEventArgs e);
    public event CrossroadsReachedEventHandler crossroadsReachedEvent;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "PacMan") {
            crossroadsReachedEvent?.Invoke(new CrossroadsReachedEventArgs(square));
        }

        if(collision.gameObject.tag == "Ghost") {
            collision.GetComponent<GhostBrain>().changeDirection(square);
        }
    }
}
