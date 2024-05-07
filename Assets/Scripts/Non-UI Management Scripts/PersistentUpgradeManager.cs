using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(5)]
public class PersistentUpgradeManager : MonoBehaviour
{
    [SerializeField]
    DescriptionBox descriptionBox;

    Stats persistentUpgradeSheet;

    [SerializeField]
    GridPlanner widgetPlanner;

    [SerializeField]
    GameObject upgradeWidgetPrefab;

    Coroutine hoverDelayCoroutine;

    [SerializeField]
    Button saveButton, wipeButton;

    static Button wiper, saver;

    static Dictionary<Stats.Types, PersistentUpgradeDisplay> upgradeWidgets;

    private void Awake()
    {
        wiper = wipeButton;
        saver = saveButton; 
        CreateUpgradeWidgets();
    }


    void CreateUpgradeWidgets()
    {
        upgradeWidgets = new Dictionary<Stats.Types, PersistentUpgradeDisplay>();

        List<GameObject> emptyWidgets = widgetPlanner.gridObjects;
        int widgetIndex = 0;
        bool wipeButtonState = false;
        //GameManager.persistentStats.sheetDict
        foreach(KeyValuePair<Stats.Types, Stats.Stat> pair in GameManager.persistentStats.sheetDict)
        {
            if(widgetIndex < emptyWidgets.Count)
            {
                upgradeWidgets[pair.Key] = emptyWidgets[widgetIndex++].GetComponent<PersistentUpgradeDisplay>();
                upgradeWidgets[pair.Key].FillData(new PersistentUpgrade(pair.Key, GameManager.instance.persistentLevels[pair.Key], pair.Value.maxLevel), this);
                wipeButtonState |= GameManager.instance.persistentLevels[pair.Key] != pair.Value.minLevel;
            }
        }
        ShowSaveButton(false);
        ShowWipeButton(wipeButtonState);
    }

    static void UpdateWidget(Stats.Types type)
    {
        upgradeWidgets[type].UpdateData();
    }

    public static void UpdateWidgets()
    {
        bool wipeButtonState = false;
        bool saveButtonState = false;
        foreach (Stats.Types type in upgradeWidgets.Keys)
        {
            UpdateWidget(type);

            wipeButtonState |= GameManager.instance.persistentLevels[type] != GameManager.persistentStats.sheetDict[type].minLevel;
            wipeButtonState |= upgradeWidgets[type].GetStatLevel() != GameManager.persistentStats.sheetDict[type].minLevel;

            saveButtonState |= upgradeWidgets[type].GetStatLevel() != GameManager.instance.persistentLevels[type];
        }
        ShowSaveButton(saveButtonState);
        ShowWipeButton(wipeButtonState);
    }

    public static void UndoPendingUpggrades()//call this on upgrade menu leaving
    {
        bool wipeButtonState = false;
        foreach (KeyValuePair<Stats.Types, Stats.Stat> pair in GameManager.persistentStats.sheetDict)
        {
            upgradeWidgets[pair.Key].UndoLevelChange();
            upgradeWidgets[pair.Key].UpdateData();
            wipeButtonState |= GameManager.instance.persistentLevels[pair.Key] != GameManager.persistentStats.sheetDict[pair.Key].minLevel;
        }
        ShowSaveButton(false);
        ShowWipeButton(wipeButtonState);
    }

    public static void ResetWidgets()
    {
        foreach (KeyValuePair<Stats.Types, Stats.Stat> pair in GameManager.persistentStats.sheetDict)
        {
            upgradeWidgets[pair.Key].ResetLevel();
            upgradeWidgets[pair.Key].UpdateData();
        }
        ShowSaveButton(false);
        ShowWipeButton(false);
    }

    public static Dictionary<Stats.Types, int> GetLevels()
    {
        Dictionary<Stats.Types, int> levelDict = new Dictionary<Stats.Types, int>();

        foreach(Stats.Types type in upgradeWidgets.Keys)
        {
            levelDict[type] = upgradeWidgets[type].GetStatLevel();
        }

        return levelDict;
    }


    public void SetHoverBox(string text)
    {
        if (hoverDelayCoroutine != null)
        {
            StopCoroutine(hoverDelayCoroutine);
        }
        descriptionBox.SetText(text);
    }

    public void HideHoverBox()
    {
        if (hoverDelayCoroutine != null)
        {
            StopCoroutine(hoverDelayCoroutine);
        }
        hoverDelayCoroutine = StartCoroutine(DelayedHideHoverBox());
    }

    public static void ShowSaveButton(bool flag)
    {
        saver.interactable = flag;
    }

    public static void ShowWipeButton(bool flag)
    {
        wiper.interactable = flag;
    }

    IEnumerator DelayedHideHoverBox()
    {
        yield return new WaitForSecondsRealtime(descriptionBox.hoverBoxHideDelay);//needs real time since scaled time is paused outside of gameplay
        descriptionBox.SetText("");
    }

    public class PersistentUpgrade
    {
        Stats.Types type;
        int level;
        int maxLevel;
        public int cost = 50;

        public Stats.Types Type
        {
            get
            {
                return type;
            }
        }

        public float Value
        {
            get
            {
                return GameManager.persistentStats.sheetDict[type].ValueAtLevel(level);
            }
        }

        public int Level
        {
            get
            {
                return level;
            }
        }

        public void IncreaseLevel()
        {
            if (Upgradable())
            {
                level++;
                GameManager.instance.AddCurrency(-cost);
            }
        }

        public void DecreaseLevel()
        {
            if (level > 0)
            {
                level--;
                GameManager.instance.AddCurrency(cost);
            }
        }

        public bool Upgradable()
        {
            return level < maxLevel && GameManager.instance.Currency >= cost;
        }



        public PersistentUpgrade(Stats.Types type_, int level_, int maxLevel_)
        {
            type = type_;
            level = level_;
            maxLevel = maxLevel_;
        }

        public void RevertUpgrade()
        {
            Refund(cost * (level - GameManager.instance.persistentLevels[type]));
            level = GameManager.instance.persistentLevels[type];
        }
        
        public void Reset()
        {
            Refund(cost * (level - GameManager.instance.persistentLevels[type]));
            level = 0;
        }

        void Refund(int amount)
        {
            GameManager.instance.AddCurrency(amount);
        }
    }

    [Serializable]
    public class DescriptionBox
    {
        [SerializeField]
        RectTransform mainTransform;
        [SerializeField]
        TextMeshProUGUI descriptionText;

        [SerializeField]
        public float hoverBoxHideDelay;

        public void SetText(string text)
        {
            if(text == "")
            {
                Hide(true);
            }
            else
            {
                descriptionText.text = text;
                Hide(false);
            }
        }

        public void Hide(bool flag)
        {
            mainTransform.gameObject.SetActive(!flag);
        }
    }
}
