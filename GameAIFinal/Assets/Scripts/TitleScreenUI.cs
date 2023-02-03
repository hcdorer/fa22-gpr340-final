using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour {
    [SerializeField] string gameSceneName;
    
    public void playButton() {
        SceneManager.LoadScene(gameSceneName);
    }

    public void exitButton() {
        Application.Quit();
    }
}
