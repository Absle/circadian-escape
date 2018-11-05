using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StdT12.Interfaces;

public class Patrol : MonoBehaviour {

	public float maxDistance = 25.0f;
	//public float lookRadius = 1.0f; // Enemy's sound radius
	private GameObject player; // Refrence to our player
	private PlayerController playerController;

	// Graph
	public GameObject[] goArray = new GameObject[18];
	private Transform[] points = new Transform[18];

	private int destPoint = 0;
	private NavMeshAgent agent;

	// Get the enemy position
	public Transform enemyTransform;
	public Transform phoneTransform;
	public bool flag = false; 

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent (typeof(PlayerController)) as PlayerController;
		Debug.Log ((player==null) + " " + (playerController==null));
		//playerController = gameObject.GetComponent(typeof(PlayerController)) as PlayerController;

		for (int i = 0; i < goArray.Length; i++) {
			points [i] = goArray [i].transform;
		}

		agent.autoBraking = false;

		GoToNextPoint ();
		Debug.Log ("Start");
	}

	void GoToNextPoint(){

		if (points.Length == 0)
			return;

		agent.destination = points [destPoint].position;

		// Pick the follow up point
		//destPoint = (destPoint + 1) % points.Length;
		destPoint = ((int)(Random.Range(0.0f, 200.0f))) % points.Length;
	}
	
	// Update is called once per frame
	void Update () {

		Transform target = player.transform;
		float distance = Vector3.Distance (target.position, transform.position);

		if (playerController.isHiding) {
			if (!agent.pathPending && agent.remainingDistance < 2.0f) {
				Debug.Log ("BackToSearch");
				GoToNextPoint ();
			}
		} else {

			RaycastHit hit;
			Physics.Raycast (enemyTransform.position, phoneTransform.position - enemyTransform.position, out hit, maxDistance);

			/*
			if (flag) {
				flag = false;
				target = player.transform;
				agent.SetDestination (target.position);
				Debug.Log ("Out of sight!");
			}
			*/

			//if (Physics.Raycast (enemyTransform.position, phoneTransform.position - enemyTransform.position, out hit, maxDistance)) {
			if (hit.collider.CompareTag ("Phone")) {
				agent.SetDestination (player.transform.position);
				flag = true;
				Debug.Log ("RayCast In");
			} else if (flag) {
				flag = false;
				target = player.transform;
				agent.SetDestination (target.position);
				Debug.Log ("Out of sight!");
			}
			//}


			/*if (distance <= lookRadius) {
				Debug.Log ("lookRadius");
				agent.SetDestination (player.transform.position);
			} else */
			else if (!agent.pathPending && agent.remainingDistance < 2.0f) {
				Debug.Log ("BackToSearch");
				GoToNextPoint ();
			}
		}
	}


	/*private void HandleRaycasting(){
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance)){
			if (hit.collider.CompareTag("Player")){
				agent.SetDestination(player.transform.position);
			}
		}
	}*/



}



// Set up some location to go to randomly. 
// Use raycast to see if player is in line of sight. 
// If player is then chase, and if broken go to last known location of player. 
