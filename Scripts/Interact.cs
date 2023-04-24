using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Interact : MonoBehaviour
{
    PlayerMovement pm;
    InputManager input;
    public LayerMask interacyables;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        input = GetComponent<InputManager>();
    }
    void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.25f, pm.body.transform.forward, out hit, 0.5f,interacyables))
        {
            IInteractable intertactableObj = hit.collider.GetComponent<IInteractable>();
            
                

            

            if (input.doPickUpItem)
            {
                if (intertactableObj != null)
                    intertactableObj.Interact();
            }



        }
    }
}
