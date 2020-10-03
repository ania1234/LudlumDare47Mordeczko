using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<ItemInfo> items;
    public List<ShopSlot> slots;
    public GameObject slotPlace;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }

        GameManager.instance.onDayTimeChanged += OnDayTimeChanged;
        OnDayTimeChanged(GameManager.instance.DayTime);
    }

    private void OnDayTimeChanged(DayTimeEnum value)
    {
        slotPlace.SetActive(value == DayTimeEnum.day);
    }
}
