using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjOof : MonoBehaviour
{

    GameObject mainCamera;
    bool pressed;
    bool carrying;
    GameObject carriedObject;

    public float distance;
    public float smooth;

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    bool isRotation;



    // Use this for initialization
    void Start ()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            pressed = !pressed;

        }

        if (carrying)
        {
            carry(carriedObject);
            checkDrop();

        }
        else
        {
            pickUp();
        }

		
	}

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
    }

    void pickUp()
    {

     //   if (Input.GetKeyDown(KeyCode.R))
      //  {
       //     isRotation = !isRotation;

//        }

        if (pressed)
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

             float v = Input.GetAxis("Mouse X") * speed;
            float h = Input.GetAxis("Mouse Y") * rotationSpeed;




            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));

            RaycastHit hit;

             if(Physics.Raycast(ray, out hit))
              {
            Physics.Raycast(ray, out hit);
            PickupAttempt p = hit.collider.GetComponent<PickupAttempt>();
            if (p != null)
            {
                carrying = true;
                carriedObject = p.gameObject;
                p.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    if (Input.GetKey(KeyCode.R))
                    {
                        // p.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        // p.gameObject.GetComponent<Rigidbody>().useGravity = true;
                        p.gameObject.transform.RotateAroundLocal(mainCamera.transform.up, -Mathf.Deg2Rad * v);
                        p.gameObject.transform.RotateAroundLocal(mainCamera.transform.right, Mathf.Deg2Rad * h);
                      //  p.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 80);
                    }

                    // if(isRotation)


                    // p.gameObject.rigidbody.isKinematic = true;
                    //   }
                }

            }

        }
    }

    void checkDrop()
    {
       
        if (!pressed)
        {
            dropObject();
        }

    }

    void dropObject()
    {
            carrying = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        // carriedObject.gameObject.rigidbody.isKinematic = false;
        carriedObject = null;
    }


}
