﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMovementSpeed;
    public float jumpForce;
    public int maxJumpCount = 3;
    public Player player;

    private bool shouldJump;
    private void Update()
    {
        if (InputManager.instance.ShouldJump)
        {
            shouldJump = true;
        }
        InputManager.instance.ConsumeShouldJump();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        player.animator.SetBool("IsWalking", player.state.canMove);

        if (!player.state.canMove)
        {
            return;
        }

        var xMovement = 1;

        player.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        player.transform.position = player.transform.position + new Vector3(1, 0, 0) * xMovement * horizontalMovementSpeed * Time.fixedDeltaTime;

        if (shouldJump)
        {
            player.jumpCount++;
            if (player.state.canJump && player.jumpCount < maxJumpCount)
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            }
            shouldJump = false;
        }
    }
}
