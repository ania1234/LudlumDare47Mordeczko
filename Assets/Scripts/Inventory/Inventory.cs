using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventoryGrid grid;

    public int capacity = 10;

    private List<ItemInfo> items = new List<ItemInfo>();

    private void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemInfo item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
            grid.AddInFirstSpace(item);
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
