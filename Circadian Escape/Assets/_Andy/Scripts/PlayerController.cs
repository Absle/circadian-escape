using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        RaycastHit hit = CheckForInteractables();
        if(canInteract && Input.GetKeyDown(KeyCode.E))
        {
            hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.RequireReceiver);
        }
	}

    private void FixedUpdate()
    {

    }

    private RaycastHit CheckForInteractables()
    {
        RaycastHit hit;
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxInteractDist) && hit.collider.CompareTag("Interactable"))
        {
            canInteract = true;
            actionPrompt.text = hit.collider.gameObject.GetComponent<Text>().text;
            actionPrompt.enabled = true;
            //hit.transform.gameObject.SendMessage("DisplayInteractMessage", SendMessageOptions.RequireReceiver);
        }

        else
        {
            canInteract = false;
            actionPrompt.enabled = false;
        }

        return hit;
    }
}
