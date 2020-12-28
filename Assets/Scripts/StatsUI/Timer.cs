using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Timer : MonoBehaviour
{
    Image image;

    private IEnumerator Start()
    {
        while (GameManager_old.instance == null)
        {
            yield return null;
        }

        image = GetComponent<Image>();

        GameManager_old.instance.onDayTimeChanged += OnDayTimeChanged;
        GameManager_old.instance.onDayTimeLeftChanged += OnDayTimeLeftChanged;
        OnDayTimeChanged(GameManager_old.instance.DayTime);
    }

    private void OnDayTimeLeftChanged(float value)
    {
        image.fillAmount = Mathf.Lerp(0, 1, Mathf.InverseLerp(0, GameManager_old.instance.dayDuration, value));
    }

    private void OnDayTimeChanged(DayTimeEnum value)
    {
        image.fillAmount = 1;
    }
}
