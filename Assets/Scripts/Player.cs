using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState state;

    public static Player instance;

    private void Awake()
    {
        state = new PlayerState();
        state.canMove = true;
        state.canJump = true;

        instance = this;
    }
}
