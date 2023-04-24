using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCookRecipe : MonoBehaviour
{
    public string RecipeFunction;
    public Items requirement;
    public void CookRecipe()
    {
        FindObjectOfType<InventorySystem>().HasEnough(requirement, 1);

        FindObjectOfType<CampFire>().Invoke(RecipeFunction,0);
    }
}
