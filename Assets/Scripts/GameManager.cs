using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<int> onPassChanged = new Action<int>(x=>{});
    public static GameManager instance;

    public int levelNumber { get; private set; }

    //może nie chcemy progressowaćdo nastepnego levelu jesli nie mamy 
    public bool canProgressLevel = true;

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

    public void ProgressLevel()
    {
        levelNumber++;
        onPassChanged(levelNumber);
    }

    public void GameOver()
    {

    }
}
