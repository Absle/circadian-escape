using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StdT12;
using StdT12.Enums;

public class BatteryPickUpController : MonoBehaviour, StdT12.Interfaces.IPickUpable
{
    [SerializeField]
    private string pickUpMessage = "Press 'E' to Pick Up Battery";
    public string PickUpMessage { get { return pickUpMessage; } }

    private PickUpType type = PickUpType.Battery;
    public PickUpType Type { get { return type; } }
    
    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
