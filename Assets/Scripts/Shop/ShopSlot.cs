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
            if (Input.GetMouseButtonDown(1))
            {
                Turn();
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
    }

    public void Init(ItemInfo[] i)
    {
        turnPhase = 0;
        capacity.sprite = i[turnPhase].capacityIcon;
        icon.sprite = i[turnPhase].icon;
        items = i;

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

        var gridPosition = Inventory.instance.grid.GetGridPositionFromMousePosition(Input.mousePosition);
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = Input.mousePosition;
            Vector2 gridPosition = Inventory.instance.grid.GetGridPositionFromMousePosition(finalPos);
            if(Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
            {
                Inventory.instance.grid.AddItem(items, turnPhase, (int)gridPosition.x, (int)gridPosition.y);
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
