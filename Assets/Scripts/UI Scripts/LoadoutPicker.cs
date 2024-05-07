using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPicker : MonoBehaviour
{
    [SerializeField]
    LoadoutOption[] loadoutOptions;

    [SerializeField]
    RectTransform[] slotOverlays;

    int loadoutSlots = 2;

    Weapon[] pickedWeapons;

    [SerializeField]
    Button confirmButton;

    void Start()
    {
        pickedWeapons = new Weapon[loadoutSlots];

        LockOtherOptions(false);
    }

    public void ConfirmLoadout()
    {
        //load selected weapons to player, set their levels by looking at gamemanager levels
        PlayerInteractions.instance.Initialize(pickedWeapons);
        GameManager.instance.StartGame();

    }

    public void UpdateOptionsUI()
    {
        foreach(LoadoutOption option in loadoutOptions)
        {
            option.UpdateOptionUI();
        }
    }

    public void SetSlot(Weapon weapon, Vector3 position)//do this tomorrow it is very dumb to do it like this
    {
        if(weapon == null)
        {
            return;
        }

        if (weapon == pickedWeapons[0])
        {
            //remove overlay from 0 and remove the first one and unlock options
            pickedWeapons[0] = null;
            slotOverlays[0].gameObject.SetActive(false);
        }
        else if(weapon == pickedWeapons[1])
        {
            //remove overlay from 1 and rmeove the second one and unlock options
            pickedWeapons[1] = null;
            slotOverlays[1].gameObject.SetActive(false);
        }
        else
        {
            if (pickedWeapons[0] == null)
            {
                pickedWeapons[0] = weapon;
                slotOverlays[0].position = position;
                slotOverlays[0].gameObject.SetActive(true);

            }
            else if (pickedWeapons[1] == null)
            {
                pickedWeapons[1] = weapon;

                slotOverlays[1].position = position;
                slotOverlays[1].gameObject.SetActive(true);
            }
        }

        LockOtherOptions(!pickedWeapons.Contains(null));

        /*
        string s = "";
        foreach(Weapon w in pickedWeapons)
        {
            s+= (w == null ? "null" : w.name) + " ";
        }
        Debug.Log(s);
        */
    }

    void LockOtherOptions(bool flag)
    {
        //Debug.Log(flag ? "LOCKING" : "UNLOCKING");
        if (!flag)//unlocking
        {
            for (int i = 0; i < loadoutOptions.Length; i++)
            {
                loadoutOptions[i].SetInteractable(!flag);
            }
            EnableConfirm(false);
        }
        else//locking
        {
            for (int i = 0; i < loadoutOptions.Length; i++)
            {
                if (!pickedWeapons.Contains(loadoutOptions[i].weapon))
                {
                    loadoutOptions[i].SetInteractable(!flag);
                }
            }
            EnableConfirm(true);
        }
        
    }

    void EnableConfirm(bool flag)
    {
        confirmButton.interactable = flag;
    }

    

}
