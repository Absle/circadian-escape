using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private bool paused;
    public GameObject pauseMenu;

    /*
    public void Start()
    {
        Cursor.visible = true;
        Screen.lockCursor = false;
    }
    */


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            paused = !paused;

        }
        if (paused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
           
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
           // Cursor.visible = false;
           // Screen.lockCursor = true;
        }

    }

    
         

 //   public void ResumeGame()
  //  {
   //     paused = false;
    //    pauseMenu.SetActive(false);
     //   Time.timeScale = 1.0f;
    //}

}
