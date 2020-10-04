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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = Input.mousePosition;

            var rect = grid.GetComponent<RectTransform>();
            var gridPosMin = new Vector2(Screen.width * (rect.anchorMin.x), Screen.height * (rect.anchorMin.y));
            var gridPosMax = new Vector2(Screen.width * (rect.anchorMax.x), Screen.height * (rect.anchorMax.y));
            //Vector2 finalPos = capacity.GetComponent<RectTransform>().anchoredPosition;

            var posAdj = new Vector2(finalPos.x - gridPosMin.x, finalPos.y - gridPosMax.y);

            Vector2 finalSlot;
            finalSlot.x = Mathf.Floor(posAdj.x / grid.CellSize.x);
            finalSlot.y = Mathf.Floor(-posAdj.y / grid.CellSize.y);

            //Debug.Log($"{((int)(finalSlot.x) + (int)(item.size.x) - 1) < grid.gridSize.x} {((int)(finalSlot.y) + (int)(item.size.y) - 1) < grid.gridSize.y} {(int)(finalSlot.x) >= 0} {(int)finalSlot.y >= 0}");

            if (((int)(finalSlot.x) + (int)(item.size.x) - 1) < grid.gridSize.x 
                && ((int)(finalSlot.y) + (int)(item.size.y) - 1) < grid.gridSize.y 
                && ((int)(finalSlot.x)) >= 0 
                && (int)finalSlot.y >= 0)
            {
                List<Vector2> newPosItem = new List<Vector2>();
                bool fit = false;

                for (int sizeY = 0; sizeY < item.size.y; sizeY++)
                {
                    for (int sizeX = 0; sizeX < item.size.x; sizeX++)
                    {
                        if (grid.grid[(int)finalSlot.x + sizeX, (int)finalSlot.y + sizeY] != 1)
                        {
                            Vector2 pos;
                            pos.x = (int)finalSlot.x + sizeX;
                            pos.y = (int)finalSlot.y + sizeY;
                            newPosItem.Add(pos);
                            fit = true;
                        }
                        else
                        {
                            fit = false;

                            this.transform.GetComponent<RectTransform>().anchoredPosition = oldPosition;
                            sizeX = (int)item.size.x;
                            sizeY = (int)item.size.y;
                            newPosItem.Clear();

                        }

                    }

                }
                if (fit)
                {
                    Inventory.instance.AddItem(item, newPosItem);
                }
                else
                {
                    Inventory.instance.AddItem(item);
                }
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
