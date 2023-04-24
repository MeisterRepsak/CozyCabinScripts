using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    public Knife knife;

    public Transform ingredientPose;
    public Canvas cookMenu;

    private void Start()
    {
        cookMenu.enabled = false;
    }

    public void openCookMenu()
    {
        cookMenu.enabled = true;
    }

    public void startRecipe1()
    {
        if (knife.potatoSpawned == false && knife.cookMenuOpen == true)
        {
            knife.potatoSpawned = true;
            GameObject temp = Instantiate(knife.potato, ingredientPose.position, Quaternion.identity);
            knife.ingredients = temp.GetComponent<ingredients>();
            cookMenu.enabled = false;
        }
    }
}
