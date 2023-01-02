using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroads : GridAligned {
    public Square square { get => Square.getSquareAt(levelGrid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0))); }

    public delegate void CrossroadsReachedEventHandler(object sender, EventArgs e);
    public event CrossroadsReachedEventHandler crossroadsReachedEvent;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "PacMan") {
            crossroadsReachedEvent?.Invoke(this, new EventArgs());
        }
    }
}
