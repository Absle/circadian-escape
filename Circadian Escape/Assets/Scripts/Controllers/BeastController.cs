using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StdT12;
using StdT12.Enums;
using UnityEngine.SceneManagement;

public class BeastController : MonoBehaviour
{
    private T12_GameManager manager = null;
    private PlayerController player = null;
    private Camera phone = null;
    private GameObject[] rooms = null;
    private NavMeshAgent agent = null;

    public BeastAIState CurrentState { get; private set; }
    public GameObject CurrentTarget { get; private set; }

    private bool firstUpdate;
    private bool canSeePlayer;
    private bool visibleToPlayer;
    private bool stateChanged;
    private int numRoomSearched;
    private float playerDistance;
    private float targetWanderDistance = 1.0f;
    private Vector3 lastKnownPlayerPos;
    private int maxSearchRooms = 1;
    [SerializeField]
    private int maxConsiderSearch = 3;
    [SerializeField]
    private float minApproachDistance = 5.0f;
    //[SerializeField]
    //private float maxApproachTime = 60.0f;
    [SerializeField]
    private float wanderSpeed = 3.0f;
    [SerializeField]
    private float approachSpeed = 3.0f;
    [SerializeField]
    private float pursueSpeed = 5.0f;
    [SerializeField]

    private float maxSightDistance = 1000.0f;


	 public float LOSE_DISTANCE = 1.5f;
	 [SerializeField]
	 private float attackRange = 1.5f;


    
    private delegate void StateBehavior();
    private StateBehavior tutorial;
    private StateBehavior wander;
    private StateBehavior approach;
    private StateBehavior pursue;
    private StateBehavior search;
    private StateBehavior currentBehavior;

    // Use this for initialization
	private void Start()
    {
        manager = FindObjectOfType<T12_GameManager>();

        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        CurrentState = BeastAIState.Tutorial;

        firstUpdate = true;
        canSeePlayer = false;
        visibleToPlayer = false;
        stateChanged = false;
        playerDistance = 100.0f; //just something big until the first update
        numRoomSearched = 0;

        tutorial = new StateBehavior(StateTutorialBehavior);
        wander = new StateBehavior(StateWanderBehavior);
        approach = new StateBehavior(StateApproachBehavior);
        pursue = new StateBehavior(StatePursueBehavior);
        search = new StateBehavior(StateSearchBehavior);
        currentBehavior = tutorial;
    }

    // Update is called once per frame
    private void Update()
    {

        if(firstUpdate)
        {
            firstUpdate = false;
            player = manager.Player;
            phone = manager.PhoneCamera;
            rooms = manager.RoomGraph;
        }

		if(Vector3.Distance(player.transform.position, gameObject.transform.position) < LOSE_DISTANCE)
                {
                   // SceneManager.LoadScene(2);
                    Debug.Log("YOU DIED!!!");
               }
        //first, determine state flags
        //check if player is in line of sight
        RaycastHit hit;
        Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, maxSightDistance);
        if(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Phone"))
        {
            //?
            /*
            if(!canSeePlayer)
            {
                Debug.Log("Player spotted!");
            }
            */
			

            canSeePlayer = true;
        }
        else
        {
            canSeePlayer = false;

        }

        //check if beast position is in phone camera view port
        Vector3 viewPos = phone.WorldToViewportPoint(transform.position);
        if(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            visibleToPlayer = true;
        }
        else
        {
            visibleToPlayer = false;
        }

        playerDistance = Vector3.Distance(player.transform.position, transform.position);

        UpdateAIState();
        
        //?
        if(stateChanged)
        {
            Debug.Log("BeastAIState Changed to: " + CurrentState.ToString());
        }

        currentBehavior();
	}

    private void UpdateAIState()
    {
        stateChanged = false; //assume AI state unchanged until seen otherwise
        //state transition conditions based on current state
        switch(CurrentState)
        {
            case BeastAIState.Tutorial:
                // tutorial --> wander
                if(!player.InTutorial)
                {
                    //teleport to random location and then proceed to normal gameplay
                    TeleportToRoom(GetRandomRoomNode());
                    CurrentState = BeastAIState.Wander;
                    stateChanged = true;
                    currentBehavior = wander;
                }
                break;

            case BeastAIState.Wander:
                // wander --> approach
                if(canSeePlayer && !player.isHiding)
                {
                    CurrentState = BeastAIState.Approach;
                    stateChanged = true;
                    currentBehavior = approach;
                }
                break;

            case BeastAIState.Approach:
                // approach --> wander
                if(player.isHiding || !canSeePlayer)
                {
                    CurrentState = BeastAIState.Wander;
                    stateChanged = true;
                    currentBehavior = wander;
                }

                // approach --> pursue
                else if(visibleToPlayer || (playerDistance <= minApproachDistance))
                {
                    CurrentState = BeastAIState.Pursue;
                    stateChanged = true;
                    currentBehavior = pursue;
                }
                break;

            case BeastAIState.Pursue:
                // pursue --> search
                if(!canSeePlayer)
                {
                    lastKnownPlayerPos = player.gameObject.transform.position;
                    CurrentState = BeastAIState.Search;
                    stateChanged = true;
                    currentBehavior = search;
                }
                break;

            case BeastAIState.Search:
                // search --> pursue
                if(canSeePlayer && !player.isHiding)
                {
                    CurrentState = BeastAIState.Pursue;
                    stateChanged = true;
                    currentBehavior = pursue;
                }

                // search --> wander
                else if(numRoomSearched >= maxSearchRooms)
                {
                    CurrentState = BeastAIState.Wander;
                    stateChanged = true;
                    currentBehavior = wander;
                }
                break;

            default:
                Debug.LogError(string.Format(   "ERROR in {0}({1}): BeastController does not know how to update from its current AI state.", 
                                                gameObject.name, gameObject.GetInstanceID()));
                break;
        }

    }

