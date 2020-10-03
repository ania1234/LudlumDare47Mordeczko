using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public Transform teleportPoint;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }

        GameManager.instance.levelEnd = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.canProgressLevel)
        {
            if (collision.gameObject == Player.instance.gameObject)
            {
                GameManager.instance.ProgressLevel(teleportPoint);
            }
        }
    }
}
