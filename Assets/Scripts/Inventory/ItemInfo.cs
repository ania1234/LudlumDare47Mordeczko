using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/New Item", order = 1)]
public class ItemInfo : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Vector2 size;
}
