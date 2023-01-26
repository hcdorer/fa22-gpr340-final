using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour {
    [SerializeField] string sceneName;
    
    public void playButton() {
        SceneManager.LoadScene(sceneName);
    }

    public void exitButton() {
        Application.Quit();
    }
}
