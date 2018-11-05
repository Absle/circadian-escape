using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    private bool paused;
    public GameObject pauseMenu;
    private GameObject player;
    public Button resumeButton;

    // public void Start()
    // {
    //    Cursor.visible = !Cursor.visible;
    // Screen.lockCursor = false;
    //  }

    bool resume;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        //Player.GetComponent<FirstPersonController>().enabled = true;

        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;

            if (paused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.GetComponent<FirstPersonController>().enabled = false;

            }
            else
            {
                Resume();
            }
        }



    }

    public void Resume()
    {
        
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.visible = false;
            Screen.lockCursor = true;
        resume = false;
    }
    
         



}
