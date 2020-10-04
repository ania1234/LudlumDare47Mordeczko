using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState state;

    public static Player instance;

    public SpriteRenderer itemSprite;

    public int jumpCount { get; internal set; }

    public int maxHealth;

    public int health { get; private set; }
    private string CurrentItemName { 
        get => currentItemName;
        set {
            currentItemName = value;
            if (string.IsNullOrEmpty(value))
            {
                itemSprite.enabled = false;
            }
        }
    }

    private string currentItemName;

    public Animator animator;


    #region signals
    public event Action<int> onHealthChanged = new Action<int>(x => { });
    #endregion

    private void Awake()
    {
        state = new PlayerState();
        state.canMove = false;
        state.canJump = true;
        health = maxHealth;
        instance = this;
    }

    public bool HasItemEquipped(string itemName)
    {
        var answer = CurrentItemName == itemName;
        if (answer)
        {
            CurrentItemName = "";
        }
        return answer;
    }

    public void EquipItem(ItemInfo item)
    {
        CurrentItemName = item.name;
        itemSprite.enabled = true;
        itemSprite.sprite = item.icon;
    }

    public void TakeBeatingFrom(Enemy enemy)
    {
        //TODO: loose health
        SetHealth(health - 1);
        //TODO: some invincible routine to progress with lost health
    }

    public void SetHealth(int newValue)
    {
        health = Mathf.Min(maxHealth, Mathf.Max(newValue, 0));
        onHealthChanged(health);
    }
}
