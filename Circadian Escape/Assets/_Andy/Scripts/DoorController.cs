using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private string interactMessage;

    private bool isOpen;
    private bool canInteract;
    private int openId;
    private Animator anim;

	private void Start()
    {
        interactMessage = "Press 'E' to Open";

        isOpen = false;
        anim = gameObject.GetComponentInParent<Animator>();
	}
	
	private void Update()
    {
		
	}

    public void Interact()
    {
        anim.SetBool("Open", !isOpen);
        isOpen = !isOpen;
    }

    public void DisplayInteractMessage()
    {
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }*/
}
