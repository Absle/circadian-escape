﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartLoadUp : MonoBehaviour
{
    public GameObject tart;

    public Button startButton;

    // Use this for initialization
    void Start()
    {
        startButton = startButton.GetComponent<Button>();

        startButton.onClick.AddListener(ToUpgrade);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ToUpgrade()
    {
        SceneManager.LoadScene(1);
    }

}