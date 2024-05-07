using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public Dictionary<string, Weapon> activeweapons;

    //bitmask for weapons, value in binary should have 1 on the index of weaponId
    int bitmask;

    public List<Weapon> allWeapons;

    void Start()
    {
        
    }

    public Weapon AddWeapon(int weaponId)
    {
        Weapon weapon = null;

        if (!activeweapons.ContainsKey(allWeapons[weaponId].name))
        {
            weapon = Instantiate(allWeapons[weaponId]);
            activeweapons[weapon.name] = weapon;
        }

        return weapon;
    }


}
