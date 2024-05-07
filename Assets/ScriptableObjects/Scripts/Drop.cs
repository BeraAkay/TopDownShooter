using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Drop : ScriptableObject
{
    public enum DropType
    {
        Exp, Loot
    }

    public DropType type;

    //public int baseValue;

    public Sprite sprite;

    public void DropFunction(int value)
    {
        switch (type)
        {
            case DropType.Exp:
                PlayerLevels.instance.AddExp(value);
                break;
            case DropType.Loot:
                LevelManager.instance.LootRoll(value);
                break;
            default:
                break;
        }
    }
}
