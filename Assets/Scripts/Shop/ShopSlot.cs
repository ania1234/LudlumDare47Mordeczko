using System.Collections;
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
    ItemInfo[] items;

    private bool isDragging;
    private bool shouldTurn;

    int turnPhase = 0;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        oldPosition = capacity.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if (isDragging )
        {
            if (InputManager.instance.ShouldRotate)
            {
                Turn();
                InputManager.instance.ConsumeShouldRotate();
            }
        }
    }

    private void Turn()
    {
        turnPhase++;
        turnPhase = turnPhase % 4;
        capacity.sprite = items[turnPhase].capacityIcon;
        icon.sprite = items[turnPhase].icon;

        var rect = capacity.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items[turnPhase].capacityIcon.texture.height);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, items[turnPhase].capacityIcon.texture.width);

        var rect2 = icon.GetComponent<RectTransform>();
        rect2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items[turnPhase].icon.texture.height);
        rect2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, items[turnPhase].icon.texture.width);

        var gridPosition = Inventory.instance.grid.GetGridPositionFromWorldPosition(capacity.transform.position);
        if (Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
        {

        }
    }

    public void Init(ItemInfo[] i)
    {
        turnPhase = 0;
        capacity.sprite = i[turnPhase].capacityIcon;
        icon.sprite = i[turnPhase].icon;
        items = i;

        group.blocksRaycasts = true;

        capacity.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 128);
        icon.GetComponent<RectTransform>().sizeDelta = new Vector2(190, 128);

        ReturnToSlot();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = false;
        capacity.transform.SetParent(grid.itemPlace);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = Camera.main.ScreenToWorldPoint(InputManager.instance.MousePosition);
        capacity.transform.position = new Vector3(pos.x, pos.y, 0) + GameManager.instance.draggableItemOffset; 

        var gridPosition = Inventory.instance.grid.GetGridPositionFromWorldPosition(capacity.transform.position);
        if (Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
        {
            icon.color = Color.white;
            capacity.color = Color.white;
        }
        else
        {
            icon.color = Color.gray;
            capacity.color = Color.red;
        }

        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        icon.color = Color.white;
        capacity.color = Color.white;

        var pos = Camera.main.ScreenToWorldPoint(InputManager.instance.MousePosition);
        var finalPos = new Vector3(pos.x, pos.y, 0) + GameManager.instance.draggableItemOffset;
        Vector2 gridPosition = Inventory.instance.grid.GetGridPositionFromWorldPosition(finalPos);
        if(Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
        {
            Inventory.instance.grid.AddItem(items, turnPhase, (int)gridPosition.x, (int)gridPosition.y);
        }

        ReturnToSlot();

        group.blocksRaycasts = true;
        for (int i = 0; i < Inventory.instance.grid.gridSize.y; i++)
        {
            for (int ii = 0; ii < Inventory.instance.grid.gridSize.x; ii++)
            {
                Inventory.instance.grid.slotPrefabs[ii + i * 9].GetComponent<Image>().color = Color.white;
            }
        }
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
