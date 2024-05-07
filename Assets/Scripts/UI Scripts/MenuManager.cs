using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField]
    public enum State { MainMenu, PauseMenu, NoMenu, DeathMenu, StatUpgradeMenu, LoadoutMenu, WeaponUpgradeMenu };

    public MenuObject[] menus;
    
    Dictionary<State, GameObject> menuDict;

    GameObject activeMenu;

    [System.Serializable]
    public class MenuObject
    {
        public State state;
        public GameObject menu;
    }

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


        menuDict = new Dictionary<State, GameObject> ();
        foreach(MenuObject mobj in menus)
        {
            menuDict.Add(mobj.state, mobj.menu);
        }
    }

    public void GoToMenu(MenuReference menuRef)//called from buttons
    {
        GoToMenu(menuRef.target);
        GameManager.instance.Pause(menuRef.pauseOnTarget);
    }

    public void GoToMenu(State target)//called from scripts
    {
        if (!gameObject.activeInHierarchy)
        {
            MenusActive(true);
        }
        if(activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = menuDict[target];
        if (activeMenu == null)
        {
            MenusActive(false);
        }
        else
        {
            activeMenu.SetActive(true);
        }

    }

    public void MenusActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void DisableAllMenus()
    {
        foreach(KeyValuePair<State, GameObject> pair in menuDict)
        {
            if(pair.Value != null)
            {
                pair.Value.SetActive(false);
            }
        }
    }

    
}
