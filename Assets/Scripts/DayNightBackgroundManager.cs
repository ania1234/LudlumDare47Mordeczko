using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightBackgroundManager : MonoBehaviour
{
    public List<SpriteRenderer> spritesToDim = new List<SpriteRenderer>();
    public List<SpriteRenderer> spritesToFlip = new List<SpriteRenderer>();
    public List<GameObject> dayOnlyGameObjects = new List<GameObject>();
    public List<GameObject> nightOnlyGameObjects = new List<GameObject>();

    //we can do it bc it's a jam game
    private OutputVolume[] bouncesToDelete;

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
        if (bouncesToDelete==null || bouncesToDelete.Length==0)
        {
            bouncesToDelete = GameManager_old.FindObjectsOfType<OutputVolume>();
        }

        if (obj == DayTimeEnum.day)
        {
            foreach(var bounce in bouncesToDelete)
            {
                bounce.enabled = false;
            }

            foreach(var sprite in spritesToDim)
            {
                sprite.color = new Color(0.2f, 0.2f, 0.2f);
            }

            foreach(var sprite in spritesToFlip)
            {
                sprite.transform.localScale = new Vector3(1, -1, 1);
            }

            foreach(var go in dayOnlyGameObjects)
            {
                go.SetActive(true);
            }

            foreach(var go in nightOnlyGameObjects)
            {
                go.SetActive(false);
            }
        }
        else
        {
            foreach(var bounce in bouncesToDelete)
            {
                bounce.enabled = true;
            }

            foreach (var sprite in spritesToDim)
            {
                sprite.color = Color.white;
            }

            foreach (var sprite in spritesToFlip)
            {
                sprite.transform.localScale = new Vector3(1, 1, 1);
            }

            foreach (var go in dayOnlyGameObjects)
            {
                go.SetActive(false);
            }

            foreach (var go in nightOnlyGameObjects)
            {
                go.SetActive(true);
            }
        }
    }
}
