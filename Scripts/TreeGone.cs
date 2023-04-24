using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGone : MonoBehaviour
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

    public IEnumerator ChopTree()
    {
        rendere.enabled = false;
        _collider.enabled = false;
        FindObjectOfType<InventorySystem>().PutInInventory(wood, Random.Range(5, 15));
        foreach(GameObject go in GameManager.instance.trees)
        {
            if(gameObject == go)
            {
                GameManager.instance.trees.Remove(go);
                GameManager.instance.CheckIfTreesGone();
                yield break;
                
            }
        }
        yield return null;
    }
}
