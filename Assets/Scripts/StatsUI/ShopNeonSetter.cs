using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNeonSetter : MonoBehaviour
{
    public Image openShop;
    public Image closeShop;
    public Image close;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }

        GameManager.instance.onDayTimeChanged += OnDayTimeChanged;
    }

    private void OnDayTimeChanged(DayTimeEnum value)
    {
        openShop.gameObject.SetActive(value == DayTimeEnum.day);
        closeShop.gameObject.SetActive(value == DayTimeEnum.night);
        close.gameObject.SetActive(value == DayTimeEnum.night);
    }
}
