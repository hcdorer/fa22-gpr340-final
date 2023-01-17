using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;

    public void updateScoreText(int score) {
        scoreText.text = "Score: " + score;
    }

    public void updateLivesText(int lives) {
        livesText.text = "Lives: " + lives;
    }
}
