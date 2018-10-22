using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;
using StdT12.Interfaces;
using StdT12.Enums;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxInteractDist = 2.0f;

    public bool canInteract = false;

    private Canvas canvas;
    private Text actionPrompt;
    private int numBatteries = 0;
    private Text batteryCounter;
    private Transform camTransform;

    //?
    bool WINNING = false;

    private void Start()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        actionPrompt = GameObject.Find("ActionPrompt").GetComponent<Text>();
        batteryCounter = GameObject.Find("BatteryCounter").GetComponent<Text>();

        camTransform = Camera.main.transform;
	}
	
	private void Update()
    {
        //check for raycast hits
        RaycastHit hit;
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxInteractDist))
        {
            //check for interactable objects
            if(hit.collider.CompareTag("Interactable"))
            {
                IInteractable interactable = hit.collider.gameObject.GetComponent(typeof(IInteractable)) as IInteractable; //get interactable object

                //display interaction message
                actionPrompt.text = interactable.InteractMessage;
                actionPrompt.enabled = true;

                //let player interact with 'E'
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }

            //check for pick up items
            else if (hit.collider.CompareTag("PickUp"))
            {
                IPickUpable pickup = hit.collider.gameObject.GetComponent(typeof(IPickUpable)) as IPickUpable; //get pickup item

                //display pick up message
                actionPrompt.text = pickup.PickUpMessage;
                actionPrompt.enabled = true;

                if(Input.GetKeyDown(KeyCode.E))
                {
                    PickUpType type = pickup.Type;
                    WINNING = (hit.collider.gameObject.name == "TheBiggestPickle"); //?
                    Destroy(hit.collider.gameObject);
                    if(type == PickUpType.Battery)
                    {
                        ++numBatteries;
                    }
                    /*//? TODO: add the item to the inventory, not just display it in the log
                    Debug.Log("YOU PICKED UP: " + type.ToString()); //?*/
                }
            }

            //?
           

            //if no interesting objects found, disable actionPrompt
            else
            {
                actionPrompt.enabled = false;
            }

            batteryCounter.text = "Batteries: " + numBatteries;
        }
        else if (WINNING)
        {
            actionPrompt.fontSize = 50;
            actionPrompt.text = "YOU HAVE FIND THE BIGGEST PICKLE!\nDANNY'S QUEST IS COMPLETE!\nYOU WIN!!!";
            actionPrompt.enabled = true;
        }

        else
        {
            actionPrompt.enabled = false;
        }
        
        /*IInteractable interactable = CheckForInteractables();
        if(canInteract && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }

        IPickUpable pickup = CheckForPickUps();*/
	}

    /*private IInteractable CheckForInteractables()
    {
        IInteractable interactable = null;
        RaycastHit hit;
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxInteractDist) && hit.collider.CompareTag("Interactable"))
        {
            canInteract = true;
            interactable = hit.collider.gameObject.GetComponent(typeof(IInteractable)) as IInteractable;
            actionPrompt.text = interactable.InteractMessage;
            actionPrompt.enabled = true;
        }

        else
        {
            canInteract = false;
            actionPrompt.enabled = false;
        }

        return interactable;
    }*/
}
