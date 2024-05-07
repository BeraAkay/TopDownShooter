using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : Damageable
{
    public float healTimer;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStats();
        stats[Stats.Types.CurrentHealth] = stats[Stats.Types.MaxHealth];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= healTimer)
        {
            stats[Stats.Types.CurrentHealth] = stats[Stats.Types.MaxHealth];
            UpdateColor();
        }

    }

    public override void TakeDamage(float damage)
    {
        stats[Stats.Types.CurrentHealth] -= damage * (1 - stats[Stats.Types.Mitigation]);
        UpdateColor();
        timer = 0;
    }

    void UpdateColor()
    {
        float healthRatio = stats[Stats.Types.CurrentHealth] / stats[Stats.Types.MaxHealth];
        GetComponent<SpriteRenderer>().color = new Color(1, healthRatio, healthRatio);
    }
}
