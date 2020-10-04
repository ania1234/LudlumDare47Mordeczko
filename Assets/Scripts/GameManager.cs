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
    public int maxLevelNumber = 7;

    public static GameManager instance;

    public int levelNumber { get; private set; }

    //może nie chcemy progressowaćdo nastepnego levelu jesli nie rozwalilismy wszystkich zagadek,
    //albo jestesmy w trakcie progressu
    public bool canProgressLevel { get; set; } = true;

    private DayTimeEnum currentDayTime;
    public LevelEnd levelEnd { get; set; }
    public DayTimeEnum DayTime  => currentDayTime;

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

    private void Start()
    {
        if (levelEnd == null)
        {
            levelEnd = GameObject.FindObjectOfType<LevelEnd>();
        }

        levelNumber = -1;
        ProgressLevel(levelEnd.teleportPoint);
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

        currentDayTime = DayTimeEnum.day;
        onDayTimeChanged(currentDayTime);
        CameraManager.instance.RequestCameraFade(0.4f, false);
        yield return new WaitForSeconds(0.5f);

        var waitEOF = new WaitForEndOfFrame();
        var timeLeft = dayDuration;
        while (timeLeft > 0)
        {
            onDayTimeLeftChanged(timeLeft);
            yield return waitEOF;
            timeLeft -= Time.deltaTime;
        }

        if (levelNumber < maxLevelNumber)
        {
            currentDayTime = DayTimeEnum.night;
            onDayTimeChanged(currentDayTime);
            Player.instance.state.canMove = true;
            canProgressLevel = true;
        }
        else
        {
            GameWin();
        }
    }

    public void GameOver()
    {

    }

    public void GameWin()
    {

    }
}
