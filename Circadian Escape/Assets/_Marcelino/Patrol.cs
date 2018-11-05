using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour {

	// Idea
	public GameObject[] goArray = new GameObject[4];
	private Transform[] points = new Transform[4];

	//public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();

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
		destPoint = (destPoint + 1) % points.Length;
	}
	
	// Update is called once per frame
	void Update () {

		if (!agent.pathPending && agent.remainingDistance < 2.0f)
			GoToNextPoint ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Test") {
			//GameObject temp = other.gameObject;
			//Destroy (temp);
			Destroy (other.gameObject);
		}
	}

	/*
	void OnCollisionEnter(Collision collision){
		if (collision.tag == "DoorLayer") {
			Destroy (collision);
		}
	}
	*/
}
