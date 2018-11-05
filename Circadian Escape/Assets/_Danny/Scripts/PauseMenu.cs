using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    private bool paused;
    public GameObject pauseMenu;
    public Transform Player;
    public Button resumeButton;
    
   // public void Start()
   // {
    //    Cursor.visible = !Cursor.visible;
       // Screen.lockCursor = false;
  //  }
    


    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;

        }

       // if (paused)
       if(paused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<FirstPersonController>().enabled = false;
        }
        else
           Resume();

    }

    public void Resume()
    {
        
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            Player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.visible = false;
             Screen.lockCursor = true;
    }
    
         

 //   public void ResumeGame()
  //  {
   //     paused = false;
    //    pauseMenu.SetActive(false);
     //   Time.timeScale = 1.0f;
    //}

}
