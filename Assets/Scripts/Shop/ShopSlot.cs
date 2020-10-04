﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    public Image capacity;
    public Image icon;
    public InventoryGrid grid;

    private Vector2 oldPosition;
    private CanvasGroup group;
    ItemInfo item;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        oldPosition = capacity.GetComponent<RectTransform>().anchoredPosition;
    }

    public void Init(ItemInfo i)
    {
        capacity.sprite = i.capacityIcon;
        icon.sprite = i.icon;
        item = i;

        group.blocksRaycasts = true;
        ReturnToSlot();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = false;
        capacity.transform.SetParent(grid.itemPlace);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        capacity.transform.position = new Vector3(pos.x, pos.y, 0); //eventData.position;
        //Debug.Log(Input.mousePosition);

        //Inventory.instance.grid.GetGridPositionFromMousePosition(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = Input.mousePosition;
<<<<<<< HEAD

            var rect = grid.GetComponent<RectTransform>();
            
            var gridPosMin = new Vector2(Screen.width * (rect.sizeDelta.x), Screen.height * (rect.anchorMin.y));
            var gridPosMax = new Vector2(Screen.width * (rect.anchorMax.x), Screen.height * (rect.anchorMax.y));
            //Vector2 finalPos = capacity.GetComponent<RectTransform>().anchoredPosition;

            var posAdj = new Vector2(finalPos.x - gridPosMin.x, finalPos.y - gridPosMax.y);

            Vector2 finalSlot;
            finalSlot.x = Mathf.Floor(posAdj.x / grid.CellSize.x);
            finalSlot.y = Mathf.Floor(-posAdj.y / grid.CellSize.y);

            Debug.Log($"{((int)(finalSlot.x) + (int)(item.size.x) - 1) < grid.gridSize.x} {((int)(finalSlot.y) + (int)(item.size.y) - 1) < grid.gridSize.y} {(int)(finalSlot.x) >= 0} {(int)finalSlot.y >= 0}");

            if (((int)(finalSlot.x) + (int)(item.size.x) - 1) < grid.gridSize.x 
                && ((int)(finalSlot.y) + (int)(item.size.y) - 1) < grid.gridSize.y 
                && ((int)(finalSlot.x)) >= 0 
                && (int)finalSlot.y >= 0)
=======
            Vector2 gridPosition = Inventory.instance.grid.GetGridPositionFromMousePosition(finalPos);
            if(Inventory.instance.grid.CanItemBePlacedAtPosition(item, (int)gridPosition.x, (int)gridPosition.y))
>>>>>>> Refactor inventory
            {
                Inventory.instance.grid.AddItem(item, (int)gridPosition.x, (int)gridPosition.y);
            }
        }
        ReturnToSlot();

        group.blocksRaycasts = true;
    }

    public void ReturnToSlot()
    {
        capacity.transform.SetParent(transform);
        capacity.GetComponent<RectTransform>().anchoredPosition = oldPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
