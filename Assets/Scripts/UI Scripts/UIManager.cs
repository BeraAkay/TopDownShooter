using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[DefaultExecutionOrder(3)]
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    Image expBar;

    [SerializeField]
    TextMeshProUGUI expText;
    [SerializeField]
    Text pLevel;

    float optionSpaceWidth;//this should be the value of the width of the parent of option cards

    [SerializeField]
    GameObject optionSelectScreen;

    [SerializeField]
    RectTransform optionSpace;
    OptionScript[] optionCards = new OptionScript[5];

    [SerializeField]
    int debugInt;

    [SerializeField]
    StatSheetDisplay statSheetDisplay;

    [SerializeField]
    LoadoutDisplay loadoutDisplay;

    [SerializeField]
    Canvas menuCanvas;

    [HideInInspector]
    public MenuManager menuManager;

    [SerializeField]
    TextMeshProUGUI levelTimerDisplay;

    [SerializeField]
    TextMeshProUGUI[] currencyValueTexts;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        optionSpaceWidth = optionSpace.sizeDelta.x;
        optionCards = optionSpace.GetComponentsInChildren<OptionScript>();

        menuManager = menuCanvas.GetComponent<MenuManager>();

    }

    private void Start()
    {
        
        DisableOptions();


        menuManager.gameObject.SetActive(true);
        menuManager.DisableAllMenus();
        menuManager.GoToMenu(MenuManager.State.MainMenu);

        GameManager.instance.Pause(true);
        
    }

    public void CreateLoadoutDisplay()
    {
        loadoutDisplay.SetLoadout(PlayerInteractions.instance.mainWeapon, PlayerInteractions.instance.secondaryWeapon);
    }

    public void CreateStatSheet()
    {
        statSheetDisplay.CreateSheet();

        statSheetDisplay.UpdateStats();
    }

    public void UpdateStatSheet()
    {
        statSheetDisplay.UpdateStats();
    }

    public void SetExpBar(float barFill)
    {
        expBar.fillAmount = barFill;
    }

    public void SetExpText(string text)
    {
        expText.text = text;
    }

    public void SetLevelText(int level)
    {
        pLevel.text = level.ToString();
    }

    public void SetLevelText(string level)
    {
        pLevel.text = level;
    }


    public void DisplayOptions(IUpgradable[] statUpgrades)
    {
        optionSelectScreen.SetActive(true);

        int optionCount = statUpgrades.Length;

        optionCount = Mathf.Max(optionCount, 2);//make this into a variable thats read from a loot values script
        optionCount = Mathf.Min(optionCount, optionCards.Length);

        SetUpOptionCards(statUpgrades);
        if(optionCount < optionCards.Length)
        {
            DisableEmptyCards(optionCount);
        }

        GameManager.instance.Pause(true);
    }

    public void DisableOptions()
    {
        optionSelectScreen.SetActive(false);
        GameManager.instance.Pause(false);
    }

    

    void SetUpOptionCards(IUpgradable[] upgradeData)
    {
        int cardCount = upgradeData.Length;

        float cardWidth = optionCards[0].GetComponent<RectTransform>().sizeDelta.x;
        float paddingSpace = optionSpaceWidth - (cardCount * cardWidth);

        float padding = paddingSpace / (cardCount + 1);

        float baseOffset = -optionSpaceWidth / 2;

        Vector3 cardPosition = Vector3.zero;
        cardPosition.x += padding + baseOffset + cardWidth / 2;

        optionCards[0].GetComponent<RectTransform>().localPosition = cardPosition;

        optionCards[0].DisplayUpgrade(upgradeData[0], 0);
        optionCards[0].gameObject.SetActive(true);

        for (int i = 1; i < cardCount; i++)
        {
            cardPosition.x += (cardWidth + padding);
            optionCards[i].GetComponent<RectTransform>().localPosition = cardPosition;
            optionCards[i].DisplayUpgrade(upgradeData[i], 0);
            optionCards[i].gameObject.SetActive(true);
        }
    }

    void DisableEmptyCards(int optionCount)
    {
        for (int i = optionCount; i < optionCards.Length; i++)
        {
            optionCards[i].gameObject.SetActive(false);
        }
    }

    public void DisableCards()
    {
        DisableEmptyCards(0);
    }

    public void SetLevelTimer(float time)
    {
        levelTimerDisplay.text = time.ToString();
    }

    public void UpdateCurrencyTexts()
    {
        foreach(TextMeshProUGUI tmpro in currencyValueTexts)
        {
            tmpro.text = GameManager.instance.Currency.ToString();
        }
    }

}
