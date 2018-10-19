using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;

public class BatteryPickupController : MonoBehaviour, StdT12.Interfaces.IInteractable
{
    [SerializeField]
    private string openMessage = "Press 'E' to Pickup Battery";
    [SerializeField]
    private string closeMessage = "";

   // public GameObject batteryPickedUp;
  //  private int batteries;

    //interaction fields
    private bool canInteract = true;
  //  private float interactTime = 0.0f;

    private string interactMessage = "";
    public string InteractMessage { get { return interactMessage; } }
    private Text actionPrompt;

    //animation fields
    private bool isPickedUp = false;
    //  private int animParamOpenId;
    //private Animator anim;

    private int batteries;
    public GameObject batteryPickedUp;


    public void Start()
    {
        actionPrompt = gameObject.GetComponent<Text>();
        UpdateInteractMessage();

    }

    private void Update()
    {
        //reality check to keep animator and script in sync
        //   isOpen = anim.GetBool(animParamOpenId);

        //check if animation is done, reset interaction variables if it is
        //    if (!canInteract)
        //   {
        //      canInteract = true;
        // interactTime = 0.0f;
        //  }
        
    }

    private void UpdateInteractMessage()
    {
        if (isPickedUp)
        {
            
            interactMessage = closeMessage;
        }

        else
        {
            interactMessage = openMessage;
        }

       
    }

    public void Interact()
    {
        //only allow interaction when not animating
        if (canInteract)
        {
            //toggle door state
            isPickedUp = !isPickedUp;
            //  anim.SetBool(animParamOpenId, isOpen);

            //get game time when animation will be complete
            //interactTime = Time.time + anim.GetAnimatorTransitionInfo(0).duration;

            if (isPickedUp)
            {
                batteries += 1;
                Destroy(batteryPickedUp);
            }

            print("batteries: " + batteries);
            canInteract = false;

            UpdateInteractMessage();
        }
    }
}
