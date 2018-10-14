using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxInteractDist;

    private bool canInteract;
    private Transform camTransform;

    private void Start()
    {
        maxInteractDist = 2.0f;

        canInteract = false;
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
        canInteract = false;
        RaycastHit hit;
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxInteractDist) && hit.collider.CompareTag("Interactable"))
        {
            canInteract = true;
            hit.transform.gameObject.SendMessage("DisplayInteractMessage", SendMessageOptions.RequireReceiver);
        }

        return hit;
    }
}
