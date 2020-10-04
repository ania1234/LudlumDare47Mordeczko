using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuPlay : MonoBehaviour
{
    public string sceneName = "SampleScene";

    Button button;
    private Camera mainCamera;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);

        mainCamera = Camera.main;
    }

    private void Clicked()
    {
        SceneManager.LoadScene(sceneName);

        var dimCam = mainCamera.GetComponent<DimScript>();
        dimCam.StopAllCoroutines();
        dimCam.MoveDimValue(0, 1, 0.4f);
    }
}
