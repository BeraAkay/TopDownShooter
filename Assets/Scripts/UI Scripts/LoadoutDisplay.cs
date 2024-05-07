using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadoutDisplay : MonoBehaviour
{
    
    public Weapon main, secondary;

    [SerializeField]
    TextMeshProUGUI mainLevelText, mainDMGText, secondaryLevelText, secondaryDMGText;

    public void SetLoadout(Weapon _main, Weapon _secondary)
    {
        main = _main;
        secondary = _secondary;

        UpdateUI();
    }

    public void UpdateUI()
    {
        mainLevelText.text = main.level.ToString();
        mainDMGText.text = main.damage.ToString();

        secondaryLevelText.text = secondary.level.ToString();
        secondaryDMGText.text = secondary.damage.ToString();
    }
}
