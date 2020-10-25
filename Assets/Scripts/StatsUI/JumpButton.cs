using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public void OnClick()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Player.instance.state.canJump)
        {
            InputManager.instance.SetShouldJump();
        }
#endif
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount==1 && Player.instance.state.canJump)
        {
            InputManager.instance.SetShouldJump();
        }
#endif
    }

    public void ToggleOnlyRotate(bool value)
    {
        InputManager.instance.OnlyRotateOnButtonClick = value;
    }

    public void Rotate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        InputManager.instance.SetShouldRotate();
#endif
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            InputManager.instance.SetShouldRotate();
        }
#endif
    }
}
