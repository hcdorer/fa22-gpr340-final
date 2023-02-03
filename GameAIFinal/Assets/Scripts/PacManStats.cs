using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class PacManStats : MonoBehaviour {
    private int score = 0;
    public int Score { get => score; set => addPoints(value); }
    private int lives = 5;

    private const float I_FRAME_LENGTH = 3f;
    private bool iFrames;
    public bool IFrames { get => iFrames; }
    private float iFrameTimer;

    private CharacterMovement movement;

    [System.Serializable] public class ScoreUpdateEvent : UnityEvent<int> { }
    public ScoreUpdateEvent onScoreUpdate;

    [System.Serializable] public class LivesUpdateEvent : UnityEvent<int> { }
    public LivesUpdateEvent onLivesUpdate;

    private void Awake() {
        movement = GetComponent<CharacterMovement>();
    }

    private void Update() {
        if(iFrames) {
            iFrameTimer -= Time.deltaTime;
            if(iFrameTimer <= 0) {
                iFrames = false;
            }
        }
    }

    private void addPoints(int value) {
        score = value;
        onScoreUpdate.Invoke(score);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Ghost") {
            if(iFrames) {
                return;
            }

            GhostBrain ghost = collision.GetComponent<GhostBrain>();
            if(!ghost.respawn(this)) {
                lives--;
                onLivesUpdate.Invoke(lives);

                iFrames = true;
                iFrameTimer = I_FRAME_LENGTH;

                movement.respawn();
            }
        }
    }
}
