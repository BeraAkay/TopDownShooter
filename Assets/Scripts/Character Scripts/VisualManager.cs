using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public Animator animator;

    public SpriteRenderer weaponSprite, secondaryWeaponSprite;

    public Vector3 bulletSpawnOffset;

    public SpriteRenderer characterVisual;

    public SpriteRenderer invulnerableVisual;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void SetBool(string paramName, bool paramValue)
    {
        animator.SetBool(paramName, paramValue);
    }

    public void EquipWeapon(Weapon weapon)
    {
        weaponSprite.sprite = null;

        weaponSprite.transform.localPosition = Vector3.zero;

        if (weapon.sprite != null)
        {
            weaponSprite.sprite = weapon.sprite;
        }
    }

    public void EquipWeapons(Weapon main, Weapon secondary)
    {
        weaponSprite.sprite = null;

        weaponSprite.transform.localPosition = Vector3.zero;

        if (main.sprite != null)
        {
            weaponSprite.sprite = main.sprite;
        }

        secondaryWeaponSprite.sprite = null;

        secondaryWeaponSprite.transform.localPosition = Vector3.zero;

        if (secondary.sprite != null)
        {
            secondaryWeaponSprite.sprite = secondary.sprite;
        }
    }

    public void BecomeWeapon(Weapon weapon)
    {
        weaponSprite.sprite = null;
        weaponSprite.transform.localPosition = Vector3.zero;

        if (weapon.enemySprite != null)
        {
            weaponSprite.sprite = weapon.enemySprite;
        }

    }

    public void SetRed(float saturation)
    {
        characterVisual.color = new Color(1, saturation, saturation);
    }

    public void SetInvulnerable(float alpha)
    {
        if(invulnerableVisual == null)
        {
            return;
        }

        Color color = invulnerableVisual.color;
        color.a = alpha;
        invulnerableVisual.color = color;
    }

}
