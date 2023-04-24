using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] public Slot[] slots = new Slot[40];
    [SerializeField] GameObject InventoryUI;

    PlayerMovement pm;

    InputManager input;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        pm = GetComponent<PlayerMovement>();
        for (int i = 0; i < InventoryUI.transform.childCount; i++)
        {
            for (int k = 0; k < InventoryUI.transform.childCount; k++)
            {
                slots[k] = InventoryUI.transform.GetChild(k).GetComponent<Slot>();
                slots[k].SetStats();
            }
            if (slots[i].ItemInSlot == null)
            {
                for (int k = 0; k < slots[i].itemFrame.transform.childCount; k++)
                {
                    slots[i].itemFrame.transform.GetChild(k).gameObject.SetActive(false);
                }
            }
        }
        InventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (!InventoryUI.activeInHierarchy && input.doOpenInventory)
        {
            InventoryUI.SetActive(true);
        }
        else if (InventoryUI.activeInHierarchy && input.doOpenInventory)
        {
            InventoryUI.SetActive(false);
        }
    }


    public void PickUpItem(ItemObject obj)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.id == obj.itemStats.id && slots[i].AmountInSlot != slots[i].ItemInSlot.maxStack)
            {
                if (!WillHitMaxStack(i, obj.amount))
                {
                    slots[i].AmountInSlot += obj.amount;
                    Destroy(obj.gameObject);
                    slots[i].SetStats();
                    return;
                }
                else
                {
                    int result = NeededToFill(i);
                    obj.amount = RemainingAmount(i, obj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].SetStats();
                    PickUpItem(obj);
                    return;
                }
            }
            else if (slots[i].ItemInSlot == null)
            {
                slots[i].ItemInSlot = obj.itemStats;
                slots[i].AmountInSlot += obj.amount;
                Destroy(obj.gameObject);
                slots[i].SetStats();
                return;

            }
        }
    }
    public void PutInInventory(Items item, int amount)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.id == item.id && slots[i].AmountInSlot != slots[i].ItemInSlot.maxStack)
            {
                if (!WillHitMaxStack(i, amount))
                {
                    slots[i].AmountInSlot += amount;
                    slots[i].SetStats();
                    return;
                }
                else
                {
                    int result = NeededToFill(i);
                    amount = RemainingAmount(i, amount);
                    slots[i].AmountInSlot += result;
                    slots[i].SetStats();
                    PutInInventory(item,amount);
                    return;
                }
            }
            else if (slots[i].ItemInSlot == null)
            {
                slots[i].ItemInSlot = item;
                slots[i].AmountInSlot += amount;
                slots[i].SetStats();
                return;

            }
        }
    }

    public void CraftItem(Items requirement, int amount)
    {
        int remaining;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot != null)
            {
                if (slots[i].ItemInSlot.id == requirement.id)
                {
                    RemoveItems(slots[i], amount, out remaining);

                    amount = remaining;
                    if (amount == 0)
                    {
                        return;
                    }
                }
            }

           
        }
    }

    public bool HasEnough(Items item, int amout)
    {
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot != null)
            {
                if (slots[i].ItemInSlot.id == item.id)
                {
                    count += slots[i].AmountInSlot;
                }

            }
                

            
        }

        return count >= amout;
    }

    bool WillHitMaxStack(int index, int amount)
    {
        if (slots[index].ItemInSlot.maxStack+1 <= slots[index].AmountInSlot + amount)
            return true;
        else
            return false;
    }

    int NeededToFill(int index)
    {
        return slots[index].ItemInSlot.maxStack - slots[index].AmountInSlot;
    }
    int RemainingAmount(int index, int amount)
    {
        return (slots[index].AmountInSlot + amount) - slots[index].ItemInSlot.maxStack;
    }
    void RemoveItems(Slot slot, int amount, out int remaining)
    {
        if(slot.AmountInSlot - amount > 0)
        {
            remaining = 0;
            slot.AmountInSlot -= amount;
        }
        else
        {
             remaining = amount - slot.AmountInSlot;
            slot.AmountInSlot = 0;
            slot.ItemInSlot = null;
            slot.SetStats();
        }
    }
}
