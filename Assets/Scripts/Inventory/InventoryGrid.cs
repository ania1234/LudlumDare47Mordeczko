using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public int[,] grid;

    public List<ItemSlot> itemsInBag = new List<ItemSlot>();
    public ItemSlot prefabSlot;
    public Transform gridBackground;
    public Transform itemPlace;
    public GameObject slotPrefab;

    public int maxGridX;
    int maxGridY;

    Vector2 cellSize = new Vector2(34f, 34f);

    Inventory inventory;
    List<Vector2> posItemNaBag = new List<Vector2>();

    void Start()
    {
        inventory = Inventory.instance;

        maxGridY = (int)((inventory.capacity + 1) / maxGridX);

        grid = new int[maxGridX, maxGridY];

        for (int i = 0; i < inventory.capacity; i++)
        {
            var itemUI = Instantiate(slotPrefab, gridBackground);
        }
    }

    public bool AddInFirstSpace(ItemInfo item)
    {
        int contX = (int)item.size.x;
        int contY = (int)item.size.y;

        for (int i = 0; i < maxGridX; i++)
        {
            for (int j = 0; j < maxGridY; j++)
            {
                if (posItemNaBag.Count != (contX * contY))
                {
                    for (int sizeY = 0; sizeY < contY; sizeY++)
                    {
                        for (int sizeX = 0; sizeX < contX; sizeX++)
                        {
                            if ((i + sizeX) < maxGridX && (j + sizeY) < maxGridY && grid[i + sizeX, j + sizeY] != 1)
                            {
                                Vector2 pos;
                                pos.x = i + sizeX;
                                pos.y = j + sizeY;
                                posItemNaBag.Add(pos);
                            }
                            else
                            {
                                sizeX = contX;
                                sizeY = contY;
                                posItemNaBag.Clear();
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        if (posItemNaBag.Count == (contX * contY))
        {
            ItemSlot newSlot = Instantiate(prefabSlot, itemPlace);
            newSlot.startPosition = new Vector2(posItemNaBag[0].x, posItemNaBag[0].y);
            newSlot.item = item;
            newSlot.icon.sprite = item.icon;
            newSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newSlot.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            newSlot.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            newSlot.transform.SetParent(itemPlace, false);
            newSlot.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(newSlot.startPosition.x * cellSize.x, -newSlot.startPosition.y * cellSize.y);
            itemsInBag.Add(newSlot);

            for (int k = 0; k < posItemNaBag.Count; k++) //upgrade matrix
            {
                int posToAddX = (int)posItemNaBag[k].x;
                int posToAddY = (int)posItemNaBag[k].y;
                grid[posToAddX, posToAddY] = 1;
            }
            posItemNaBag.Clear();
            return true;
        }
        return false;
    }
}
