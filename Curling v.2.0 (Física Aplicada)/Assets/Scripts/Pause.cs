using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)){
            isPaused = !isPaused;
        }

        if (isPaused){
            MenuOn();
        }else{
            MenuOff();
        }
	}

    void MenuOn(){
        pauseUI.SetActive(true);
        Time.timeScale = 0; //Congela Tempo;
        AudioListener.pause = true; //Congela Audio;
    }

    public void MenuOff(){
        pauseUI.SetActive(false);
        Time.timeScale = 1; //Descongela Tempo;
        AudioListener.pause = false; //Descongela Audio;
        isPaused = false;
    }
}
