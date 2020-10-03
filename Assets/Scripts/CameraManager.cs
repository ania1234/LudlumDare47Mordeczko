using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Color dayBackgroundColor;
    public Color nightBackgroundColor;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private IEnumerator Start()
    {
        mainCamera = Camera.main;
        while (GameManager.instance == null)
        {
            yield return null;
        }
        GameManager.instance.onDayTimeChanged += Instance_onDayTimeChanged; ;
    }

    private void Instance_onDayTimeChanged(DayTimeEnum obj)
    {
        if (obj == DayTimeEnum.day)
        {
            mainCamera.backgroundColor = dayBackgroundColor;
        }
        else
        {
            mainCamera.backgroundColor = nightBackgroundColor;
        }
    }

    public void RequestCameraFade(float fadeTime, bool fadeIn)
    {

    }
}
