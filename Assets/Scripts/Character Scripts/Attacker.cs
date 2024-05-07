using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Damageable
{
    public Weapon weapon;

    public bool attacking;
    public bool recovering;

    public Transform barrelTransform;

    public VisualManager visManager;

    float damage, pelletSpeed;



    //public float[] universalStats;//0: HP 1: MTG 2: DMG Mult 3: RCV Mult 4:MS

    public IEnumerator Attack(bool crit)
    {
        attacking = true;

        pelletSpeed = weapon.pelletSpeed;
        damage = weapon.damage * stats[Stats.Types.DamageMultiplier];

        damage *= crit ? weapon.critModifier : 1;

        for (int i = 0; i < weapon.pelletsPerShot; i++)
        {
            GameObject newPellet = GameManager.Pooler.poolDict[weapon.poolID].GiveObject();
            newPellet.transform.position = barrelTransform.position;
            newPellet.transform.rotation = barrelTransform.rotation;

            Vector3 rotVec = Vector3.zero;
            
            rotVec.z = Random.Range(-weapon.spread, weapon.spread);//might change and make it /2 for it to be scattermax = the total angle,
                                                                                       //currently its the max angle it diverges from the straight middle
            newPellet.transform.Rotate(rotVec);

            newPellet.GetComponent<PelletHitboxScript>().SetVals(weapon.poolID, pelletSpeed, damage, gameObject, weapon.pelletLifetime, weapon.pelletSize, crit);
            newPellet.SetActive(true);
            if(weapon.shotInterval > 0)
                yield return new WaitForSeconds(weapon.shotInterval);
        }

        recovering = true;
        yield return new WaitForSeconds(weapon.recoveryTime * (1 - stats[Stats.Types.WeaponRecovery]));
        recovering = false;
        attacking = false;
    }


    public virtual void EquipWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        //visManager.EquipWeapon(newWeapon);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        visManager.SetRed(stats[Stats.Types.CurrentHealth] / stats[Stats.Types.MaxHealth]);
    }

    public override void Heal(float heal)
    {
        base.Heal(heal);
        visManager.SetRed(stats[Stats.Types.CurrentHealth] / stats[Stats.Types.MaxHealth]);
    }

}
