using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StdT12;
using StdT12.Enums;

public class BeastController : MonoBehaviour
{
    private T12_GameManager manager;
    private PlayerController player;
    private Camera phone;
    private NavMeshAgent agent;
    private GameObject[] rooms;

    public BeastAIState CurrentState { get; private set; }
    public GameObject CurrentTarget { get; private set; }

    private bool canSeePlayer;
    private bool visibleToPlayer;
    private bool stateChanged;
    private int numRoomSearched;
    private float playerDistance;
    private Vector3 lastKnownPlayerPos;
    private int maxSearchRooms = 1;
    [SerializeField]
    private int maxConsiderSearch = 3;
    [SerializeField]
    private float targetWanderDistance = 2.0f;
    [SerializeField]
    private float minApproachDistance = 5.0f;
    [SerializeField]
    private float maxApproachTime = 60.0f;
    [SerializeField]
    private float wanderSpeed = 3.0f;
    [SerializeField]
    private float pursueSpeed = 5.0f;
    [SerializeField]
    private float maxSightDistance = 50.0f;
    
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

        //?
        player = manager.Player;
        //player = StdT12Common.Player;
        phone = manager.PhoneCamera;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        rooms = manager.RoomGraph;
        //rooms = StdT12Common.RoomGraph;

        CurrentState = BeastAIState.Tutorial;

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

        /*
        //?
        foreach(GameObject node in rooms)
        {
            Debug.Log(node.name);
        }
        */
    }
	
	// Update is called once per frame
	private void Update()
    {
        //first, determine state flags
        //check if player is in line of sight
        RaycastHit hit;
        if( Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, maxSightDistance) &&
            (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Phone")))
        {
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
                if(canSeePlayer)
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
                if(!canSeePlayer || player.isHiding)
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
                    numRoomSearched = 0;
                    CurrentState = BeastAIState.Pursue;
                    stateChanged = true;
                    currentBehavior = pursue;
                }

                // search --> wander
                else if(numRoomSearched >= maxSearchRooms)
                {
                    numRoomSearched = 0;
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
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f); //most nodes set into floor, need to offset above
        gameObject.transform.position = destination.transform.position + offset;

        //?
        Debug.Log("Enemy teleported to " + destination.name);
    }

    private void StateTutorialBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;

            //?
            Debug.Log("Running Tutorial Behavior...");
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
            Debug.Log("Running Wander Behavior...");
            Debug.Log("Wandering to " + CurrentTarget.name);
        }

        else if(!agent.pathPending && agent.remainingDistance < targetWanderDistance)
        {
            CurrentTarget = GetRandomRoomNode();
            agent.SetDestination(CurrentTarget.transform.position);

            //?
            Debug.Log("Wandering to " + CurrentTarget.name);
        }
        

    }

    private void StateApproachBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;
            agent.speed = wanderSpeed;
            CurrentTarget = player.gameObject;
            agent.SetDestination(player.transform.position);

            //?
            Debug.Log("Running Approach Behavior...");
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
            Debug.Log("Running Pursue Behavior...");
        }

        
    }

    private void StateSearchBehavior()
    {
        if(stateChanged)
        {
            stateChanged = false;
            agent.speed = wanderSpeed;
            CurrentTarget = null;
            agent.SetDestination(lastKnownPlayerPos);

            //?
            Debug.Log("Running Search Behavior...");
        }

        //if last known player location or search location is reached is reached
        else if(agent.remainingDistance <= minApproachDistance)
        {
            if(CurrentTarget == null)
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

                //randomly choose 1 of the <maxConsiderSearch> closest rooms on the same floor
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
                Debug.Log("Going to " + CurrentTarget.name);
            }
            else
            {
                numRoomSearched++;
            }
        }
    }
}
