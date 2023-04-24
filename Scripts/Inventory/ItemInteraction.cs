using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    Transform cam;
    [SerializeField] LayerMask itemLayer;
    InventorySystem inventorySystem;

    [SerializeField] TextMeshProUGUI txt_HoveredItem;

    InputManager input;
    PlayerMovement pm;
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        cam = Camera.main.transform;
        inventorySystem = GetComponent<InventorySystem>();
        input = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position,0.25f,-pm.body.transform.up,out hit,0.5f, itemLayer))
        {
            if (!hit.collider.GetComponent<ItemObject>())
                return;

            txt_HoveredItem.text = $"Press 'E' to pick up {hit.collider.GetComponent<ItemObject>().amount}x {hit.collider.GetComponent<ItemObject>().itemStats.name}";

            if (input.doPickUpItem)
            {
                inventorySystem.PickUpItem(hit.collider.GetComponent<ItemObject>());
            }



        }
        else
        {
            txt_HoveredItem.text = string.Empty;
        }
    }
}
