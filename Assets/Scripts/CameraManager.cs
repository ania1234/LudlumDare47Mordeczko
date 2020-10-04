using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Color dayBackgroundColor;
    public Color nightBackgroundColor;

    public Camera mainCamera { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        mainCamera = Camera.main;
    }

    private IEnumerator Start()
    {
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
        var dimCam = mainCamera.GetComponent<DimScript>();
        dimCam.StopAllCoroutines();
        if (fadeIn)
        {
            dimCam.MoveDimValue(1, 0, fadeTime);
        }
        else
        {
            dimCam.MoveDimValue(0, 1, fadeTime);
        }
    }
}
