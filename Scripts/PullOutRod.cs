using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullOutRod : MonoBehaviour
{
    [SerializeField] GameObject rod;
   public void ActivateRod()
   {
        rod.SetActive(true);
   }
}
