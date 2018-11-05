using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeastController : MonoBehaviour
{
    public bool followingPlayer = true;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private NavMeshAgent agent;

    
    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(followingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
	}
}
