using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public Transform teleportPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.canProgressLevel)
        {
            if (collision.gameObject == Player.instance.gameObject)
            {
                Player.instance.gameObject.transform.position = teleportPoint.position;
                GameManager.instance.ProgressLevel();
            }
        }
    }
}
