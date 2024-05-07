using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxScript : MonoBehaviour
{
    public float meleeDamage;
    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IDamageable dmgable;
        if(collider.TryGetComponent<IDamageable>(out dmgable) && transform.parent != collider.transform)//so the weapon doesnt damage the owner
        {
            dmgable.TakeDamage(meleeDamage);
        }
    }

}
