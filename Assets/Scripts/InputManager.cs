using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private bool _shouldJumpUpdateNextFrame = false;

    public bool ShouldJump { get; private set; }

    private bool _shouldRotateNextFrame = false;

    internal void SetShouldRotate()
    {
        _shouldRotateNextFrame = true;
    }
    internal void ConsumeShouldRotate()
    {
        ShouldRotate = false;
    }

    internal void SetShouldJump()
    {
        _shouldJumpUpdateNextFrame = true;
    }

    internal void ConsumeShouldJump()
    {
        ShouldJump = false;
    }

    public Vector2 MousePosition { get; private set; }
    public bool ShouldRotate { get; private set; }
    public bool OnlyRotateOnButtonClick { get; internal set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        MousePosition = Input.mousePosition;
        ShouldRotate = Input.GetMouseButton(0) && Input.GetMouseButtonDown(1);
        if (_shouldJumpUpdateNextFrame)
        {
            _shouldJumpUpdateNextFrame = false;
            ShouldJump = true;
        }
        else
        {
            ShouldJump = Input.GetButtonDown("Jump");
        }
#endif
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touches.Length > 0)
        {
            MousePosition = Input.GetTouch(0).position;
        }

        if (_shouldRotateNextFrame)
        {
            ShouldRotate = true;
        }

        if (!OnlyRotateOnButtonClick)
        {
            if (Input.touches.Length == 2 && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                ShouldRotate = true;
            }
        }

        if (_shouldJumpUpdateNextFrame)
        {
            _shouldJumpUpdateNextFrame = false;
            ShouldJump = true;
        }
#endif
    }
}
