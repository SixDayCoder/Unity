﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Grid : MonoBehaviour, IPointerEnterHandler{

    public Canvas UIRoot;
    public Image  DescriptionPanel;//鼠标放在图片上时显示的物品描述信息


    private Item item = null;//当前格子存放的item的信息
    private Image itemIcon;//item icon ui
    private Text itemNumber;//item number ui
    private RectTransform rectTransform;//grid的rectTransform

    private void Start() {
        //初始化itemIcon和itemNumber的引用
        foreach(var image in GetComponentsInChildren<Image>(true)) {
            if (image.name == "ItemIcon")
                itemIcon = image.GetComponent<Image>();
        }
        itemNumber = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    public bool IsEmpty() {
        return item == null ? true : false;
    }

    public void IncNumber(uint amount) {
        if (!IsEmpty()) {
            if(item.Capacity + amount <= 99) {
                item.Capacity += amount;
                //提示该物品数量已达到上限
            }
        }
        else {
            //LogError
        }
    }

    public void AddItem(Item item) {
        if (IsEmpty()) {
            this.item = item;
            string path = "Icons/Weapon/W_Axe001";
            itemIcon.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //itemIcon.sprite = Resources.Load(item.Icon, typeof(Sprite)) as Sprite;
            itemNumber.text = item.Capacity.ToString();
        }
        else {
            //LogError
        }
    }

    public Item GetItem() {
        return item;
    }

    public void RemoveItem() {
        if(!IsEmpty())
            item = null;
        else {
            //LogError
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Vector2 bias = new Vector2(100, -150);
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIRoot.transform as RectTransform,  
                                                                Camera.main.WorldToScreenPoint(rectTransform.position),
                                                                Camera.main,    out position);
        
        DescriptionPanel.rectTransform.anchoredPosition = position + bias;
    }
}