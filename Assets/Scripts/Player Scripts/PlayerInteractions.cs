using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : Attacker
{
    //public float[] uniqueStats;//0:regen, 1: crit, 2: pickupRange, 3: xpMultiplier, 4: extra lives, 5: curse

    int currentWeaponIndex;

    [HideInInspector]
    public Weapon mainWeapon, secondaryWeapon;
    //public Weapon[] playerWeapons;

    public static PlayerInteractions instance;

    Coroutine regen;

    [SerializeField]
    Stats persistentStats;

    int resurrectCount;

    public float resurrectionTime;

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
        //this.enabled = ValidateStatSheet();
        
    }


    private void FixedUpdate()
    {
        
    }

    public void Initialize(Weapon[] weapons)
    {
        resurrectCount = 0;
        InitializeStats();
        InitializeWeapons(weapons);

        //EquipWeapon(playerWeapons[0]);
        currentWeaponIndex = 0;

        visManager.SetInvulnerable(0);

        Heal(0);

        regen = StartCoroutine(Regenerate());
    }

    public override void InitializeStats()
    {
        base.InitializeStats();

        persistentStats.CreateSheet();

        AddStatBuffs();

        stats[Stats.Types.CurrentHealth] = stats[Stats.Types.MaxHealth];
    }

    void AddStatBuffs()
    {
        foreach(Stats.Types stat in persistentStats.sheetDict.Keys)
        {
            if (stats.ContainsKey(stat))
            {
                stats[stat] += persistentStats.sheetDict[stat].ValueAtLevel(GameManager.instance.persistentLevels[stat]);
            }
            else
            {
                statLevels[stat] = GameManager.instance.persistentLevels[stat];
                stats[stat] = persistentStats.sheetDict[stat].ValueAtLevel(GameManager.instance.persistentLevels[stat]);
            }

        }
    }
    
    public int MaxCurse()
    {
        return (int)(persistentStats.sheetDict[Stats.Types.Curse].maxValue);
    }

    void InitializeWeapons(Weapon[] weapons)
    {
        //Initialize player weapons and set their levels
        mainWeapon = weapons[0];
        mainWeapon.Initialize(true);
        mainWeapon.SetLevel(GameManager.instance.weaponLevels[mainWeapon]);
        secondaryWeapon = weapons[1];
        secondaryWeapon.Initialize(true);
        secondaryWeapon.SetLevel(GameManager.instance.weaponLevels[secondaryWeapon]);

        visManager.EquipWeapons(mainWeapon, secondaryWeapon);
        //playerWeapons = new Weapon[GameManager.instance.baseWeapons.Length];
        //GiveAllWeapons();
    }

    public override void Die()
    {
        if (stats[Stats.Types.ExtraLives] - resurrectCount > 0)
        {
            StartCoroutine(Resurrect());
            resurrectCount++;
            return;
        }
        //Debug.Log("Player is Dead");
        StopCoroutine(regen);
        GameManager.instance.PlayerDeath();
    }

    IEnumerator Resurrect()
    {
        invulnerable = true;
        visManager.SetInvulnerable(1);
        stats[Stats.Types.CurrentHealth] = stats[Stats.Types.MaxHealth];
        Heal(0);

        Explode();



        float resTick = 0.05f;
        float freq = resurrectionTime / resTick;
        for(int i = 0; i < freq; i++)
        {
            yield return new WaitForSeconds(resTick);
            visManager.SetInvulnerable((freq - i) / freq);
        }

        //yield return new WaitForSeconds(resurrectionTime);

        visManager.SetInvulnerable(0);
        invulnerable = false;
    }

    void Explode()
    {
        float damage, pelletSpeed;
        bool crit;

        float degreePerPellets = 30;

        for(float i = 0; i < 360; i += degreePerPellets)
        {
            //main
            crit = Random.Range(0f, 1f) < stats[Stats.Types.CriticalChance];

            pelletSpeed = mainWeapon.pelletSpeed;
            damage = mainWeapon.damage * stats[Stats.Types.DamageMultiplier] * 2;//explode should be cool so we double the damage

            damage *= crit ? mainWeapon.critModifier : 1;

            GameObject newPellet = GameManager.Pooler.poolDict[mainWeapon.poolID].GiveObject();
            newPellet.transform.position = barrelTransform.position;
            newPellet.transform.rotation = barrelTransform.rotation;

            Vector3 rotVec = Vector3.zero;

            rotVec.z = i;
            newPellet.transform.Rotate(rotVec);

            newPellet.GetComponent<PelletHitboxScript>().SetVals(mainWeapon.poolID, pelletSpeed, damage, gameObject, mainWeapon.pelletLifetime, mainWeapon.pelletSize, crit);
            newPellet.SetActive(true);

            //secondary
            crit = Random.Range(0f, 1f) < stats[Stats.Types.CriticalChance];

            pelletSpeed = secondaryWeapon.pelletSpeed;
            damage = secondaryWeapon.damage * stats[Stats.Types.DamageMultiplier] * 2;//explode should be cool so we double the damage

            damage *= crit ? secondaryWeapon.critModifier : 1;

            newPellet = GameManager.Pooler.poolDict[secondaryWeapon.poolID].GiveObject();
            newPellet.transform.position = barrelTransform.position;
            newPellet.transform.rotation = barrelTransform.rotation;

            rotVec = Vector3.zero;

            rotVec.z = i + (degreePerPellets / 2);
            newPellet.transform.Rotate(rotVec);

            newPellet.GetComponent<PelletHitboxScript>().SetVals(secondaryWeapon.poolID, pelletSpeed, damage, gameObject, secondaryWeapon.pelletLifetime, secondaryWeapon.pelletSize, crit);
            newPellet.SetActive(true);
        }
    }


    public void AttackCall(Weapon weapon)
    {
        if (!attacking)
        {
            EquipWeapon(weapon);
            StartCoroutine(Attack(Random.Range(0f,1f) < stats[Stats.Types.CriticalChance]));
        } 
    }
    /*
    public void ChangeWeapon(int changeDir)
    {
        if (attacking && !recovering)
        {
            return;
        }

        int weaponCount = playerWeapons.Length;

        currentWeaponIndex = (changeDir + currentWeaponIndex + weaponCount) % weaponCount;//+weaponCount isnt necessary in the case of >weaponCount-1 but this is less code

        while(playerWeapons[currentWeaponIndex] == null)
        {

            currentWeaponIndex = (changeDir + currentWeaponIndex + weaponCount) % weaponCount;
        }

        EquipWeapon(playerWeapons[currentWeaponIndex]);
    }

    IEnumerator ChangeWeaponOnAvailable(int changeDir)//maybe input buffer? it might be unnecessary but idk feels a bit jank otherwise
    {
        if (attacking && !recovering)
        {
            yield return new WaitUntil(() => recovering || !attacking);
        }
        int weaponCount = playerWeapons.Length;

        currentWeaponIndex = (changeDir + currentWeaponIndex + weaponCount) % weaponCount;//+weaponCount isnt necessary in the case of >weaponCount-1 but this is less code

        while (playerWeapons[currentWeaponIndex] == null)
        {

            currentWeaponIndex = (changeDir + currentWeaponIndex + weaponCount) % weaponCount;
        }

        EquipWeapon(playerWeapons[currentWeaponIndex]);
    }
    */
    IEnumerator Regenerate()
    {
        for(; ; )
        {
            if (stats[Stats.Types.Regeneration] > 0)
            {
                Heal(stats[Stats.Types.Regeneration]);
            }

            yield return new WaitForSeconds(.1f);
        }
    }

}
