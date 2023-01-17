using System.Collections;
using System.Collections.Generic;
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

    [System.Serializable] public class ScoreUpdateEvent : UnityEvent<int> { }
    public ScoreUpdateEvent onScoreUpdate;

    [System.Serializable] public class LivesUpdateEvent : UnityEvent<int> { }
    public LivesUpdateEvent onLivesUpdate;

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
            if(!ghost.respawn()) {
                lives--;
                onLivesUpdate.Invoke(lives);

                iFrames = true;
                iFrameTimer = I_FRAME_LENGTH;
            }
        }
    }
}
