using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialouge : MonoBehaviour
{
    bool hasChopped = false; 
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && InputManager.instance.doGoInCabin)
        {
            InputManager.instance.doGoInCabin = false;
            if (!transform.parent.GetComponent<Companion>().hasIntroduced)
            {
                transform.parent.GetComponent<Companion>().hasIntroduced = true;
                StartCoroutine(transform.parent.GetComponent<Dialouge>().GoThroughDialouge(transform.parent.GetComponent<Dialouge>().introduction));
            }
            else if (GameManager.instance.hasBuiltBridge && !hasChopped)
            {
                hasChopped = true;
                StartCoroutine(transform.parent.GetComponent<Dialouge>().GoThroughDialouge(transform.parent.GetComponent<Dialouge>().helpWithWood));
            }
        }
    }
}
