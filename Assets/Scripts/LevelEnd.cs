using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public Transform teleportPoint;

    private IEnumerator Start()
    {
        while (GameManager_old.instance == null)
        {
            yield return null;
        }

        GameManager_old.instance.levelEnd = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager_old.instance.canProgressLevel)
        {
            if (collision.gameObject == Player.instance.gameObject)
            {
                GameManager_old.instance.ProgressLevel(teleportPoint);
            }
        }
    }
}
