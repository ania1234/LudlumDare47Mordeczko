using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMovementSpeed;
    public float jumpForce;
    public Player player;

    private bool shouldJump;
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            shouldJump = true;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!player.state.canMove)
        {
            return;
        }

        var xMovement = Input.GetAxis("Horizontal");
        var yMovement = Input.GetAxis("Vertical");

        if (xMovement < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }

        player.transform.position = player.transform.position + new Vector3(1, 0, 0) * xMovement * horizontalMovementSpeed * Time.fixedDeltaTime;

        if (shouldJump && player.state.canJump)
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            shouldJump = false;
        }
    }
}
