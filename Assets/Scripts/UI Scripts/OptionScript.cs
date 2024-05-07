using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI upgradeName;
    [SerializeField]
    TextMeshProUGUI upgradeDescription;
    [SerializeField]
    TextMeshProUGUI upgradeLevel;
    [SerializeField]
    Image upgradeSprite;

    public enum upgradeType { WeaponUpgrade, StatUpgrade };

    public upgradeType type;

    IUpgradable upgradeData;


    void DisplayStatUpgrade(StatUpgrade upgradeData_, int level)
    {
        upgradeLevel.text = level.ToString();

        StatFluff.Fluff fluff = upgradeData_.GetFluff();

        upgradeName.text = fluff.name;

        upgradeDescription.text = fluff.description;

        upgradeSprite.sprite = fluff.sprite;

        upgradeData = upgradeData_;
    }

    void DisplayWeaponUpgrade(Weapon weaponData, int level)
    {
        upgradeLevel.text = level.ToString();

        upgradeName.text = weaponData.name;

        upgradeDescription.text = weaponData.description;

        upgradeSprite.sprite = weaponData.sprite;

        upgradeData = weaponData;
    }

    public void DisplayUpgrade(IUpgradable upgrade, int level)
    {
        if (upgrade.upgradeClass == 0)
        {
            DisplayStatUpgrade((StatUpgrade)upgrade, level);
        }
        else if (upgrade.upgradeClass == 1)
        {
            DisplayWeaponUpgrade((Weapon)upgrade, level);
        }

    }


    public void PickOption()
    {
        upgradeData.Upgrade();
        UIManager.instance.DisableOptions();

        UIManager.instance.UpdateStatSheet();
    }

}
