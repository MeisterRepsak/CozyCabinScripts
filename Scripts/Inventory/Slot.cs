using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Items ItemInSlot;
    public int AmountInSlot;
    public GameObject itemFrame;
    public RawImage icon;
    public TextMeshProUGUI txt_amount;

    public void SetStats()
    {
        for (int i = 0; i < itemFrame.transform.childCount; i++)
        {
            itemFrame.transform.GetChild(i).gameObject.SetActive(true);
        }

        

        if (ItemInSlot == null)
        {
            icon.enabled = false;
            txt_amount.enabled = false;
            return;
        }
        else
        {
            txt_amount.enabled = true;
            icon.enabled = true;
        }


        icon.texture = ItemInSlot.icon;
        txt_amount.text = $"{AmountInSlot}x";
    }
    

}
