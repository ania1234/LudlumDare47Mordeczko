using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventoryGrid grid;

    private List<ItemInfo> items = new List<ItemInfo>();

    private void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemInfo item)
    {
        if (grid.AddInFirstSpace(item))
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
