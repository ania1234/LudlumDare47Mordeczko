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
}
