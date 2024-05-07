using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class StatDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI statName, statLevel, statValue;

    Stats.Types reference;

    /*
    public enum StatClass { RegularStat, WeaponStat };
    public StatClass statClass;
    */

    public void SetDisplay(Stats.Types type)
    {
        reference = type;
        statName.text = GameManager.StatFluff.Get(type).abbreviation;

        UpdateDisplay();
    }

    public void UpdateDisplay()//this is prob better off being tied to an event that goes off inside the iupgradable's upgrade or playerlevels' levelup call
    {
        float value = PlayerInteractions.instance.stats[reference];

        statLevel.text = PlayerInteractions.instance.statLevels[reference].ToString();
        if(statLevel.text == "0")
        {
            statLevel.text = "";
        }
        statValue.text = value.ToString();
    }
}
