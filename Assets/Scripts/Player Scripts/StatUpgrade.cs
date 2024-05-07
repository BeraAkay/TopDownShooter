using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgrade : IUpgradable
{
    public Stats.Types upgradeType;

    public int upgradeClass
    {
        get
        {
            return 0;
        }
    }

    public StatFluff.Fluff GetFluff()
    {
        return GameManager.StatFluff.Get(upgradeType);
    }

    public StatUpgrade(Stats.Types type)
    {
        upgradeType = type;
    }

    public void Upgrade()
    {
        PlayerInteractions playerRef = LevelManager.instance.playerTransform.GetComponent<PlayerInteractions>();
        Stats statRef = GameManager.playerStats;

        playerRef.statLevels[upgradeType] += 1;
        playerRef.stats[upgradeType] = statRef.sheetDict[upgradeType].ValueAtLevel(playerRef.statLevels[upgradeType]);//+add the persistent stat mods here

        if(upgradeType == Stats.Types.MaxHealth)
        {
            playerRef.Heal(statRef.sheetDict[upgradeType].UpgradePerLevel());
        }
    }

}
