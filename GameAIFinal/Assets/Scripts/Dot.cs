using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : GridAligned {
    private void OnTriggerEnter2D(Collider2D collision) {
        deleteDot(collision);
    }

    protected void deleteDot(Collider2D collision) {
        if(collision.gameObject.tag == "PacMan") {
            gameObject.SetActive(false);
        }
    }
}
