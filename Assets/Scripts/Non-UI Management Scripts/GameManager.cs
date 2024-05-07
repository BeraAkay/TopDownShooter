using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]//order for managers [0: gamemanager, 1: ]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    Stats playerStatRef;
    public static Stats playerStats;

    [SerializeField]
    Stats persistentStatRef;
    public static Stats persistentStats;

    [SerializeField]
    StatFluff statFluffRef;
    public static StatFluff StatFluff;

    public static ObjectPooler Pooler;

    public Weapon[] baseWeapons;
    public float pelletLifetime;

    public PlayerController playerController;
    public bool paused;

    public Dictionary<Stats.Types, int> persistentLevels;
    string persistenceID = "PersistentStats";

    public Dictionary<Weapon, int> weaponLevels;
    string weaponLevelsID = "WeaponLevels";



    public int Currency
    {
        get
        {
            return currency;
        }
    }
    string currencyID = "Currency";
    int currency;


    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        playerStats = playerStatRef;
        persistentStats = persistentStatRef;
        StatFluff = statFluffRef;

        if(!TryGetComponent(out Pooler))
        {
            Pooler = gameObject.AddComponent<ObjectPooler>();
        }

        playerController = FindObjectOfType<PlayerController>();

        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        persistentStats.CreateSheet();
        TryLoadPersistent();

        //PlayerPrefs.DeleteKey(weaponLevelsID);
        TryLoadWeaponLevels();

        LoadCurrency();
        if(currency == 0)//lil cheat to help try things out
        {
            currency = 20000;
            SaveCurrency();
            LoadCurrency();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        //set everything etc etc etc
        playerController.gameObject.transform.position = Vector3.zero;

        //PlayerInteractions.instance.Initialize();
        LevelManager.instance.Initialize();
        PlayerLevels.instance.Initialize();
        UIManager.instance.CreateStatSheet();
        UIManager.instance.CreateLoadoutDisplay();

        Pause(false);
    }

    public void ResumeGame()
    {
        Pause(false);
    }

    public void PlayerDeath()
    {
        CollectCurrency();
        Pause(true);
        MenuManager.instance.GoToMenu(MenuManager.State.DeathMenu);
    }

    public void CollectCurrency()
    {
        AddCurrency(LevelManager.instance.accumulatedCurrency);
        SaveCurrency();
    }

    public void Pause(bool pause)
    {
        paused = pause;
        Time.timeScale = pause ? 0 : 1;

        //#if !UNITY_WEBGL
        //Cursor.visible = pause;
        //Cursor.lockState = pause ? CursorLockMode.Confined : CursorLockMode.None;
        //#endif

        playerController.enabled = !pause;
    }

    //these 4 should probably be in PersistentUpgradeManager, but ill leave it for now since its a bit of a waste of time right now.
    public void WipePersistentLevels()
    {
        Persistence<Stats.Types, int>.Wipe(persistenceID);
        TryLoadPersistent();
        PersistentUpgradeManager.ResetWidgets();
    }

    public void WipeWeaponLevels()
    {
        //todo
        Persistence<Weapon, int>.Wipe(weaponLevelsID);
        TryLoadWeaponLevels();
        WeaponUpgradeManager.ResetWidgets();
    }

    public void SavePersistentLevels()
    {
        Persistence<Stats.Types, int>.Save(persistentLevels, persistenceID);
        PersistentUpgradeManager.UpdateWidgets();
    }

    public void SaveWeaponLevels()
    {
        Persistence<Weapon, int>.Save(weaponLevels, weaponLevelsID);
    }

    bool TryLoadPersistent()
    {
        bool flag = true;
        persistentLevels = Persistence<Stats.Types, int>.Load(persistenceID);
        if (persistentLevels == null)
        {
            flag = false;
            persistentLevels = new Dictionary<Stats.Types, int>();
            foreach (KeyValuePair<Stats.Types, Stats.Stat> pair in persistentStats.sheetDict)
            {
                persistentLevels[pair.Key] = pair.Value.minLevel;
            }

        }
        else
        {
            foreach (KeyValuePair<Stats.Types, Stats.Stat> pair in persistentStats.sheetDict)
            {
                if (!persistentLevels.ContainsKey(pair.Key))
                {
                    persistentLevels[pair.Key] = pair.Value.minLevel;
                    Debug.Log(string.Format("Added {0} as key.", pair.Key));
                    flag = false;
                }
            }
        }
        return flag;
    }

    bool TryLoadWeaponLevels()
    {
        bool flag = true;
        weaponLevels = Persistence<Weapon, int>.Load(weaponLevelsID);//put a measure here to delete the key values if the returns give errors and just treat as null
        if(weaponLevels == null)
        {
            flag = false;
            weaponLevels = new Dictionary<Weapon, int>();
            foreach(Weapon weapon in baseWeapons)
            {
                weaponLevels[weapon] = 1;
            }
        }
        else
        {
            foreach(Weapon weapon in baseWeapons)
            {
                if (!weaponLevels.ContainsKey(weapon) || weaponLevels[weapon] == 0)
                {
                    weaponLevels[weapon] = 1;
                }
                else if (weaponLevels[weapon] > weapon.maxLevel)
                {
                    weaponLevels[weapon] = weapon.maxLevel;
                }
            }
        }
        return flag;
    }


    public void ApplyPersistentStatUpgrades()
    {
        Dictionary<Stats.Types, int> newLevels = PersistentUpgradeManager.GetLevels();

        foreach(Stats.Types type in newLevels.Keys)
        {
            if(persistentLevels.ContainsKey(type) && newLevels[type] != persistentLevels[type])
            {
                persistentLevels[type] = newLevels[type];
            }
        }
        PersistentUpgradeManager.UpdateWidgets();
        SavePersistentLevels();
    }

    public void ApplyWeaponUpgrades()
    {
        //todo
        Dictionary<Weapon, int> newLevels = WeaponUpgradeManager.GetLevels();

        foreach(Weapon weapon in newLevels.Keys)
        {
            if(weaponLevels.ContainsKey(weapon) && newLevels[weapon] != weaponLevels[weapon])
            {
                weaponLevels[weapon] = newLevels[weapon];
            }
        }
        WeaponUpgradeManager.UpdateWidgets();
        SaveWeaponLevels();
    }


    public void LevelUpWeapon(Weapon weapon)
    {
        weapon.level = Mathf.Min(weapon.level + 1, weapon.maxLevel);
        weaponLevels[weapon] = weapon.level;
    }

    public void LevelUpWeapons()
    {
    }


    public void AddCurrency(int value)
    {
        currency += value;
        UIManager.instance.UpdateCurrencyTexts();
    }

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt(currencyID, Currency);
    }

    void LoadCurrency()
    {
        currency = PlayerPrefs.GetInt(currencyID);
        StartCoroutine(WaitOnCurrencyTextUpdate());
    }

    IEnumerator WaitOnCurrencyTextUpdate()
    {
        yield return new WaitUntil(() => UIManager.instance != null);

        UIManager.instance.UpdateCurrencyTexts();
    }

    private void OnApplicationPause(bool pause)
    {
        //Pause(pause);
    }

    private void OnApplicationFocus(bool focus)
    {
        /*depending on state(if in play mode), the game should pause
        if (!focus)
        {
            Pause(true);
        }
        */

        //Cursor.visible = !focus;
        /*if (focus)
        {
            Cursor.lockState = CursorLockMode.Confined;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //Debug.Log("Application lost focus");
        }*/
    }

    private void OnApplicationQuit()
    {
        
    }

}
