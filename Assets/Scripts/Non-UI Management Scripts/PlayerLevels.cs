using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class PlayerLevels : MonoBehaviour
{
    public static PlayerLevels instance;

    PlayerController player;

    [SerializeField]
    float baseExp = 20;
    [SerializeField]
    float expScalingPerLevel = 10;

    public int playerLevel;
    public float playerExp;

    public float currentExpNeeded;

    PlayerInteractions playerInteractions;

    [SerializeField]
    int debugInt;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        
    }

    public void Initialize()
    {
        playerLevel = 0;
        playerExp = 0;
        currentExpNeeded = ExpNeededForLevel(playerLevel);

        string expString = string.Format("{0} / {1}", playerExp, currentExpNeeded);
        UIManager.instance.SetExpText(expString);

        UIManager.instance.SetExpBar(playerExp / currentExpNeeded);


        playerInteractions = LevelManager.instance.playerTransform.GetComponent<PlayerInteractions>();
        player = playerInteractions.GetComponent<PlayerController>();
    }

    float ExpNeededForLevel(int level)
    {
        return baseExp + (level * expScalingPerLevel);//add to this formula for calculating necessary exp per level
    }

    public void AddExp(float amount)
    {
        playerExp += amount * playerInteractions.stats[Stats.Types.XPMultiplier];

        if(playerExp >= currentExpNeeded)
        {
            LevelUp();
        }

        string expString = string.Format("{0} / {1}", playerExp, currentExpNeeded) ;
        UIManager.instance.SetExpText(expString);

        UIManager.instance.SetExpBar(playerExp / currentExpNeeded);
    }

    void LevelUp()
    {
        playerLevel += 1;
        playerExp = 0;
        currentExpNeeded = ExpNeededForLevel(playerLevel);

        string expString = string.Format("{0} / {1}", playerExp, currentExpNeeded);
        UIManager.instance.SetExpText(expString);

        UIManager.instance.SetExpBar(playerExp / currentExpNeeded);

        UIManager.instance.DisplayOptions(GetUpgradeOptions(debugInt));
        //call the levelup ui and let it generate level up options
    }

    IUpgradable[] PossibleUpgrades(bool shuffle)//returns possible upgrades in random/or not order
    {
        List<IUpgradable> upgrades = new List<IUpgradable>();

        //for stats
        foreach(Stats.Stat stat in GameManager.playerStats.sheetDict.Values)
        {
            if(stat.type == Stats.Types.CurrentHealth)
            {
                continue;
            }

            if (stat.Upgradable(PlayerInteractions.instance.statLevels[stat.type]))
            {
                upgrades.Add(new StatUpgrade(stat.type));
            }
        }
        /*
        //for weapons, now obsolete
        for (int i = 0; i < playerInteractions.playerWeapons.Length; i++)
        {
            Weapon weapon = playerInteractions.playerWeapons[i];
            if (weapon.level != weapon.maxLevel)
            {
                upgrades.Add(weapon);
            }
        }
        */


        if (shuffle)
        {
            return Shuffle(upgrades.ToArray());
        }
        return upgrades.ToArray();

    }

    public IUpgradable[] GetUpgradeOptions(int optionAmount)
    {
        IUpgradable[] possibleUpgrades = PossibleUpgrades(true);

        IUpgradable[] upgradeOptions = new IUpgradable[optionAmount];

        /*
        for(int i = 0; i < optionAmount; i++)//modified shuffle to pick random upgrades
        {
            int random = Random.Range(i, upgradableStats.Length);

            UpgradeData temp = upgradableStats[i];
            upgradableStats[i] = upgradableStats[random];
            upgradableStats[random] = temp;

            upgradeOptions[i] = upgradableStats[i];
        }
        */

        for (int i = 0; i < optionAmount; i++)//pick first n out of the shuffled possible upgrades
        {
            upgradeOptions[i] = possibleUpgrades[i];
        }
        return upgradeOptions;
    }
    
    
    IUpgradable[] Shuffle(IUpgradable[] upgrades)//sattolo/knuth/fisheryates
    {
        for(int i = upgrades.Length - 1; i > 0; i--)
        {
            int random = Random.Range(0, i+1);

            (upgrades[random], upgrades[i]) = (upgrades[i], upgrades[random]);
        }

        return upgrades;
    }

}
