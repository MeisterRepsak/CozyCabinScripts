using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlot : MonoBehaviour
{
    public Items ItemInSlot;
    public int AmountInSlot;
    public GameObject itemFrame;
    [SerializeField] RawImage icon;
    [SerializeField] TextMeshProUGUI txt_amount;

    [SerializeField] Slot copySlot;

    // Update is called once per frame
    void Update()
    {
        ItemInSlot = copySlot.ItemInSlot;
        AmountInSlot = copySlot.AmountInSlot;
        itemFrame = copySlot.itemFrame;
        txt_amount.text = copySlot.txt_amount.text;

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
