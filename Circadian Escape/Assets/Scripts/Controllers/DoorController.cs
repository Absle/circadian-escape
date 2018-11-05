using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;

public class DoorController : MonoBehaviour, StdT12.Interfaces.IInteractable
{
    public bool isLocked = false;

    [SerializeField]
    private string lockedMessage = "Locked";
    [SerializeField]
    private string unlockMessage = "Press 'E' to Unlock";
    [SerializeField]
    private string openMessage = "Press 'E' to Open";
    [SerializeField]
    private string closeMessage = "Press 'E' to Close";

    //private static List<StdT12.Interfaces.IPickUpable> keyRing;

    //interaction fields
    private bool canInteract = true;
    private float interactTime = 0.0f;
    private string interactMessage = "";
    public string InteractMessage { get { return (canInteract ? interactMessage : ""); } }

    //animation fields
    private bool isOpen = false;
    private int animParamOpenId;
    private const float DEFAULT_DOOR_ANIMATION_LENGTH = 3.0f;
    private Animator anim;
    private AudioSource audSrc;

    private void Start()
    {
        //keyRing = (GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController).KeyRing;

        anim = gameObject.GetComponentInParent<Animator>();
        animParamOpenId = Animator.StringToHash("Open");
        audSrc = gameObject.GetComponent<AudioSource>();

        UpdateInteractMessage();
    }

	private void Update()
    {
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
            //if(keyRing)
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
            //TODO: add a locked door "click" or sound effect
        }

        //only allow interaction when not animating
        else if(canInteract)
        {
            canInteract = false;

            //toggle door state and play animation
            isOpen = !isOpen;
            anim.SetBool(animParamOpenId, isOpen);
            PlayAudio();

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
            audSrc.Play();
        }

        //play door closing sound
        else
        {
            audSrc.Play();
        }
    }
}
