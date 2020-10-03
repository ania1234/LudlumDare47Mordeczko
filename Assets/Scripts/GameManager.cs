using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<int> onPassChanged = new Action<int>(x=>{});
    public event Action<DayTimeEnum> onDayTimeChanged = new Action<DayTimeEnum>(x => { });
    public event Action<float> onDayTimeLeftChanged = new Action<float>(x => { });

    public float dayDuration = 15.0f;

    public static GameManager instance;

    public int levelNumber { get; private set; }

    //może nie chcemy progressowaćdo nastepnego levelu jesli nie rozwalilismy wszystkich zagadek,
    //albo jestesmy w trakcie progressu
    public bool canProgressLevel { get; set; } = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void ProgressLevel(Transform teleportPoint)
    {
        StartCoroutine(ProgressLevelCoroutine(teleportPoint));
    }

    /// <summary>
    /// Coroutine of most importance
    /// </summary>
    /// <param name="teleportPoint">damn</param>
    private IEnumerator ProgressLevelCoroutine(Transform teleportPoint)
    {
        canProgressLevel = false;
        Player.instance.state.canMove = false;

        CameraManager.instance.RequestCameraFade(0.4f, true);
        yield return new WaitForSeconds(0.5f);
        Player.instance.gameObject.transform.position = teleportPoint.position;
        levelNumber++;
        onPassChanged(levelNumber);
        CameraManager.instance.RequestCameraFade(0.4f, false);
        yield return new WaitForSeconds(0.5f);

        onDayTimeChanged(DayTimeEnum.day);
        var waitEOF = new WaitForEndOfFrame();
        var timeLeft = dayDuration;
        while (timeLeft > 0)
        {
            onDayTimeLeftChanged(timeLeft);
            yield return waitEOF;
            timeLeft -= Time.deltaTime;
        }

        onDayTimeChanged(DayTimeEnum.night);
        Player.instance.state.canMove = true;
        canProgressLevel = true;
    }

    public void GameOver()
    {

    }
}
