using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Stats;

[CreateAssetMenu, Serializable]
public class Stats : ScriptableObject
{
    //if you add anything new here as a type, make sure to also add that to stat fluf IN ORDER since stat fluff uses integer casting on these types and stores/accesses them that way
    [Serializable]
    public enum Types { MaxHealth, CurrentHealth, Mitigation, DamageMultiplier, WeaponRecovery, MovementSpeed, Regeneration, CriticalChance, PickupRange, XPMultiplier, ExtraLives, Curse, MeleeDamage }
    //12 types of stats
    [SerializeField]
    Stat[] sheet;

    public Dictionary<Types, Stat> sheetDict;

    [HideInInspector]
    public static Types[] persistentOnly = { Types.PickupRange, Types.XPMultiplier, Types.ExtraLives, Types.Curse };

    [HideInInspector]
    public static Types[] dropOnly = { Types.CriticalChance };//might make this hybrid due to simplicity

    [HideInInspector]
    public static Types[] hybridAcquisition = { Types.MaxHealth, Types.Mitigation, Types.DamageMultiplier, Types.WeaponRecovery, Types.MovementSpeed, Types.Regeneration };

    /* UNUSED
    [HideInInspector]
    public static Types[] damageable = { Types.MaxHealth, Types.CurrentHealth, Types.Mitigation, Types.MovementSpeed };

    [HideInInspector]
    public static Types[] attacker = { Types.DamageMultiplier, Types.WeaponRecovery };

    [HideInInspector]
    public static Types[] playerSpecific = { Types.Regeneration, Types.CriticalChance, Types.PickupRange, Types.XPMultiplier, Types.ExtraLives, Types.Curse };

    [HideInInspector]
    public static Types[] universal = { };

    [HideInInspector]
    public static Types[] enemySpecific = { Types.MeleeDamage };
    */
    int LookUp(Types type)
    {
        for(int index = 0; index < sheet.Length; index++)
        {
            if(type == sheet[index].type)
            {
                return index;
            }
        }
        return -1;
    }


    public void CreateSheet()
    {
        sheetDict = new Dictionary<Types, Stat>();
        foreach(Stat stat in sheet)
        {
            if(!sheetDict.ContainsKey(stat.type))
                sheetDict.Add(stat.type, stat);
        }

    }


    [System.Serializable]
    public class Stat
    {
        public Types type;

        public int minLevel, maxLevel;//i should remove minlevel if all stats have minlevel as 0
        public float minValue, maxValue;

        float perLevel;

        public bool Upgradable(int statLevel)
        {
            return statLevel < maxLevel && (dropOnly.Contains(type) || hybridAcquisition.Contains(type));//only if the stat is of drop or hybrid AND lower than max level

        }

        public float ValueAtLevel(int level)
        {

            return Mathf.Min(maxValue, minValue + (UpgradePerLevel() * level));//change this or this would fuck up for weapon recovery OR just change the weapon recovery thing in the shooting calculation and make it reverse

        }

        public float UpgradePerLevel()
        {
            if(perLevel == 0)
            {
                perLevel = maxValue - minValue;
                perLevel /= maxLevel - minLevel;
            }
            if(minValue == maxValue || minLevel == maxLevel)
            {
                return 0;
            }
            return perLevel;
        }
    }

}
