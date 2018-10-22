using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;

public class DoorController : MonoBehaviour, StdT12.Interfaces.IInteractable
{
    [SerializeField]
    private string openMessage = "Press 'E' to Open";
    [SerializeField]
    private string closeMessage = "Press 'E' to Close";

    //interaction fields
    private bool canInteract = true;
    private float interactTime = 0.0f;

    private string interactMessage = "";
    public string InteractMessage { get { return interactMessage; } }
    
    //animation fields
    private bool isOpen = false;
    private int animParamOpenId;
    private Animator anim;



  //  public AudioClip MusicClip;

    private AudioSource MusicSource;



    private void Start()
    {
        anim = gameObject.GetComponentInParent<Animator>();
        animParamOpenId = Animator.StringToHash("Open");
        UpdateInteractMessage();

        MusicSource = gameObject.GetComponent<AudioSource>();
      //  MusicSource.clip = MusicClip;
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
        if(isOpen)
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
        if(canInteract)
        {
            //toggle door state
            isOpen = !isOpen;
            anim.SetBool(animParamOpenId, isOpen);



            MusicSource.Play();

            //get game time when animation will be complete
            interactTime = Time.time + anim.GetAnimatorTransitionInfo(0).duration;
            canInteract = false;

            UpdateInteractMessage();
        }
    }    
}
