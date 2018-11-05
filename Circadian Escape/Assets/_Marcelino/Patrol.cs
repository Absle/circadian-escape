using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StdT12.Interfaces;

public class Patrol : MonoBehaviour {

	public float maxDistance = 10.0f;
	public float lookRadius = 5f; // Enemy's sound radius
	private GameObject player; // Refrence to our player

	// Graph
	public GameObject[] goArray = new GameObject[18];
	private Transform[] points = new Transform[18];

	private int destPoint = 0;
	private NavMeshAgent agent;

	// Get the enemy position
	public Transform enemyTransform;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		for (int i = 0; i < goArray.Length; i++) {
			points [i] = goArray [i].transform;
		}

		agent.autoBraking = false;

		GoToNextPoint ();
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

		bool flag = false;
		RaycastHit hit;
		if (Physics.Raycast(enemyTransform.position, enemyTransform.position-player.transform.position, out hit, maxDistance)){
			if (hit.collider.CompareTag("Player")){
				agent.SetDestination(player.transform.position);
				flag = true;
			}
		}

		if (distance <= lookRadius || flag)
			agent.SetDestination (player.transform.position);
		else if (!agent.pathPending && agent.remainingDistance < 2.0f)
			GoToNextPoint ();
	}


	private void HandleRaycasting(){
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance)){
			if (hit.collider.CompareTag("Player")){
				agent.SetDestination(player.transform.position);
			}
		}
	}


	/*
	// Will use this once we figure out the door issue. 
	void OnTriggerEnter(Collider other){
		if (other.gameObject.layer == 9 && other.tag == "Interactable") {
			//GameObject temp = other.gameObject;
			//Destroy (temp);
			//Destroy (other.gameObject);
			IInteractable interactable = other.gameObject.GetComponent(typeof(IInteractable)) as IInteractable;
			interactable.Interact ();
		}
	}
	*/

	/*
	void OnCollisionEnter(Collision collision){
		if (collision.tag == "DoorLayer") {
			Destroy (collision);
		}
	}
	*/
}



// Set up some location to go to randomly. 
// Use raycast to see if player is in line of sight. 
// If player is then chase, and if broken go to last known location of player. 
