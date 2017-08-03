using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Grid : MonoBehaviour,     IPointerEnterHandler, IPointerExitHandler,
                    IBeginDragHandler, IDragHandler,         IEndDragHandler
{

    public Canvas UIRoot;

    private Item          item = null;//当前格子存放的item的信息
    private Image         itemIcon;//item icon ui
    private Text          itemNumber;//item number ui
    private bool          isDraginng = false;//判断是否正在拖拽
    private RectTransform rectTransform;//grid的rectTransform
   

    private void Start() {
        //初始化itemIcon和itemNumber的引用
        foreach(var image in GetComponentsInChildren<Image>(true)) {//即便不可见也获取资源
            if (image.name == "ItemIcon")
                itemIcon = image.GetComponent<Image>();
        }
        itemNumber = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    #region add,remove,update item

    public bool IsEmpty() {
        return item == null ? true : false;
    }

    public void AddItem(Item item) {
        if (IsEmpty()) {
            this.item = item;
            string path = item.icon;
            itemIcon.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            itemNumber.text = item.capacity.ToString();
            itemIcon.GetComponent<RectTransform>().gameObject.SetActive(true);
        }
        else {
            //LogError
        }
    }

    public Item GetItem() {
        if(item == null) {
            //LogError
            return null;
        }
        return item;
    }

    public void RemoveItem() {
        if (!IsEmpty()) { 
            item = null;
            itemIcon.GetComponent<RectTransform>().gameObject.SetActive(false);  //设置不可见 
            itemIcon.sprite = null;
            itemNumber.text = "";
        }   
        else {
            //LogError
        }
    }

    #endregion


    #region pointer event
    public void OnPointerEnter(PointerEventData eventData) {
        if (!IsEmpty() && !isDraginng) {
            Vector2 bias = new Vector2(100, -150);
            Vector2 position;
            position = Word2UIPosition(rectTransform.position);
            DescriptionPanel.Instance.SetPosition(position + bias);
            DescriptionPanel.Instance.DisplayItemInformation(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        DescriptionPanel.Instance.InitPosition();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        DescriptionPanel.Instance.SetVisable(false);
        DragItem.Instance.BeginDragItem(item, itemIcon);
        isDraginng = true;//开始拖拽

    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 position = Word2UIPosition(eventData.position);
        DragItem.Instance.DragingItem(position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        //问题,如何在结束的时候获得结束位置的Grid的信息?使用射线检测

        GameObject go = eventData.pointerCurrentRaycast.gameObject;
                   
        if(go.tag == "Grid") {
            Grid grid = go.GetComponent<Grid>();
            if (grid.IsEmpty()) {//如果目标grid没有东西,直接覆盖
                grid.AddItem(DragItem.Instance.GetItem());
                RemoveItem();
            }
            else {//交换两者的数据
                Knapsack.Instance.ExangeItem(grid, this);
            }
        }

        DragItem.Instance.EndDragItem();
        isDraginng = false;//结束拖拽,移动到了新的位置 
    }
    #endregion


    private Vector2 Word2UIPosition(Vector3 wordPos) {
        //转换完毕后更改对象的 rectTransform.anchoredPosition
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIRoot.transform as RectTransform,
                                                                Camera.main.WorldToScreenPoint(wordPos),
                                                                Camera.main, out position);
        return position;
    }
}
