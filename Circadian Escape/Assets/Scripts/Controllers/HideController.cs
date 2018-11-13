using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using StdT12;

public class HideController : MonoBehaviour, StdT12.Interfaces.IInteractable
{
	[SerializeField]
	private string openMessage = "Press 'E' to Hide";
	[SerializeField]
	private string closeMessage = "Press 'E' to Exit";

	//interaction fields
	//private bool canInteract = true;
	//private float interactTime = 0.0f;

	private string interactMessage = "";
	public string InteractMessage { get { return interactMessage; } }

	//animation fields
	private bool isOpen = false;

    // Switching cameras
    public Camera phoneCam;
	private Camera main;
	private Camera cam;
	private GameObject player;
	private PlayerController playerController;

	private void Start()
	{
		UpdateInteractMessage();
		main = Camera.main;
		//cam = gameObject.GetComponentInParent<Camera> ();
		cam = gameObject.GetComponentInChildren<Camera> ();
		cam.enabled = false;
		//cam = GameObject.FindObjectOfType<Camera> ().GetComponent<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent (typeof(PlayerController)) as PlayerController;
		//playerController = gameObject.GetComponent (typeof(PlayerController)) as PlayerController;
	}

	private void Update()
	{
		//check if animation is done, reset interaction variables if it is
//		if(!canInteract && Time.time >= interactTime)
//		{
//			canInteract = true;
//			interactTime = 0.0f;
//		}
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
//		if(canInteract)
//		{
			//toggle door state
			isOpen = !isOpen;

			if (isOpen) {
				main.enabled = false;
				cam.enabled = true;
                phoneCam.enabled = false;
				player.GetComponent<FirstPersonController>().enabled = false;
				playerController.isHiding = true;
			} else {
				playerController.isHiding = false;
				main.enabled = true;
				cam.enabled = false;
                phoneCam.enabled = true;
				player.GetComponent<FirstPersonController> ().enabled = true;
			}

			//get game time when animation will be complete
//			interactTime = Time.time + anim.GetAnimatorTransitionInfo(0).duration;
//			canInteract = false;

			UpdateInteractMessage();
//		}
	}    
}
