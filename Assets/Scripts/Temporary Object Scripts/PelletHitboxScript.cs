using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletHitboxScript : MonoBehaviour
{
    float pelletDamage;
    float pelletSpeed;

    string shooterTag;

    float lifetime;

    Coroutine lifetimeCoroutine;

    ObjectPooler.Poolable poolID;

    void Start()
    {
        //GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void OnEnable()
    {
        lifetimeCoroutine = StartCoroutine(Lifetimer(lifetime));//the counter might move to fixedupdate, if the lifetime is going to be modified while the object is already counting down
    }

    private void OnDisable()
    {
        StopCoroutine(lifetimeCoroutine);//not necessary.
    }

    void FixedUpdate()
    {
        transform.position += transform.up * pelletSpeed * Time.fixedDeltaTime;
    }
    public void SetVals(ObjectPooler.Poolable id, float speed, float damage, GameObject origin, float _lifetime, float size, bool isCrit)
    {
        pelletDamage = damage;
        pelletSpeed = speed;
        shooterTag = origin.tag;

        lifetime = _lifetime;

        transform.localScale *= size;
        poolID = id;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IDamageable dmgable;

        //Debug.Log("HitSomething");

        if (!collider.CompareTag(shooterTag) && collider.name != gameObject.name)//so the weapon doesnt damage the owner
        {
            //Debug.Log("HitSomethingImportant");
            if (collider.TryGetComponent(out dmgable))
            {
                dmgable.TakeDamage(pelletDamage);
                //Debug.Log(pelletDamage);
            }

            //Debug.Log("Trigger Collision");
            /*
            if (gameObject)
            {
                //StopCoroutine(lifetimeCoroutine);
                Destroy(gameObject);
            }
            //Debug.Log("Destroyself");
            */
        }
        

    }

    IEnumerator Lifetimer(float _lifetime)
    {
        yield return new WaitForSeconds(_lifetime);

        GameManager.Pooler.poolDict[poolID].ReturnObject(gameObject);
        //if (gameObject)
        //    Destroy(gameObject);
    }

}