﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 startPosition;
    public ItemInfo[] items;
    public Image icon;
    public Vector2 size;

    private Vector2 oldPosition;
    private CanvasGroup group;

    InventoryGrid grid;
    public int turnPhase = 0;

    private bool isDragging;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items[turnPhase].GetYSize() * size.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, items[turnPhase].GetXSize() * size.x);

        group = GetComponent<CanvasGroup>();

        grid = GetComponentInParent<InventoryGrid>();
    }

    void Update()
    {
        if (isDragging)
        {
            Debug.Log("Dragging");
            if (Input.GetMouseButtonDown(1))
            {
                Turn();
            }
        }
    }

    private void Turn()
    {
        turnPhase++;
        turnPhase = turnPhase%4;
        var rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items[turnPhase].GetYSize() * size.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, items[turnPhase].GetXSize() * size.x);
        icon.sprite = items[turnPhase].icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.itemName);
        //string title = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.itemName;
        //string body = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.itemDescription;
        //int attributte1 = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.getAtt1();
        //Sprite icon_attribute = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.getAtt1Icon();
        //string rarity = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.rarity;
        //Functionalities descript = FindObjectOfType<Functionalities>();

        //descript.changeDescription(title, body, attributte1, rarity, icon_attribute);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Functionalities descript = FindObjectOfType<Functionalities>();

        //descript.changeDescription("", "", 0, "");

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = transform.GetComponent<RectTransform>().anchoredPosition;

        group.blocksRaycasts = false;
    }



    public void OnDrag(PointerEventData eventData)
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x, pos.y, 0); //eventData.position;
        var pattern = items[turnPhase].GetPattern();

        for (int i = 0; i < items[turnPhase].GetYSize(); i++)
        {
            for (int j = 0; j < items[turnPhase].GetXSize(); j++)
            {
                if (pattern[i][j] == 1)
                {
                    grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;
                }
            }
        }

        var gridPosition = Inventory.instance.grid.GetGridPositionFromMousePosition(Input.mousePosition);

        if (Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.gray;
        }

        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        icon.color = Color.white;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = Input.mousePosition;
            Vector2 gridPosition = Inventory.instance.grid.GetGridPositionFromMousePosition(finalPos);
            if (Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
            {
                Inventory.instance.grid.AddItem(items, turnPhase, (int)gridPosition.x, (int)gridPosition.y);
            }
            Destroy(gameObject);
        }
        else
        {

            //PlayerController player;
            //player = FindObjectOfType<PlayerController>();

            //TetrisListItens itenInGame; // list of items prefab to could be instantiated when dropping item.
            //itenInGame = FindObjectOfType<TetrisListItens>();

            //for (int t = 0; t < itenInGame.prefabs.Length; t++)
            //{
            //    if (itenInGame.itens[t].itemName == item.itemName)
            //    {
            //        Instantiate(itenInGame.prefabs[t].gameObject, new Vector2(player.transform.position.x + Random.Range(-1.5f, 1.5f), player.transform.position.y + Random.Range(-1.5f, 1.5f)), Quaternion.identity); //dropa o item

            //        Destroy(this.gameObject);
            //        break;
            //    }

            //}

        }
        group.blocksRaycasts = true;
        for (int i = 0; i < Inventory.instance.grid.gridSize.y; i++)
        {
            for (int ii = 0; ii < Inventory.instance.grid.gridSize.x; ii++)
            {
                Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void Clicked()
    {
        Player.instance.EquipItem(items[turnPhase]);

        var pattern = items[turnPhase].GetPattern();

        for (int i = 0; i < items[turnPhase].GetYSize(); i++)
        {
            for (int j = 0; j < items[turnPhase].GetXSize(); j++)
            {
                if (pattern[i][j] == 1)
                {
                    grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;
                }
            }
        }
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Clicked();
        }
    }
}
