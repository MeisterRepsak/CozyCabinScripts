using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public ingredients ingredients;
    public manager manage;
    public GameObject[] cookingObjects;
    public Transform[] startPose;

    public int cut;

    public bool cookMenuOpen = false;


    //ingredients
    public GameObject potato;
    public bool potatoSpawned = false;

    public GameObject tomato;
    public bool tomatoSpawned = false;

    public GameObject onion;
    public GameObject apple;
    public GameObject berry;


    public void Update()
    {
        spawnCookTools();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            recipe1();
        }
    }


    public void spawnCookTools()
    {
        if (Input.GetKeyDown(KeyCode.E) && cookMenuOpen == false)
        {
            manage.openCookMenu();

            for (int i = 0; i < cookingObjects.Length; i++)
            {
                Instantiate(cookingObjects[i], startPose[i].position, Quaternion.identity);
            }

            cookMenuOpen = true;
        }
    }

    public void recipe1()
    {
        if (cut < ingredients.cuts && ingredients.gameObject.tag == "ingredient")
        {
            cut++;
            print("cut!");
        }
        else if (cut == ingredients.cuts && ingredients.gameObject.tag == "ingredient")
        {
            GameObject.Destroy(ingredients.gameObject);
            cut = 0;
        }

        if (cut == 0 && tomatoSpawned == false)
        {
            GameObject temp = Instantiate(tomato, manage.ingredientPose.position, Quaternion.identity);
            ingredients = temp.GetComponent<ingredients>();
            manage = temp.GetComponent<manager>();
            tomatoSpawned = true;
            cut = 1;
        }
    }
}
