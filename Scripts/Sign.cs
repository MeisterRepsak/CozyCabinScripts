using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    bool hasGuidedTotrees = false;
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && InputManager.instance.doGoInCabin)
        {
            if (!GameManager.instance.hasBuiltBridge && !hasGuidedTotrees)
            {
                if(!hasGuidedTotrees)
                    hasGuidedTotrees=true;
                StartCoroutine(FindObjectOfType<Dialouge>().GoThroughDialouge(FindObjectOfType<Dialouge>().tellToChopWood));
            }


            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputManager.instance.doGoInCabin = false;
            GameObject.Find("Canvas").transform.Find("BridgeBuildUI").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameObject.Find("Canvas").transform.Find("BridgeBuildUI").gameObject.SetActive(false);
        }
    }
}
