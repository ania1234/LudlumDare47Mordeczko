using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    private float _lastTimeClicked = 0;
    

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
        turnPhase = turnPhase%4;
        var rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items[turnPhase].GetYSize() * size.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, items[turnPhase].GetXSize() * size.x);
        icon.sprite = items[turnPhase].icon;

        var gridPosition = Inventory.instance.grid.GetGridPositionFromWorldPosition(this.transform.position);
        if (Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
        {

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = transform.GetComponent<RectTransform>().anchoredPosition;

        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = Camera.main.ScreenToWorldPoint(InputManager.instance.MousePosition);
        transform.position = new Vector3(pos.x, pos.y, 0) + GameManager_old.instance.draggableItemOffset; //eventData.position;
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

        var gridPosition = Inventory.instance.grid.GetGridPositionFromWorldPosition(this.transform.position);

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

        var pos = Camera.main.ScreenToWorldPoint(InputManager.instance.MousePosition);
        var finalPos = new Vector3(pos.x, pos.y, 0) + GameManager_old.instance.draggableItemOffset;
        Vector2 gridPosition = Inventory.instance.grid.GetGridPositionFromWorldPosition(finalPos);
        if (Inventory.instance.grid.CanItemBePlacedAtPosition(items[turnPhase], (int)gridPosition.x, (int)gridPosition.y))
        {
            Inventory.instance.grid.AddItem(items, turnPhase, (int)gridPosition.x, (int)gridPosition.y);
        }
        Destroy(gameObject);

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
        if(Time.time - _lastTimeClicked < GameManager_old.instance.doubleClickDuration)
        { 
            Clicked();
        }
        else
        {
            _lastTimeClicked = Time.time;
        }
    }
}
