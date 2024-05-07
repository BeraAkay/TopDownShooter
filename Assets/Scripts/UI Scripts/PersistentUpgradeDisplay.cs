using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PersistentUpgradeDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    PersistentUpgradeManager.PersistentUpgrade statData;
    PersistentUpgradeManager managerRef;

    [SerializeField]
    TextMeshProUGUI nameText, levelText, costText;

    [SerializeField]
    Button increaseButton, decreaseButton;

    [SerializeField]
    GameObject highlight;


    public void FillData(PersistentUpgradeManager.PersistentUpgrade data, PersistentUpgradeManager manager)
    {
        statData = data;
        managerRef = manager;

        nameText.text = GameManager.StatFluff.Get(statData.Type).name;
        costText.text = statData.cost.ToString();
        UpdateData();
    }

    public void UpdateData()
    {
        levelText.text = statData.Level.ToString();

        increaseButton.interactable = statData.Upgradable();

        decreaseButton.interactable = statData.Level > 0;

        bool valuesChanged = GameManager.instance.persistentLevels[statData.Type] != statData.Level;

        highlight.SetActive(valuesChanged);
        //managerRef.ShowSaveButton(valuesChanged);

    }

    public void UndoLevelChange()
    {
        statData.RevertUpgrade();
    }

    public void ResetLevel()
    {
        statData.Reset();
    }

    public int GetStatLevel()
    {
        return statData.Level;
    }


    public void OnPointerEnter(PointerEventData data)
    {
        managerRef.SetHoverBox(GameManager.StatFluff.Get(statData.Type).description);
    }

    public void OnPointerExit(PointerEventData data)
    {
        managerRef.HideHoverBox();
    }


    public void DecreaseLevel()
    {
        statData.DecreaseLevel();
        
        PersistentUpgradeManager.UpdateWidgets();
    }

    public void IncreaseLevel()
    {
        statData.IncreaseLevel();
        PersistentUpgradeManager.UpdateWidgets();
    }

    public void Highlight(bool flag)
    {
        highlight.SetActive(flag);
    }
}
