using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int capacity = 10;

    private List<ItemInfo> items = new List<ItemInfo>();

    public void AddItem(ItemInfo item)
    {
        if (items.Count < capacity)
        {
            items.Add(item); 
        }
    }

    public void RemoveItem(ItemInfo item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }
}
