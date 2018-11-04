using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StdT12.Enums;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject slotHolder;

    private int slots;
    private Transform[] slot;
    private bool inventoryEnabled;

    private GameObject itemPickedUp;

    public void Start()
    {
        // slot being detected
        slots = slotHolder.transform.childCount;
        slot = new Transform[slots];
        DetectInventorySlots();
    }

    public void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;

        }
        if(inventoryEnabled)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
        */
    }

   // public void PickedUp ()
   // {
    //    AddItem(itemPickedUp);
   // }

    //  public void OnTriggerEnter(Collider other)
    //  {
    //  if(other.gameObject.GetComponent<Item>())
    /*  if(other.tag == "Item")
      {
          print("Colliding");
          itemPickedUp = other.gameObject;
          AddItem(itemPickedUp);
      }

    */
    // }

    //itemPickedUp = other.gameObject;
           


    public void AddItem(PickUpType item)
    {
       // AddItem(itemPickedUp);

        for (int i = 0; i < slots; i++)
        {
            if(slot[i].GetComponent<Slot>().empty)
            {
                slot[i].GetComponent<Slot>().item = itemPickedUp;
                slot[i].GetComponent<Slot>().itemIcon = itemPickedUp.GetComponent<Item>().icon;
            }
        }
    }

    public void DetectInventorySlots()
    {
        for(int i = 0; i < slots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i);
           // print(slot[i].name);
        }
    }




}
