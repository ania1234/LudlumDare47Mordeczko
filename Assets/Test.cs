using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ItemInfo item;
    public ItemInfo item2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Inventory.instance.AddItem(item);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Inventory.instance.AddItem(item2);
        }
    }
}
