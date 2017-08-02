using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DescriptionPanel : MonoBehaviour {


    #region singleton
    private DescriptionPanel() { }
    private static DescriptionPanel _instance = null;
    public  static DescriptionPanel Instance{
        get{
            return _instance;
        }
    }
    #endregion



    private Vector2 initPosition;
    private RectTransform rectTrasform;

    public Text NameText;
    public Text AttributeText;
    public Text DesText;
    public Text PriceText;


    private void Awake() {
        _instance = this;
        rectTrasform = GetComponent<RectTransform>();
        initPosition = rectTrasform.position;
    }

    public void DisplayItemInformation(Item item) {

        SetCommonText(item.Quality, item.Name, item.Description, item.SellPrice);

        switch (item.ItemType) {
            case EItemType.Consume: {

            }
            break;

            case EItemType.Equipment: {

            }
            break;

            case EItemType.Material: {

            }
            break;

            case EItemType.Weapon: {

            }
            break;

            default: break;
        }        
    }

    public void SetPosition(Vector2 UIPosition) {
        rectTrasform.anchoredPosition = UIPosition;
    }

    public void InitPosition() {
        rectTrasform.position = initPosition;
    }


    private void SetCommonText(EQuality quality,string name, string des, uint sellprice) {//名字 描述 售价是公共的Text区域
        //根据不同的品质显示不同的染色
        NameText.text = name;

        switch (quality) {
            case EQuality.Epic:
                NameText.color = new Color(1, 0, 1);//紫色
                break;
            case EQuality.Lengendary:
                NameText.color = Color.yellow;
                break;
            case EQuality.Rare:
                NameText.color = Color.blue;
                break;
            case EQuality.Normal:
                NameText.color = Color.grey;
                break;

            default: break;
        }

        DesText.text = des;
        PriceText.text = sellprice.ToString();

    }

}
