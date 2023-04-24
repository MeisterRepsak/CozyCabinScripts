using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarManager : MonoBehaviour
{
    [Range(0,7)] public int equippedSlotID;
    public HotBarSlot currentSlot;
    public Color32 equippedColor;
    public Color32 defaultColor;
    InputManager input;
    ItemEquipManager itemEquipManager;
    private void Start()
    {
        itemEquipManager = FindObjectOfType<ItemEquipManager>();
        input = FindObjectOfType<InputManager>();
    }
    private void Update()
    {
        int yuh = equippedSlotID;
        equippedSlotID = input.scrollHotbar;

        if (equippedSlotID != yuh)
            SetSlot();
        
    }
    public void SetSlot()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == equippedSlotID)
            {
                currentSlot = transform.GetChild(i).GetComponent<HotBarSlot>();
                currentSlot.transform.GetChild(0).GetComponent<Image>().color = equippedColor;
                
            }
            else
            {
                transform.GetChild(i).GetChild(0).GetComponent<Image>().color = defaultColor;
            }
        }
    }

}
