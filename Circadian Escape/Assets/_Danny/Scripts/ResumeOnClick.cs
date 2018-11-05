using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class ResumeOnClick : MonoBehaviour {

    public GameObject resume;

    public Button resumeButton;

    public GameObject pauseMenu;
    public Transform Player;

    // Use this for initialization
    void Start()
    {
        resumeButton = resume.GetComponent<Button>();

        resumeButton.onClick.AddListener(Resume);

    }


    public void Resume()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Player.GetComponent<FirstPersonController>().enabled = true;
        // Cursor.visible = false;
        // Screen.lockCursor = true;
    }





}
