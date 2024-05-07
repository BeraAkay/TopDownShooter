using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MeleeDamage : MonoBehaviour
{
    IEnemy enemyScript;

    float range;

    private void Start()
    {
        enemyScript = GetComponent<IEnemy>();
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        if(enemyScript == null)
        {
            this.enabled = false;
            throw new MissingComponentException("No Enemy Script Found");
        }
        if(circleCollider == null)
        {
            this.enabled = false;
            throw new MissingComponentException("No CircleCollider2D Found");
        }
        else
        {
            range = circleCollider.radius + enemyScript.Player.GetComponent<CircleCollider2D>().radius;
        }
    }


    private void FixedUpdate()
    {
        if (Check())
        {
            Debug.Log("player takes melee damage");
            enemyScript.Player.GetComponent<PlayerInteractions>().TakeDamage(enemyScript.StatSheet[Stats.Types.MeleeDamage] * enemyScript.StatSheet[Stats.Types.DamageMultiplier] * Time.fixedDeltaTime);
        }
    }

    bool Check()
    {
        if (Vector2.Distance(transform.position, enemyScript.Player.transform.position) <= range)
        {
            return true;
        }
        return false;
    }



}
