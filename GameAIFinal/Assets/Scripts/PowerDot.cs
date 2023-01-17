using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDot : Dot {
    private const int POINT_VALUE = 500;

    public delegate void PowerDotCollectedEventHandler();
    public event PowerDotCollectedEventHandler powerDotCollectedEvent;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "PacMan") {
            collision.GetComponent<PacManStats>().Score += POINT_VALUE;
            powerDotCollectedEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}
