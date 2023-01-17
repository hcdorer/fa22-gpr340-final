using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PacManStats : MonoBehaviour {
    private int score = 0;
    public int Score { get => score; set => addPoints(value); }
    [SerializeField] private int lives = 5;

    [System.Serializable] public class ScoreUpdateEvent : UnityEvent<int> { }
    public ScoreUpdateEvent onScoreUpdate;

    [System.Serializable] public class LivesUpdateEvent : UnityEvent<int> { }
    public LivesUpdateEvent onLivesUpdate;

    private void addPoints(int value) {
        score = value;
        onScoreUpdate.Invoke(score);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Ghost") {
            GhostBrain ghost = collision.GetComponent<GhostBrain>();
            if(!ghost.respawn()) {
                lives--;
                onLivesUpdate.Invoke(lives);
            }
        }
    }
}
