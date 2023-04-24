using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridge : MonoBehaviour
{
    public Items wood;
    public void BuildDaBridge()
    {
        if (FindObjectOfType<InventorySystem>().HasEnough(wood, 150) && !GameManager.instance.hasBuiltBridge)
        {
            FindObjectOfType<InventorySystem>().CraftItem(wood, 150);
            GameObject bridge = GameObject.Find("bridge");
            bridge.GetComponent<MeshCollider>().enabled = true;
            bridge.GetComponent<MeshRenderer>().enabled = true;
            GameManager.instance.hasBuiltBridge = true;
            //StartCoroutine(FindObjectOfType<Dialouge>().GoThroughDialouge(FindObjectOfType<Dialouge>().NextTime));
        }
    }
}
