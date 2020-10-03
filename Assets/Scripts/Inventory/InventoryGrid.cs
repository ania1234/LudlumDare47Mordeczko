using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public int[,] grid;//2 dimensions

    public List<ItemSlot> itemsInBag = new List<ItemSlot>();

    public int maxGridX;
    int maxGridY;

    public ItemSlot prefabSlot;
    Vector2 cellSize = new Vector2(34f, 34f);

    Inventory inventory;
    List<Vector2> posItemNaBag = new List<Vector2>();

    void Start()
    {
        inventory = Inventory.instance;

        maxGridY = (int)((inventory.capacity + 1) / maxGridX);

        grid = new int[maxGridX, maxGridY];
    }

    public bool AddInFirstSpace(ItemInfo item)
    {
        int contX = (int)item.size.x;
        int contY = (int)item.size.y;

        for (int i = 0; i < maxGridX; i++)
        {
            for (int j = 0; j < maxGridY; j++)
            {
                if (posItemNaBag.Count != (contX * contY)) // if false, the item fit the bag
                {
                    //for each x,y position (i,j), test if item fits
                    for (int sizeY = 0; sizeY < contY; sizeY++) // item size in Y
                    {
                        for (int sizeX = 0; sizeX < contX; sizeX++)//item size in X
                        {
                            if ((i + sizeX) < maxGridX && (j + sizeY) < maxGridY && grid[i + sizeX, j + sizeY] != 1)//inside of index
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

        if (posItemNaBag.Count == (contX * contY)) // if item already in bag
        {
            ItemSlot newSlot = Instantiate(prefabSlot);
            newSlot.startPosition = new Vector2(posItemNaBag[0].x, posItemNaBag[0].y);
            newSlot.item = item;
            newSlot.icon.sprite = item.icon;
            newSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newSlot.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            newSlot.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            newSlot.transform.SetParent(this.GetComponent<RectTransform>(), false);
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
            Debug.Log("COunt: " + itemsInBag.Count);
            return true;
        }
        return false;
    }
}
