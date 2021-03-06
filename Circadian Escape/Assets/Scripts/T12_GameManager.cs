﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StdT12.Interfaces;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class T12_GameManager : MonoBehaviour
{
    public static T12_GameManager instance = null; //singleton instance

    public PlayerController Player { get; private set; }
    public Camera PhoneCamera { get; private set; }
    public BeastController Beast { get; private set; }
    public GameObject[] RoomGraph { get; private set; }

	private GameObject player;
	public GameObject slender;

    private void Awake()
    {
        //enforcing singleton pattern on GameManager
        if(instance == null)
        {
            instance = this;
        }

    /*    else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this)/
		*/
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Beast = GameObject.FindGameObjectWithTag("Beast").GetComponent<BeastController>();
        PhoneCamera = GameObject.FindGameObjectWithTag("Phone").GetComponentInChildren<Camera>();

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        RoomGraph = new GameObject[nodes.Length];

		player = GameObject.FindGameObjectWithTag ("Player");

        //sort RoomGraph by node names (Node1 to Node18 as of 12 Nov. 2018)
        foreach(GameObject node in nodes)
        {
            string stringdex = node.name.Replace("Node", "");
            int index;
            try
            {
                index = int.Parse(stringdex);
            }
            catch(System.Exception)
            {
                throw;
            }
            RoomGraph[index - 1] = node;
        }
    }

    public void PlayerLose()
    {
		SceneManager.LoadScene(2);
		slender.GetComponent<AudioSource>().Play();
	//	player.GetComponent<FirstPersonController>().enabled = false;
		//Invoke("Dead", 8);
		//yield WaitForSeconds(5);
       // Debug.Log("YOU LOSE!!!");
        
    }

}
