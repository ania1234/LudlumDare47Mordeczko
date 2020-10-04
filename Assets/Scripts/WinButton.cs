using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinButton : MonoBehaviour
{
    public string sceneName = "Menu";

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        SceneManager.LoadScene(sceneName);
    }
}
