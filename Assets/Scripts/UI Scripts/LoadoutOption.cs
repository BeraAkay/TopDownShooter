using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutOption : MonoBehaviour
{
    LoadoutPicker loadoutPicker;

    
    public Weapon weapon;

    [SerializeField]
    TextMeshProUGUI optionName, optionDescription, optionLevelValue;

    [SerializeField]
    Image optionImage;

    // Start is called before the first frame update
    void Start()
    {
        loadoutPicker = GetComponentInParent<LoadoutPicker>();

        GetComponent<Button>().onClick.AddListener(OnClickTask);

        UpdateOptionUI();//call this in loadoutpicker when wrapping this up
    }

    public void UpdateOptionUI()
    {
        optionName.text = weapon.name;
        optionDescription.text = weapon.description;

        optionLevelValue.text = GameManager.instance.weaponLevels[weapon].ToString();

        optionImage.sprite = weapon.sprite;
    }

    void OnClickTask()
    {
        loadoutPicker.SetSlot(weapon, GetComponent<RectTransform>().position);
    }

    public void SetInteractable(bool flag)
    {
        GetComponent<Button>().interactable = flag;
    }
}
