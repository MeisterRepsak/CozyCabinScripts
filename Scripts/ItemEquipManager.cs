using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipManager : MonoBehaviour
{
    [SerializeField] HotBarManager hotbar;
    PlayerManager pManager;

    private void Start()
    {
        pManager = FindObjectOfType<PlayerManager>();
        
    }
    private void Update()
    {
        if (hotbar.currentSlot == null)
            return;

        if (hotbar.currentSlot.ItemInSlot != null)
            EquipItem(hotbar.currentSlot.ItemInSlot.id);
        else
            pManager.playerStates = playerStates.normal;

    }

    public void EquipItem(int id)
    {
        switch (id)
        {
            case 3:
                pManager.playerStates = playerStates.fishing;
                break;
            default:
                pManager.playerStates = playerStates.normal;
                break;
        }
    }
}
