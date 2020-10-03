using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            Player.instance.jumpCount = 0;
        }
    }
}
