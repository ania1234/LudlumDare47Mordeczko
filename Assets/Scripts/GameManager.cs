using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public event Action<int> onPassChanged = new Action<int>(x=>{});
    public event Action<DayTimeEnum> onDayTimeChanged = new Action<DayTimeEnum>(x => { });
    public event Action<float> onDayTimeLeftChanged = new Action<float>(x => { });

    public float dayDuration = 15.0f;
    public int maxLevelNumber = 7;

    public static GameManager instance;
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;

    public Animator heroAnimator;
    public Animator allanimator;
    public string menuSceneName = "Menu";
    public string winSceneName = "WinScene";
    public bool isdead = false;
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
            //DontDestroyOnLoad(this);
        }
        else
        {
            //GameObject.Destroy(this.gameObject);
        }
    }
    public float timeLeft;
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
        if (!isdead)
        {
            StartCoroutine(ProgressLevelCoroutine(teleportPoint));
        }
    }

    /// <summary>
    /// Coroutine of most importance
    /// </summary>
    /// <param name="teleportPoint">damn</param>
    /// 
    public bool firsttime = true;
    private IEnumerator ProgressLevelCoroutine(Transform teleportPoint)
    {
        canProgressLevel = false;
        Player.instance.state.canMove = false;
        Player.instance.SetHealth( Player.instance.health+1);
        if (!firsttime)
        {
            CameraManager.instance.RequestCameraFade(0.4f, true);
            yield return new WaitForSeconds(0.4f);
            camera1.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
            allanimator.Play(0);
            heroAnimator.SetBool("IsWalking", true);
            yield return new WaitForSeconds(2f);
            //CameraManager.instance.RequestCameraFade(0.4f, true);
            yield return new WaitForSeconds(1.5f);
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);

        }
        yield return new WaitForSeconds(0.6f);
        CameraManager.instance.RequestCameraFade(0.4f, false);
        Player.instance.gameObject.transform.position = teleportPoint.position;
        firsttime = false;
        levelNumber++;
        onPassChanged(levelNumber);

        currentDayTime = DayTimeEnum.day;
        onDayTimeChanged(currentDayTime);
        yield return new WaitForSeconds(0.5f);

        var waitEOF = new WaitForEndOfFrame();
        timeLeft = dayDuration;
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
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < Inventory.instance.grid.gridSize.y; i++)
            {
                for (int ii = 0; ii < Inventory.instance.grid.gridSize.x; ii++)
                {
                    Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.white;
                }
            }
            Player.instance.state.canMove = true;
            canProgressLevel = true;

        }
        else
        {
            GameWin();
        }
    }
    public void EndTimer()
    {
        timeLeft = 0;
    }
    public void GameOver()
    {
        isdead = true;
        StartCoroutine(GameOverCorutine());


    }

    private IEnumerator GameOverCorutine()
    {
        Player.instance.state.canMove = false;
        CameraManager.instance.RequestCameraFade(0.3f, true);
        yield return new WaitForSeconds(0.4f);
        camera1.gameObject.SetActive(false);
        CameraManager.instance.RequestCameraFade(0.1f, false);
        camera3.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(menuSceneName);
    }

    public void GameWin()
    {
        SceneManager.LoadScene(winSceneName);
        CameraManager.instance.RequestCameraFade(0.4f, true);
    }
}
