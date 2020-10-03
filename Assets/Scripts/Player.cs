using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState state;

    public static Player instance;

    public int jumpCount { get; internal set; }

    private string currentItemName;

    private void Awake()
    {
        state = new PlayerState();
        state.canMove = true;
        state.canJump = true;

        instance = this;
    }

    public bool HasItemEquipped(string itemName)
    {
        return currentItemName == itemName;
    }

    public void EquipItem(Item item)
    {
        currentItemName = item.info.name;
    }

    public void TakeBeatingFrom(Enemy enemy)
    {
        //TODO: loose health
        //TODO: some invincible routine to progress with lost health
    }
}
