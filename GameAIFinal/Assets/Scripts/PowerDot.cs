using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDot : Dot {
    public delegate void PowerDotCollectedEventHandler();
    public event PowerDotCollectedEventHandler powerDotCollectedEvent;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "PacMan") {
            Destroy(gameObject);
            powerDotCollectedEvent?.Invoke();
        }
    }
}
