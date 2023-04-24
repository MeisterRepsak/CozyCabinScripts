using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Items", menuName = "Create new Item")]
public class Items : ScriptableObject
{
   //static int currentid = 0;
   //Items()
   //{
   //    id = currentid;
   //    currentid++;
   //}
    [Header("DON'T TOUCH IDnnn \n" +
        "Hvis jeg ser nogen røre det bliver jeg aggressiv. Vh. Kasper Livoni Schnejder")]
    public int id;
    public Texture2D icon;
    public string itemName;
    public int maxStack;
}