    private GameObject GetRandomRoomNode()
    {
        GameObject room = rooms[Random.Range(0, rooms.Length-1)];
        return room;        
    }

    private void TeleportToRoom(GameObject destination)
    {
        //Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f); //most nodes set into floor, need to offset above
        gameObject.transform.position = destination.transform.position;// + offset;

        //?
        Debug.Log("Enemy teleported to " + destination.name);
    }

    private void StateTutorialBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;

            //?
            //Debug.Log("Running Tutorial Behavior...");
        }


    }

    private void StateWanderBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;
            agent.speed = wanderSpeed;
            CurrentTarget = GetRandomRoomNode();
            agent.SetDestination(CurrentTarget.transform.position);

            //?
            //Debug.Log("Running Wander Behavior...");
            //Debug.Log("Wandering to " + CurrentTarget.name);
        }

        //upon reaching target
        else if(!agent.pathPending && agent.remainingDistance < targetWanderDistance)
        {
            //!
            CurrentTarget = GetRandomRoomNode();
            agent.SetDestination(CurrentTarget.transform.position);

            //?
            //Debug.Log("Wandering to " + CurrentTarget.name);
        }
    }

    private void StateApproachBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;
            agent.speed = approachSpeed;
            CurrentTarget = player.gameObject;
            agent.SetDestination(player.transform.position);

            //?
            //Debug.Log("Running Approach Behavior...");
        }

        else if(!agent.pathPending)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void StatePursueBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;
            agent.speed = pursueSpeed;
            CurrentTarget = player.gameObject;
            agent.SetDestination(player.transform.position);

            //?
            //Debug.Log("Running Pursue Behavior...");


			//
			if(Vector3.Distance(player.transform.position, gameObject.transform.position) < LOSE_DISTANCE)
			Debug.Log("Player lost!");
			//
        }
        else if(!agent.pathPending)
        {
            agent.SetDestination(player.transform.position);
        }

        if(playerDistance < attackRange)
        {
            player.Damage();
        }
    }

    private void StateSearchBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;
            agent.speed = wanderSpeed;
            CurrentTarget = player.gameObject;
            numRoomSearched = 0;
            agent.SetDestination(lastKnownPlayerPos);

            //?
            //Debug.Log("Running Search Behavior...");
        }

        //if last known player location or search location is reached is reached
        else if(!agent.pathPending && agent.remainingDistance <= targetWanderDistance)
        {
            //if last known player location is reached
            if(CurrentTarget == player.gameObject)
            {
                //copy the list of room nodes
                GameObject[] nearestRooms = new GameObject[rooms.Length];
                for(int i = 0; i < rooms.Length; i++)
                {
                    nearestRooms[i] = rooms[i];
                }

                //bubble sort them by distance from the beast
                for(int i = 0; i < nearestRooms.Length - 1; i++)
                {
                    for(int j = 0; j < nearestRooms.Length - i - 1; j++)
                    {
                        float a = Vector3.Distance(nearestRooms[j].transform.position, transform.position);
                        float b = Vector3.Distance(nearestRooms[j + 1].transform.position, transform.position);
                        if(a > b)
                        {
                            GameObject temp = nearestRooms[j + 1];
                            nearestRooms[j + 1] = nearestRooms[j];
                            nearestRooms[j] = temp;
                        }
                    }
                }

                //randomly choose one of the <maxConsiderSearch> closest rooms on the same floor
                CurrentTarget = nearestRooms[Random.Range(0, maxConsiderSearch - 1)];
                agent.SetDestination(CurrentTarget.transform.position);

                
                //?
                string roomNames = "Rooms: ";
                foreach(GameObject node in rooms)
                {
                    roomNames = roomNames + node.name + ", ";
                }
                Debug.Log(roomNames);
                string nearNames = "Nears: ";
                foreach(GameObject node in nearestRooms)
                {
                    nearNames = nearNames + node.name + ", ";
                }
                Debug.Log(nearNames);
                Debug.Log(string.Format("Closest Search Options: {0}, {1}, {2}", nearestRooms[0].name, nearestRooms[1].name, nearestRooms[2].name));
                Debug.Log("Searching: " + CurrentTarget.name);
                
                //?
                //Debug.Log("Reached last known. Searching: " + CurrentTarget.name);
            }

            //if the room to be searched is reached, increment
            else
            {
                numRoomSearched++;
                Debug.Log("Finished searching " + CurrentTarget.name);
            }
        }
    }
}
