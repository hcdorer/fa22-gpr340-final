using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : GridAligned {
    [SerializeField] GridAligned exit;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag != "PacMan") {
            return;
        }

        CharacterMovement pacman = collision.GetComponent<CharacterMovement>();
        pacman.teleport(exit.GridPosition);
    }
}
