using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;
using StdT12.Enums;
using StdT12.Interfaces;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxInteractDist = 2.0f;

    private T12_GameManager manager;
    private PlayerInventory inv;
    public List<int> KeyRing { get { return inv.KeyRing; } }
    public bool InTutorial { get; private set; }

    private Text actionPrompt;
    private Transform camTransform;

	// Used for AI
	public bool isHiding = false;

    private void Start()
    {
        manager = FindObjectOfType<T12_GameManager>();

        //?
        //InTutorial = true;
        InTutorial = false;

        actionPrompt = GameObject.Find("ActionPrompt").GetComponent<Text>();
        actionPrompt.text = "";
        actionPrompt.enabled = true;
		isHiding = false; 

        camTransform = Camera.main.transform;
        inv = new PlayerInventory();
	}
	
	private void Update()
    {
        HandleRaycasting();
	}

    private void HandleRaycasting()
    {
        string message = "";
        RaycastHit hit;
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxInteractDist))
        {
            //check for interactable objects
            if(hit.collider.CompareTag("Interactable"))
            {
                IInteractable interactable = hit.collider.gameObject.GetComponent(typeof(IInteractable)) as IInteractable; //get interactable object

                //display interaction message
                message = interactable.InteractMessage;

                //let player interact with 'E'
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }

            //check for pick up items
            else if(hit.collider.CompareTag("PickUp"))
            {
                IPickUpable pickup = hit.collider.gameObject.GetComponent(typeof(IPickUpable)) as IPickUpable; //get pickup item
                message = pickup.PickUpMessage;

                if(Input.GetKeyDown(KeyCode.E))
                {
                    GameObject pickupObj = hit.collider.gameObject;
                    inv.AddItem(pickup, pickupObj);
                }
            }
        }

        //display message
        actionPrompt.text = message;
    }

    /*
     * Called by Beast when damaging the Player
     */
    public void Damage()
    {
        //currently loses immediately
        manager.PlayerLose();
    }

    //nested private class to handle inventory and items
    private class PlayerInventory
    {
        private int numBatteries;
        //public int NumBatteries { get { return numBatteries; } }

        private List<int> keyRing;
        public List<int> KeyRing { get { return keyRing; } }

        private Text batteryCounter;

        public PlayerInventory()
        {
            numBatteries = 0;
            keyRing = new List<int>();

            batteryCounter = GameObject.Find("BatteryCounter").GetComponent<Text>();
            UpdateUI();
        }

        private void UpdateUI()
        {
            batteryCounter.text = string.Format("Batteries: {0}", numBatteries);

            //TODO: integrate key items into UI when it's finished'

        }

        public void AddItem(IPickUpable item, GameObject itemObj)
        {
            if(item.Type == PickUpType.Battery)
            {
                ++numBatteries;
                Destroy(itemObj);
            }

            else if(item.Type == PickUpType.KeyItem)
            {
                KeyItem key = item as KeyItem;
                keyRing.Add(key.DoorID);
                Destroy(itemObj);
            }

            else
            {
                Debug.LogError("PlayerInventory does not have handling for this PickUpType!");
            }

            UpdateUI();
        }

        public void UseBattery()
        {
            if(numBatteries > 0)
            {
                --numBatteries;
                //TODO: integrate into battery bar
            }
        }
    }
}
