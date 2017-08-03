using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour {

    #region singleton
    private DragItem() {
    }
    private static DragItem _instance = null;
    public  static DragItem Instance {
        get {
            return _instance;
        }
    }
    #endregion

    private Image image;
    private Item  item;//当前正在被拖拽的item的信息
    private RectTransform rectTransform;


    private void Awake() {
        _instance = this;   
    }
    private void Start() {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void SetVisable(bool flag) {
        gameObject.SetActive(flag);
    }

    //开始拖拽,保存被拖拽的item的信息,设置DragItem面板的位置
    public void BeginDragItem(Item item, Image itemUI) {
        SetVisable(true);
        image.sprite = itemUI.sprite;
        this.item = item;
    }

    //拖拽当中,更新DragItem的位置
    public void DragingItem(Vector2 UIPosition) {
        rectTransform.anchoredPosition = UIPosition;
    }

    //拖拽结束,让DragItem不再显示,并且丢弃持有的item的信息
    public void EndDragItem() {
        SetVisable(false);
        item = null;
    }

    //获取当前持有的Item的信息
    public Item GetItem() {
        if(item == null) {
            //LogError
            return null;
        }
        return item;
    }

}
