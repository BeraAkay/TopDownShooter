using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PersistentUpgradeManager;

[DefaultExecutionOrder(6)]
public class WeaponUpgradeManager : MonoBehaviour
{
    [SerializeField]
    GridPlanner widgetPlanner;

    [SerializeField]
    Button saveButton, wipeButton;

    static Button wiper, saver;

    //[SerializeField]
    //GameObject upgradeWidgetPrefab;

    static Dictionary<Weapon, WeaponUpgradeDisplay> upgradeWidgets;

    private void Awake()
    {
        wiper = wipeButton;
        saver = saveButton;
        CreateUpgradeWidgets();
    }

    void CreateUpgradeWidgets()
    {
        upgradeWidgets = new Dictionary<Weapon, WeaponUpgradeDisplay>();

        List<GameObject> emptyWidgets = widgetPlanner.gridObjects;
        int widgetIndex = 0;

        bool wipeButtonState = false;
        //GameManager.persistentStats.sheetDict
        foreach (Weapon weapon in GameManager.instance.baseWeapons)
        {
            if (widgetIndex < emptyWidgets.Count)
            {
                upgradeWidgets[weapon] = emptyWidgets[widgetIndex++].GetComponent<WeaponUpgradeDisplay>();
                upgradeWidgets[weapon].FillData(weapon, this);
                wipeButtonState |= GameManager.instance.weaponLevels[weapon] != 1;
            }
        }
        ShowSaveButton(false);
        ShowWipeButton(wipeButtonState);
    }

    static void UpdateWidget(Weapon weapon)
    {
        upgradeWidgets[weapon].UpdateData();
    }

    public static void UpdateWidgets()
    {
        bool wipeButtonState = false;
        bool saveButtonState = false;
        foreach (Weapon weapon in GameManager.instance.baseWeapons)
        {
            UpdateWidget(weapon);

            wipeButtonState |= GameManager.instance.weaponLevels[weapon] != 1;
            wipeButtonState |= upgradeWidgets[weapon].GetWeaponLevel() != 1;

            saveButtonState |= upgradeWidgets[weapon].GetWeaponLevel() != GameManager.instance.weaponLevels[weapon];

        }
        ShowSaveButton(saveButtonState);
        ShowWipeButton(wipeButtonState);
    }

    public static void UndoPendingUpgrades()//call this on upgrade menu leaving
    {
        bool wipeButtonState = false;
        foreach (Weapon weapon in GameManager.instance.baseWeapons)
        {
            upgradeWidgets[weapon].UndoLevelChange();
            upgradeWidgets[weapon].UpdateData();
            wipeButtonState |= GameManager.instance.weaponLevels[weapon] != 1;
        }
        ShowSaveButton(false);
        ShowWipeButton(wipeButtonState);
    }

    public static void ResetWidgets()
    {
        foreach (Weapon weapon in GameManager.instance.baseWeapons)
        {
            upgradeWidgets[weapon].ResetLevel();
            upgradeWidgets[weapon].UpdateData();
        }
        ShowSaveButton(false);
        ShowWipeButton(false);
    }

    public static Dictionary<Weapon, int> GetLevels()
    {
        Dictionary<Weapon, int> levelDict = new Dictionary<Weapon, int>();

        foreach (Weapon weapon in upgradeWidgets.Keys)
        {
            levelDict[weapon] = upgradeWidgets[weapon].GetWeaponLevel();
        }

        return levelDict;
    }

    public static void ShowSaveButton(bool flag)
    {
        saver.interactable = flag;
    }

    public static void ShowWipeButton(bool flag)
    {
        wiper.interactable = flag;
    }

}
