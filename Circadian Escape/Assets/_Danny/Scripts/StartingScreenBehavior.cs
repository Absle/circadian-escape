using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreenBehavior : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(1);

            Debug.Log("BB FUCK ME");
        }

    }
}
