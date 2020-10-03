using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaytimeLeftTracker : MonoBehaviour
{
    public TMPro.TMP_Text dayTimeText;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }
        GameManager.instance.onDayTimeChanged += Instance_onDayTimeChanged;
        GameManager.instance.onDayTimeLeftChanged += Instance_onDayTimeLeftChanged;
    }

    private void Instance_onDayTimeLeftChanged(float obj)
    {
        dayTimeText.SetText($"Daytime left: {obj.ToString("0.00")}s");
    }

    private void Instance_onDayTimeChanged(DayTimeEnum obj)
    {
        dayTimeText.gameObject.SetActive(obj == DayTimeEnum.day);
    }
}
