using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StdT12;
using StdT12.Enums;
using StdT12.Interfaces;

public class KeyItem : MonoBehaviour, IPickUpable
{
    [SerializeField]
    private GameObject door;

    [SerializeField]
    private string pickUpMessage = "Press 'E' to Pick Up Key";
    public string PickUpMessage { get { return pickUpMessage; } }

    private PickUpType type = PickUpType.KeyItem;
    public PickUpType Type { get { return type; } }

    private int doorID;
    public int DoorID { get { return doorID; } }

    // Use this for initialization
    void Start ()
    {
        //?
        /*
        GameObject[] doorList = GameObject.FindGameObjectsWithTag("Door");
        int randDex = Random.Range(0, doorList.Length);
        Debug.Log(randDex + " " + doorList.Length);
        doorID = doorList[randDex].GetInstanceID();
        DoorController doorController = doorList[randDex].GetComponentInChildren(typeof(DoorController)) as DoorController;
        doorController.isLocked = true;
        */

        doorID = door.GetInstanceID();
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        GameObject node = nodes[Random.Range(0, nodes.Length-1)];
        Debug.Log("KeyItem moving to " + node.name);
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f); //most nodes set into floor, need to offset above
        gameObject.transform.position = node.transform.position + offset;
	}
}
