using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNeonSetter : MonoBehaviour
{
    public Image openShop;
    public Image closeShop;
    public Image close;
    public Image frameOpen;
    public Image frameClose;

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
        openShop.gameObject.SetActive(value == DayTimeEnum.day);
        closeShop.gameObject.SetActive(value == DayTimeEnum.night);
        close.gameObject.SetActive(value == DayTimeEnum.night);
        frameOpen.gameObject.SetActive(value == DayTimeEnum.day);
        frameClose.gameObject.SetActive(value == DayTimeEnum.night);
    }
}
