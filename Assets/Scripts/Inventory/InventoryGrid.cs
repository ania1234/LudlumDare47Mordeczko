﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    /// <summary>
    /// first x coordinate, then y
    /// </summary>
    public int[,] grid;

    public List<ItemSlot> itemsInBag = new List<ItemSlot>();
    public ItemSlot prefabSlot;
    public GridLayoutGroup gridBackground;
    public Transform itemPlace;
    public GameObject slotPrefab;

    public Vector2 gridSize;

    Vector2 cellSize = new Vector2(130f, 100f);

    List<Vector2> posItemInBag = new List<Vector2>();

    public Vector2 CellSize { get => cellSize; }

    public List<GameObject> slotPrefabs = new List<GameObject>();

    void Start()
    {
        var rect = GetComponent<RectTransform>();

        grid = new int[(int)gridSize.x, (int)gridSize.y];
        cellSize = new Vector2(GetComponent<RectTransform>().sizeDelta.x / gridSize.x, GetComponent<RectTransform>().sizeDelta.y / gridSize.y);
        gridBackground.cellSize = cellSize;
        var capacity = gridSize.x * gridSize.y;

        for (int i = 0; i < capacity; i++)
        {
            slotPrefabs.Add(Instantiate(slotPrefab, gridBackground.transform));
        }
    }

    internal bool AddItem(ItemInfo[] items, int turnPhase, int x, int y)
    {
        var item = items[turnPhase];
        if(!CanItemBePlacedAtPosition(item, x, y))
        {
            return false;
        }

        ItemSlot newSlot = Instantiate(prefabSlot, itemPlace);
        newSlot.startPosition = new Vector2(x, y);
        newSlot.items = items;
        newSlot.turnPhase = turnPhase;
        newSlot.icon.sprite = item.icon;
        newSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        newSlot.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
        newSlot.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        newSlot.transform.SetParent(itemPlace, false);
        newSlot.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        newSlot.size = cellSize;
        newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * cellSize.x, -y * cellSize.y);
        itemsInBag.Add(newSlot);

        //let's color the grid accordingly
        var itemPattern = item.GetPattern();

        //items are held by the middle 
        int itemHeight = itemPattern.Count;
        int itemWidth = itemPattern[0].Length;

        //if items are held by the middle 
        //int itemMiddlePointX = itemWidth / 2;
        //int itemMiddlePointY = itemHeight / 2;

        //int itemMinXPositionInInventory = x - itemMiddlePointX;
        //int itemMaxXpositionInInventory = x + itemMiddlePointX;
        //int itemMinYPositionInInventory = y - itemMiddlePointY;
        //int itemMaxYPositionInInventory = y + itemMiddlePointY;

        //if items are held by upper left corner
        int itemMinXPositionInInventory = x;
        int itemMaxXpositionInInventory = x + itemWidth;
        int itemMinYPositionInInventory = y;
        int itemMaxYPositionInInventory = y + itemHeight;

        for (int yPos = itemMinYPositionInInventory; yPos < itemMaxYPositionInInventory; yPos++)
        {
            for (int xPos = itemMinXPositionInInventory; xPos < itemMaxXpositionInInventory; xPos++)
            {
                grid[xPos, yPos] = itemPattern[yPos - itemMinYPositionInInventory][xPos - itemMinXPositionInInventory];
            }
        }

        return true;
    }

    /// <summary>
    /// Returns -1, -1 if we are too far away from the grid
    /// </summary>
    internal Vector2 GetGridPositionFromWorldPosition(Vector3 worldPosition)
    {
        int slotPrefabIndex = -1;
        float minDistance = float.MaxValue;
        var mousePosition = CameraManager.instance.mainCamera.WorldToScreenPoint(worldPosition);
        for (int i =0; i<slotPrefabs.Count; i++)
        {
            var slotPrefabScreenPosition = CameraManager.instance.mainCamera.WorldToScreenPoint(slotPrefabs[i].transform.position);
            var distance = Vector2.Distance(mousePosition, slotPrefabScreenPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                slotPrefabIndex = i;
            }
        }

        var gridPositionX = slotPrefabIndex % (int)gridSize.x;
        var gridPositionY = Math.Floor( (float)slotPrefabIndex / gridSize.x);
        var result = new Vector2(gridPositionX, (int)gridPositionY);
        //Debug.Log($"Slot prefab: {slotPrefabIndex} Distance: {minDistance} Result: {result}");

        if (minDistance > 50)
        {
            return new Vector2(-1, -1);
        }

        return result;
    }

    public bool CanItemBePlacedAtPosition(ItemInfo item, int x, int y)
    {
        if(x<0 || y < 0)
        {
            return false;
        }

        var itemPattern = item.GetPattern();


        int itemHeight = itemPattern.Count;
        int itemWidth = itemPattern[0].Length;

        //if items are held by the middle 
        //int itemMiddlePointX = itemWidth / 2;
        //int itemMiddlePointY = itemHeight / 2;

        //int itemMinXPositionInInventory = x - itemMiddlePointX;
        //int itemMaxXpositionInInventory = x + itemMiddlePointX;
        //int itemMinYPositionInInventory = y - itemMiddlePointY;
        //int itemMaxYPositionInInventory = y + itemMiddlePointY;

        //if items are held by upper left corner
        int itemMinXPositionInInventory = x ;
        int itemMaxXpositionInInventory = x + itemWidth;
        int itemMinYPositionInInventory = y ;
        int itemMaxYPositionInInventory = y + itemHeight;

        bool result = true;

        if(
            itemMinXPositionInInventory<0 || itemMinYPositionInInventory<0 ||
            itemMaxXpositionInInventory>gridSize.x || itemMaxYPositionInInventory>gridSize.y
            )
        {
            return false;
        }
        for(int i=0; i< gridSize.y; i++)
        {
            for (int ii = 0; ii < gridSize.x; ii++)
            {
                if(i< itemMaxYPositionInInventory && i>= itemMinYPositionInInventory && ii< itemMaxXpositionInInventory && ii >= itemMinXPositionInInventory)
                {

                    if (itemPattern[i - itemMinYPositionInInventory][ii - itemMinXPositionInInventory] == 1)
                    {
                        if (grid[ii, i] == 1)
                        {

                            Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.red;
                        }
                        else
                        {


                            Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.green;
                        }
                    }
                    else
                    {

                        Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.white;
                    }
                }
                else
                {

                    Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.white;
                }
            }

        }

        for (int yPos = itemMinYPositionInInventory; yPos < itemMaxYPositionInInventory; yPos++)
        {
            for(int xPos = itemMinXPositionInInventory; xPos < itemMaxXpositionInInventory; xPos++)
            {

                if(itemPattern[yPos- itemMinYPositionInInventory][xPos- itemMinXPositionInInventory] == 1 &&
                    grid[xPos, yPos] == 1)
                {
                    result = false;
                }
            }
        }

        return result;
    }
}
