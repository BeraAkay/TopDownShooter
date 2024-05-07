using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    //public float[] universalStats;//0: HP 1: MTG 2: DMG Mult 3: RCV Mult 4:MS
    
    public Dictionary<Stats.Types , float> stats;
    public Dictionary<Stats.Types , int> statLevels;
    [SerializeField]
    public Stats statSheet;


    [HideInInspector]
    public bool invulnerable = false;

    bool dead = false;


    public virtual void TakeDamage(float damage)
    {
        if(invulnerable)
        {
            return;
        }

        stats[Stats.Types.CurrentHealth] -= damage * (1 - stats[Stats.Types.Mitigation]);

        if(stats[Stats.Types.CurrentHealth] <= 0 && !dead)
        {
            dead = true;
            Die();
        }
    }

    public virtual void InitializeStats()
    {
        statSheet.CreateSheet();
        stats = new Dictionary<Stats.Types, float>();
        statLevels = new Dictionary<Stats.Types, int>();

        foreach (KeyValuePair<Stats.Types, Stats.Stat> pair in statSheet.sheetDict)
        {
            stats.Add(pair.Key, pair.Value.minValue);
            statLevels.Add(pair.Key, pair.Value.minLevel);
        }

        stats[Stats.Types.CurrentHealth] = stats[Stats.Types.MaxHealth];
        statLevels[Stats.Types.CurrentHealth] = statLevels[Stats.Types.MaxHealth];

    }

    public virtual void Heal(float heal)
    {
        stats[Stats.Types.CurrentHealth] = Mathf.Min(stats[Stats.Types.CurrentHealth] + heal, stats[Stats.Types.MaxHealth]);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
