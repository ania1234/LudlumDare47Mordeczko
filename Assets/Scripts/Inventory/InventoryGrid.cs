using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
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

    void Start()
    {
        var rect = GetComponent<RectTransform>();

        grid = new int[(int)gridSize.x, (int)gridSize.y];
        cellSize = new Vector2(Screen.width * (rect.anchorMax.x - rect.anchorMin.x) / gridSize.x, Screen.height * (rect.anchorMax.y - rect.anchorMin.y) / gridSize.y);
        gridBackground.cellSize = cellSize;
        var capacity = gridSize.x * gridSize.y;

        for (int i = 0; i < capacity; i++)
        {
            Instantiate(slotPrefab, gridBackground.transform);
        }
    }

    public bool AddItem(ItemInfo item)
    {
        int contX = (int)item.size.x;
        int contY = (int)item.size.y;

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                if (posItemInBag.Count != (contX * contY))
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
                                posItemInBag.Add(pos);
                            }
                            else
                            {
                                sizeX = contX;
                                sizeY = contY;
                                posItemInBag.Clear();
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

        if (posItemInBag.Count == (contX * contY))
        {
            ItemSlot newSlot = Instantiate(prefabSlot, itemPlace);
            newSlot.startPosition = new Vector2(posItemInBag[0].x, posItemInBag[0].y);
            newSlot.item = item;
            newSlot.icon.sprite = item.icon;
            newSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newSlot.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            newSlot.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            newSlot.transform.SetParent(itemPlace, false);
            newSlot.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            newSlot.size = cellSize;
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(newSlot.startPosition.x * cellSize.x, -newSlot.startPosition.y * cellSize.y);
            itemsInBag.Add(newSlot);

            for (int k = 0; k < posItemInBag.Count; k++) //upgrade matrix
            {
                int posToAddX = (int)posItemInBag[k].x;
                int posToAddY = (int)posItemInBag[k].y;
                grid[posToAddX, posToAddY] = 1;
            }
            posItemInBag.Clear();
            return true;
        }
        return false;
    }
}
