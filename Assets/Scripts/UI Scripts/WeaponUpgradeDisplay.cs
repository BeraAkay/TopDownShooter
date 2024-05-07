using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponUpgradeDisplay : MonoBehaviour//this could share an interface with PersistentUpgradeDisplay maybe but i just want to wrap this project up so i will not
{
    Weapon weapon;

    WeaponUpgradeManager managerRef;

    [SerializeField]
    TextMeshProUGUI nameText, levelText, costText;

    [SerializeField]
    Button increaseButton, decreaseButton;

    [SerializeField]
    GameObject highlight;

    [SerializeField]
    Image weaponSprite;

    /*
    public int Level
    {
        get { return tempLevel; }
    }
    */
    int tempLevel = 1;

    int cost = 150;

    public void FillData(Weapon _weapon, WeaponUpgradeManager manager)
    {
        weapon = _weapon;
        managerRef = manager;
        nameText.text = weapon.name;
        weaponSprite.sprite = weapon.sprite;
        costText.text = cost.ToString();
        tempLevel = GameManager.instance.weaponLevels[weapon];

        UpdateData();
    }

    public void UpdateData()
    {
        levelText.text = tempLevel.ToString();//GameManager.instance.weaponLevels[weapon].ToString();

        increaseButton.interactable = tempLevel < weapon.maxLevel && GameManager.instance.Currency >= cost;

        decreaseButton.interactable = tempLevel > 1;

        bool valuesChanged = GameManager.instance.weaponLevels[weapon] != tempLevel;

        highlight.SetActive(valuesChanged);
        //managerRef.ShowSaveButton(valuesChanged);

    }

    public void UndoLevelChange()
    {
        Refund(cost * (tempLevel - GameManager.instance.weaponLevels[weapon]));
        tempLevel = GameManager.instance.weaponLevels[weapon];
    }

    public void ResetLevel()
    {
        Refund(cost * (tempLevel - GameManager.instance.weaponLevels[weapon]));
        tempLevel = 1;
    }

    void Refund(int amount)
    {
        GameManager.instance.AddCurrency(amount);
    }

    public int GetWeaponLevel()
    {
        return tempLevel;
    }

    /* weapon upgrades are not planned to have the hoverbox to explain since they are clearer
    public void OnPointerEnter(PointerEventData data)
    {
        PersistentUpgradeManager.SetHoverBox(GameManager.StatFluff.Get(statData.Type).description);
    }

    public void OnPointerExit(PointerEventData data)
    {
        PersistentUpgradeManager.HideHoverBox();
    }
    */


    public void DecreaseLevel()
    {
        //statData.DecreaseLevel();
        //decrease weapon level here
        tempLevel--;
        GameManager.instance.AddCurrency(cost);

        WeaponUpgradeManager.UpdateWidgets();
    }

    public void IncreaseLevel()
    {
        //statData.IncreaseLevel();
        //increase weapon level here
        tempLevel++;
        GameManager.instance.AddCurrency(-cost);

        WeaponUpgradeManager.UpdateWidgets();
    }

    public void Highlight(bool flag)
    {
        highlight.SetActive(flag);
    }

}
