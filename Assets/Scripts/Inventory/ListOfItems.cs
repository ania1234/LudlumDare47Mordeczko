using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfItems", menuName = "Data/New ListOfItems", order = 2)]
public class ListOfItems : ScriptableObject
{
    public ItemInfo[] items;
}

