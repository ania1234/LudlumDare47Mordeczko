using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image capacity;
    public Image icon;

    public void Init(ItemInfo item)
    {
        capacity.sprite = item.capacityIcon;
        icon.sprite = item.icon;
    }
}
