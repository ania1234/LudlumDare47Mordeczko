using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventoryGrid grid;

    private void Awake()
    {
        instance = this;
    }
}
