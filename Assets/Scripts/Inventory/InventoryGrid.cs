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

    public Vector2 gridSize;

    Vector2 cellSize = new Vector2(34f, 34f);

    Inventory inventory;
    List<Vector2> posItemNaBag = new List<Vector2>();

    void Start()
    {
        inventory = Inventory.instance;

        grid = new int[(int)gridSize.x, (int)gridSize.y];

        var capacity = gridSize.x * gridSize.y;

        for (int i = 0; i < capacity; i++)
        {
            var itemUI = Instantiate(slotPrefab, gridBackground);
        }
    }

    public bool AddInFirstSpace(ItemInfo item)
    {
        int contX = (int)item.size.x;
        int contY = (int)item.size.y;

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                if (posItemNaBag.Count != (contX * contY))
                {
                    for (int sizeY = 0; sizeY < contY; sizeY++)
                    {
                        for (int sizeX = 0; sizeX < contX; sizeX++)
                        {
                            if ((i + sizeX) < gridSize.x && (j + sizeY) < gridSize.y && grid[i + sizeX, j + sizeY] != 1)
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
