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
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.GetYSize() * size.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.GetXSize() * size.x);

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
        var pattern = item.GetPattern();

        for (int i = 0; i < item.GetYSize(); i++)
        {
            for (int j = 0; j < item.GetXSize(); j++)
            {
                if (pattern[i][j] == 1)
                {
                    grid.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = Input.mousePosition;
            Vector2 gridPosition = Inventory.instance.grid.GetGridPositionFromMousePosition(finalPos);
            if (Inventory.instance.grid.CanItemBePlacedAtPosition(item, (int)gridPosition.x, (int)gridPosition.y))
            {
                Inventory.instance.grid.AddItem(item, (int)gridPosition.x, (int)gridPosition.y);
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
