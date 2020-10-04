using System;
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

    internal void AddItem(ItemInfo item, int x, int y)
    {
        if (grid.AddItem(item, x, y))
        {
            items.Add(item);
        }
    }
}
