using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : GridAligned {
    [SerializeField] private int pointValue = 100;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "PacMan") {
            collision.GetComponent<PacManStats>().Score += pointValue;
            Destroy(gameObject);
        }
    }
}
