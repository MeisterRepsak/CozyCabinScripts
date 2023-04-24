using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tree : MonoBehaviour
{
    public Items wood;
    MeshRenderer rendere;
    Collider _collider;
    void Start()
    {
        _collider = GetComponent<Collider>();
        rendere = GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && InputManager.instance.doGoInCabin)
        {
            InputManager.instance.doGoInCabin = false;
            StartCoroutine(ChopTree());
        }
    }

    IEnumerator ChopTree()
    {
        rendere.enabled = false;
        _collider.enabled = false;
        FindObjectOfType<InventorySystem>().PutInInventory(wood, Random.Range(5,15));
        yield return new WaitForSeconds(60);
        rendere.enabled = true;
        _collider.enabled = true;
    }
}
