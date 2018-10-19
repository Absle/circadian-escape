using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StdT12;
using StdT12.Interfaces;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxInteractDist = 2.0f;

    public bool canInteract = false;

    private Text actionPrompt;
    private Transform camTransform;

    private void Start()
    {
        actionPrompt = GameObject.FindObjectOfType<Canvas>().GetComponentInChildren<Text>();

        camTransform = Camera.main.transform;
	}
	
	private void Update()
    {
        IInteractable interactable = CheckForInteractables();
        if(canInteract && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }
	}

    private void FixedUpdate()
    {

    }

    private IInteractable CheckForInteractables()
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
    }
}
