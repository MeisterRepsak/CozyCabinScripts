using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideCabin : MonoBehaviour
{
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && InputManager.instance.doGoInCabin)
        {
            InputManager.instance.doGoInCabin = false;
            SceneManager.LoadScene(3);
        }
    }
}
