using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private string openMessage = "Press 'E' to Open";
    [SerializeField]
    private string closeMessage = "Press 'E' to Close";

    public bool isOpen = false;
    public bool canInteract = true;

    private int animParamOpenId;
    public float interactTime = 0.0f;

    private Text actionPrompt;
    private Animator anim;

    public Text ActionPrompt { get; private set; }

    private void Start()
    {
        anim = gameObject.GetComponentInParent<Animator>();
        animParamOpenId = Animator.StringToHash("Open");
        actionPrompt = gameObject.GetComponent<Text>();
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
        if(isOpen)
        {
            actionPrompt.text = closeMessage;
        }

        else
        {
            actionPrompt.text = openMessage;
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

            //get game time when animation will be complete
            interactTime = Time.time + anim.GetAnimatorTransitionInfo(0).duration;
            canInteract = false;
            //?
            Debug.Log("Time: " + Time.time);
            Debug.Log("duration: " + anim.GetAnimatorTransitionInfo(0).duration);
            Debug.Log("interactTime: " + interactTime);

            UpdateInteractMessage();
        }
    }

    /*
    public void DisplayInteractMessage()
    {
        //only display message when not animating
        if(canInteract)
        {
            
        }

        //otherwise, ensure message is disabled
        else
        {

        }
    }
    */
    
}
