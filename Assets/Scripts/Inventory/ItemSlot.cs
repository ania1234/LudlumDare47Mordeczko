using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 startPosition;
    public ItemInfo item;
    public Image icon;
    public Vector2 size;

    private Vector2 oldPosition;
    private CanvasGroup group;

    InventoryGrid grid;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.size.y * size.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.size.x * size.x);

        group = GetComponent<CanvasGroup>();

        grid = GetComponentInParent<InventoryGrid>();
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

        for (int i = 0; i < item.size.y; i++)
        {
            for (int j = 0; j < item.size.x; j++)
            {
                grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = GetComponent<RectTransform>().anchoredPosition;

            Vector2 finalSlot;
            finalSlot.x = Mathf.Floor(finalPos.x / size.x);
            finalSlot.y = Mathf.Floor(-finalPos.y / size.y);

            if (((int)(finalSlot.x) + (int)(item.size.x) - 1) < grid.gridSize.x && ((int)(finalSlot.y) + (int)(item.size.y) - 1) < grid.gridSize.y && ((int)(finalSlot.x)) >= 0 && (int)finalSlot.y >= 0) // test if item is inside slot area
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
                    for (int i = 0; i < item.size.y; i++)
                    {
                        for (int j = 0; j < item.size.x; j++)
                        {
                            grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;

                        }
                    }

                    for (int i = 0; i < newPosItem.Count; i++)
                    {
                        grid.grid[(int)newPosItem[i].x, (int)newPosItem[i].y] = 1;
                    }

                    this.startPosition = newPosItem[0];
                    transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosItem[0].x * size.x, -newPosItem[0].y * size.y);
                }
                else
                {
                    for (int i = 0; i < item.size.y; i++)
                    {
                        for (int j = 0; j < item.size.x; j++)
                        {
                            grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 1;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < item.size.y; i++)
                {
                    for (int j = 0; j < item.size.x; j++)
                    {
                        grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;    
                    }
                }
                Destroy(gameObject);
            }
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
        group.blocksRaycasts = true; //register hit on item again
    }

    public void Clicked()
    {
        //if (item.usable)
        //{
        //    item.Use();


        //    Destroy(this.gameObject); //item drop
        //    Functionalities descript = FindObjectOfType<Functionalities>();

        //    descript.changeDescription("", "", 0, "");//clean description
        //}
    }
}
