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

    #region set panel's position
    public void SetPosition(Vector2 UIPosition) {
        rectTrasform.anchoredPosition = UIPosition;
    }

    public void InitPosition() {
        rectTrasform.position = initPosition;
    }
    #endregion


    #region show items information

    public void SetVisable(bool flag) {
        gameObject.SetActive(flag);
    }

    public void DisplayItemInformation(Item item) {
        SetVisable(true);
        ClearText();
        SetCommonText(item.quality, item.name, item.description, item.sellprice);

        switch (item.itemtype) {
            case EItemType.Consume: {
                Consume c = item as Consume;
                SetConsumeText(c.hp, c.mp);
            }
            break;

            case EItemType.Equipment: {
                Equipment e = item as Equipment;
                SetEquipmentText(e.equipmentType, e.strength, e.agility, e.intellect, e.defend);
            }
            break;

            case EItemType.Material: {

            }
            break;

            case EItemType.Weapon: {

                Weapon w = item as Weapon;
                SetWeaponText(w.strength, w.agility, w.intellect, w.damage);
            }
            break;

            default: break;
        }
    }

    private void ClearText() {
        NameText.text = "";
        AttributeText.text = "";
        DesText.text = "";
        PriceText.text = "";
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
                NameText.color = Color.black;
                break;

            default: break;
        }

        DesText.text = des;
        PriceText.text = sellprice.ToString();

    }

    private void SetWeaponText(uint strength, uint agility, uint intellect, uint damage) {

        NameText.text += "(武器)";

        string text = @"
        <color=orange>力量:{0}</color>
        <color=green>敏捷:{1}</color>
        <color=blue>智力:{2}</color>
        <color=red>伤害:{3}</color>";

        string format = string.Format(text, strength.ToString(), agility.ToString(), intellect.ToString(), damage.ToString());
        AttributeText.text = format;

    }

    private void SetEquipmentText(EEquipmentType type,uint strength, uint agility, uint intellect, uint defend) {
        switch (type) {
            case EEquipmentType.Head:
            NameText.text += "(头盔)";
            break;

            case EEquipmentType.Belt:
            NameText.text += "(腰带)";
            break;

            case EEquipmentType.Leg:
            NameText.text += "(护腿)";
            break;

            case EEquipmentType.Wear:
            NameText.text += "(上衣)";
            break;

            case EEquipmentType.Ring:
            NameText.text += "(指环)";
            break;

            case EEquipmentType.Necklace:
            NameText.text += "(项链)";
            break;

            default:break;
        }

        string text = @"
        <color=orange>力量:{0}</color>
        <color=green>敏捷:{1}</color>
        <color=blue>智力:{2}</color>
        <color=black>防御:{3}</color>";

        string format = string.Format(text, strength.ToString(), agility.ToString(), intellect.ToString(), defend.ToString());
        AttributeText.text = format;
    }

    private void SetConsumeText(uint hp, uint mp) {
        NameText.text += "(恢复品)";

        string text = @"
        <color=red>生命恢复:{0}</color>
        <color=blue>魔法恢复:{2}</color>";

        string format = string.Format(text, hp.ToString(), mp.ToString());
        AttributeText.text = format;
    }
    #endregion 
}
