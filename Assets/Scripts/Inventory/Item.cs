using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemInfo info;

    public Vector2 Size => info.size;

    public Sprite Icon => info.icon;
}
