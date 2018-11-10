using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBattery : MonoBehaviour
{
    public SimpleHealthBar healthBar;

    private bool flashLightEnabled;
    public GameObject flashLight;
    public GameObject lightObject;
    private GameObject batteryPickedUp;
    private GameObject phonePickedUp;
    private bool isPhoneEquipped;
    private float usedEnergy;

    public float maxEnergy;

    private int batteries;
    private float currentEnergy;

    private PlayerController anotherScript;


    public void Start()
    {
        batteries = 1;
        currentEnergy = maxEnergy;
        maxEnergy = 50 * batteries;
      //  anotherScript = GetComponent<PlayerController>();
    }

    public void Update()
    {
        // 1 battery = 50 energy 
        maxEnergy = 50 * batteries;
        currentEnergy = maxEnergy - usedEnergy;
        //currentEnergy = 1;
        //equip
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPhoneEquipped = !isPhoneEquipped;

        }

        if (isPhoneEquipped)
        {
            flashLight.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                flashLightEnabled = !flashLightEnabled;
            }


            if (flashLightEnabled)
            {
                lightObject.SetActive(true);

                if (currentEnergy <= 0)
                {
                    lightObject.SetActive(false);
                    batteries = 0;
                }

                if (currentEnergy > 0)
                {
                    //currentEnergy = maxEnergy;
                    lightObject.SetActive(true);
                    //   currentEnergy -= 10.0f * Time.deltaTime;
                    usedEnergy += .05f * Time.deltaTime;
                    /// realCurrent = currentEnergy;
                    //  print("Current Energy " + currentEnergy);


                }

                if (usedEnergy >= 50)
                {
                    batteries -= 1;
                    usedEnergy = 1;
                }
            }
            else
                lightObject.SetActive(false);


        }
        else
            flashLight.SetActive(false);

    //    if (Input.GetKeyDown(KeyCode.F))
     //   {
      //      flashLightEnabled = !flashLightEnabled;
       // }


       

        healthBar.UpdateBar(currentEnergy, maxEnergy);

        /*print("Energy MAXIMUM  " + maxEnergy);
        print("BATTERIES: " + batteries);
        print("Energy Consumed  " + usedEnergy);
        print("Current Energy " + currentEnergy);*/

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Battery")
        {
            batteryPickedUp = other.gameObject;
            batteries += 1;
            Destroy(batteryPickedUp);
        }

    }

}
