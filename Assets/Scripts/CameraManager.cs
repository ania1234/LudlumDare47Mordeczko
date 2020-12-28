using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Color dayBackgroundColor;
    public Color nightBackgroundColor;

    public Camera mainCamera { get; private set; }
    private DimScript dimCam;

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
        while (GameManager_old.instance == null)
        {
            yield return null;
        }
        GameManager_old.instance.onDayTimeChanged += Instance_onDayTimeChanged; ;
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
        if (dimCam == null)
        {
            dimCam = mainCamera.GetComponent<DimScript>();
        }

        StopAllCoroutines();

        if (fadeIn)
        {
            StartCoroutine(MoveDimValueCoroutine(1, 0, fadeTime));
        }
        else
        {
            StartCoroutine(MoveDimValueCoroutine(0, 1, fadeTime));
        }
    }

    private IEnumerator MoveDimValueCoroutine(float startValue, float targetValue, float time)
    {
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            var dimValue = Mathf.Lerp(startValue, targetValue, timeElapsed / time);
            dimCam.SetDimValue(dimValue);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }
}
