using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumberTracker : MonoBehaviour
{
    public TMPro.TMP_Text levelNumberText;

    private IEnumerator Start()
    {
        while (GameManager_old.instance == null)
        {
            yield return null;
        }
        GameManager_old.instance.onPassChanged += Instance_onPassChanged;
    }

    private void Instance_onPassChanged(int obj)
    {
        levelNumberText.SetText ( $"Level {obj}");
    }
}
