using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public bool randomItems = true;
    public List<ItemInfo> items;
    public List<ShopSlot> slots;
    public GameObject slotPlace;

    public ItemInfo itemToAdd1;
    public ItemInfo itemToAdd2;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }
        if (instance = this)
        {
            instance = this;
        }
        GameManager.instance.onDayTimeChanged += OnDayTimeChanged;
      //  OnDayTimeChanged(GameManager.instance.DayTime);
    }
    public void AddItem1()
    {
        items.Add(itemToAdd1);
        slotPlace.gameObject.SetActive(true);
        slots[3].gameObject.SetActive(true);
    }
    public void AddItem2()
    {
        items.Add(itemToAdd2);
        slotPlace.gameObject.SetActive(true);
        slots[4].gameObject.SetActive(true);
    }
    private void OnDayTimeChanged(DayTimeEnum value)
    {
        if (value == DayTimeEnum.day)
        {
            SetShop(); 
        }
        else
        {
            for (int slot = 0; slot < slots.Count; slot++)
            {
                slots[slot].ReturnToSlot();
            }
        }
        slotPlace.SetActive(value == DayTimeEnum.day);
    }

    void SetShop()
    {
        if (randomItems)
        {
            System.Random rnd = new System.Random();
            var randomizedList = from item in items
                                 orderby rnd.Next()
                                 select item;
            items = randomizedList.ToList();
        }
        for (int slot = 0; slot < items.Count; slot++)
        {
            slots[slot].Init(items[slot]);
        }
    }
}
