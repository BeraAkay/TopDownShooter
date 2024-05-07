using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Weapon : ScriptableObject, IUpgradable
{
    public new string name;

    public int level, maxLevel;

    public string description;

    [HideInInspector]
    public float damage, recoveryTime, pelletsPerShot, spread, pelletSize, pelletSpeed, critModifier, shotInterval, pelletLifetime;

    [SerializeField]
    float baseDamage, baseRecoveryTime, basePelletsPerShot, baseSpread, basePelletSize, basePelletSpeed, baseCritModifier, baseShotInterval, basePelletLifetime;
    [SerializeField]
    float damageCap, recoveryTimeCap, pelletsPerShotCap, spreadCap, pelletSizeCap, pelletSpeedCap, critModifierCap, shotIntervalCap, pelletLifetimeCap;

    public Sprite sprite;

    public Sprite enemySprite;

    public GameObject pellet;
    public ObjectPooler.Poolable poolID;

    public int upgradeClass
    {
        get
        {
            return 1;
        }
    }

    public void Upgrade()
    {
        if(level == 0)
        {
            Initialize(true);
            return;
        }

        damage += (1.0f / maxLevel) * (damageCap - baseDamage);
        recoveryTime += (1.0f / maxLevel) * (recoveryTimeCap - baseRecoveryTime);
        pelletsPerShot += (1.0f / maxLevel) * (pelletsPerShotCap - basePelletsPerShot);
        spread += (1.0f / maxLevel) * (spreadCap - baseSpread);
        pelletSize += (1.0f / maxLevel) * (pelletSizeCap - basePelletSize);
        pelletSpeed += (1.0f / maxLevel) * (pelletSpeedCap - basePelletSpeed);
        critModifier += (1.0f / maxLevel) * (critModifierCap - baseCritModifier);
        shotInterval += (1.0f / maxLevel) * (shotIntervalCap - baseShotInterval);
        pelletLifetime += (1.0f / maxLevel) * (pelletLifetimeCap - basePelletLifetime);
        level += 1;
    }

    public void SetLevel(int toLevel)
    {
        damage = (toLevel-1) * ((1.0f / maxLevel) * (damageCap - baseDamage)) + baseDamage;
        recoveryTime = (toLevel-1) * ((1.0f / maxLevel) * (recoveryTimeCap - baseRecoveryTime)) + baseRecoveryTime;
        pelletsPerShot = (toLevel - 1) * ((1.0f / maxLevel) * (pelletsPerShotCap - basePelletsPerShot)) + basePelletsPerShot;
        spread = (toLevel - 1) * ((1.0f / maxLevel) * (spreadCap - baseSpread)) + baseSpread;
        pelletSize = (toLevel - 1) * ((1.0f / maxLevel) * (pelletSizeCap - basePelletSize)) + basePelletSize;
        pelletSpeed = (toLevel - 1) * ((1.0f / maxLevel) * (pelletSpeedCap - basePelletSpeed)) + basePelletSpeed;
        critModifier = (toLevel - 1) * ((1.0f / maxLevel) * (critModifierCap - baseCritModifier)) + baseCritModifier;
        shotInterval = (toLevel - 1) * ((1.0f / maxLevel) * (shotIntervalCap - baseShotInterval)) + baseShotInterval;
        pelletLifetime = (toLevel - 1) * ((1.0f / maxLevel) * (pelletLifetimeCap - basePelletLifetime)) + basePelletLifetime;
        level = toLevel;
    }

    public void Initialize(bool asActive = true)
    {
        if (asActive)
        {
            damage = baseDamage;
            recoveryTime = baseRecoveryTime;
            pelletsPerShot = basePelletsPerShot;
            spread = baseSpread;
            pelletSize = basePelletSize;
            pelletSpeed = basePelletSpeed;
            critModifier = baseCritModifier;
            shotInterval = baseShotInterval;
            pelletLifetime = basePelletLifetime;
            level = 1;
        }
        else
        {
            level = 0;
        }
    }

    public void ShowWeapon()
    {
        Debug.Log("Damage: " + pelletsPerShot.ToString());
        Debug.Log("Pellets: " + pelletsPerShot.ToString());
    }
}
