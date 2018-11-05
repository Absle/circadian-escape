using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;

public class ExitDoorController : MonoBehaviour, StdT12.Interfaces.IInteractable
{
    public bool isLocked = true;

    [SerializeField]
    private string lockedMessage = "Locked";
    [SerializeField]
    private string unlockMessage = "Press 'E' to Unlock";
    [SerializeField]
    private string openMessage = "Press 'E' to Open";
    [SerializeField]
    private string closeMessage = "Press 'E' to Close";

    private int rootID;
    private PlayerController playerController;

    //interaction fields
    private bool canInteract = true;
    private float interactTime = 0.0f;
    private string interactMessage = "";
    public string InteractMessage { get { return (canInteract ? interactMessage : ""); } }

    //animation fields
    private bool isOpen = false;
    //private int animParamOpenId;
    private const float DEFAULT_DOOR_ANIMATION_LENGTH = 3.0f;
    //private Animator anim;
    //private AudioSource audSrc;

    private void Start()
    {
        rootID = gameObject.GetInstanceID();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerController)) as PlayerController;

        //anim = gameObject.GetComponentInParent<Animator>();
        //animParamOpenId = Animator.StringToHash("Open");
        //audSrc = gameObject.GetComponent<AudioSource>();

        //UpdateInteractMessage();
    }

    private void Update()
    {
        //reality check to keep animator and script in sync
        //isOpen = anim.GetBool(animParamOpenId);
        UpdateInteractMessage();

        //check if animation is done, reset interaction variables if it is
        if(!canInteract && Time.time >= interactTime)
        {
            canInteract = true;
            interactTime = 0.0f;
        }
    }

    private void UpdateInteractMessage()
    {
        if(isLocked)
        {
            if(playerController.KeyRing.Contains(rootID))
            {
                interactMessage = unlockMessage;
            }

            else
            {
                interactMessage = lockedMessage;
            }
        }

        else if(isOpen)
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
        if(isLocked)
        {
            //TODO: add locked door "click" sound effect

            //if the player has the key, unlock the door
            if(playerController.KeyRing!=null && playerController.KeyRing.Contains(rootID))
            {
                isLocked = false;
                //TODO: actual win condition
                Debug.Log("YOU ESCAPED!!!");
            }

            //if the player attempts to open the door w/o a key
            else
            {

            }
        }

        //only allow interaction when not animating
        else if(canInteract)
        {
            canInteract = false;

            //toggle door state and play animation
            isOpen = !isOpen;
            //anim.SetBool(animParamOpenId, isOpen);
            //PlayAudio();

            //get time until animation is complete
            float current = Time.time;
            float duration = DEFAULT_DOOR_ANIMATION_LENGTH; //dynamically determining animation length is stupid hard for some reason
            interactTime = current + duration;

            UpdateInteractMessage();
        }
    }

    //could be used to tie audio control into animation event later on
    public void PlayAudio()
    {
        //play door opening sound
        if(isOpen)
        {
            //audSrc.Play();
        }

        //play door closing sound
        else
        {
            //audSrc.Play();
        }
    }
}
