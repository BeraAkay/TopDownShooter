using System.Collections;
using UnityEngine;
using Pathfinding;
using static OptionScript;

public class SimpleEnemy : Attacker
{
    public GameObject player;

    //public EnemyParameters.Type type;

    //a variable for enemy type or weapon type for enemy
    //that will be set or init in initenemytype()

    [SerializeField]
    float effectiveRange = 2;

    [SerializeField]
    int dropValue;

    [SerializeField]
    Drop[] drops;

    bool inRange;
    public AIDestinationSetter destSetter;
    public AIPath pathScript;

    float meleeRange;
    float meleeMult = 1;

    int level = 1;

    Coroutine actionLoop, rangeChecker;

    private void OnEnable()
    {
        InitEnemyType();

        InitializeStats();


        visManager.SetRed(stats[Stats.Types.CurrentHealth] / stats[Stats.Types.MaxHealth]);

        meleeRange = GetComponent<CircleCollider2D>().radius + player.GetComponent<CircleCollider2D>().radius;

        rangeChecker = StartCoroutine(RangeChecker());

        actionLoop = StartCoroutine(ActionLoop());
    }

    public override void InitializeStats()
    {
        base.InitializeStats();

        ScaleEnemy(LevelManager.instance.Difficulty);
        pathScript.maxSpeed = stats[Stats.Types.MovementSpeed];
    }


    void ScaleEnemy(int difficulty)
    {
        level = difficulty;
        foreach(Stats.Types type in statSheet.sheetDict.Keys)
        {
            stats[type] = statSheet.sheetDict[type].ValueAtLevel(level);
            statLevels[type] = level;
        }

        stats[Stats.Types.CurrentHealth] = stats[Stats.Types.MaxHealth];
        statLevels[Stats.Types.CurrentHealth] = statLevels[Stats.Types.MaxHealth];

    }

    public override void Die()
    {
        //play death/destr anim or whatever

        LevelManager.instance.SetDead(gameObject);

        foreach(Drop drop in drops)
        {
            Dropper.Instance.Drop(drop, transform.position, dropValue);
        }

        StopCoroutine(rangeChecker);
        StopCoroutine(actionLoop);

        GameManager.Pooler.poolDict[ObjectPooler.Poolable.BasicEnemy].ReturnObject(gameObject);
        //Destroy(gameObject);
    }

    public void Collect()
    {
        //LevelManager.instance.SetDead(gameObject);
        StopCoroutine(rangeChecker);
        StopCoroutine(actionLoop);
        GameManager.Pooler.poolDict[ObjectPooler.Poolable.BasicEnemy].ReturnObject(gameObject);
    }

    void InitEnemyType()
    {
        //type = EnemyParameters.Type.RangedSingle;
        if (weapon)
        {
            weapon = Instantiate(weapon);
            weapon.Initialize();
            EquipWeapon(weapon);
            meleeMult = 1;
            effectiveRange = weapon.pelletLifetime * weapon.pelletSpeed * 0.8f;

            //pathScript.endReachedDistance = weapon.pelletLifetime * weapon.pelletSpeed * 0.8f;//effectiveRange;
        }
        else
        {
            effectiveRange = 0;

            //pathScript.endReachedDistance = 0;// effectiveRange;
            meleeMult = 2;
        }
    }


    bool SimpleRangeCheck()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= effectiveRange;
    }

    IEnumerator RangeChecker()
    {
        for(; ; )
        {
            if (CheckMelee())
            {
                player.GetComponent<PlayerInteractions>().TakeDamage(stats[Stats.Types.MeleeDamage] * stats[Stats.Types.DamageMultiplier] * meleeMult);
            }
            //inRange = SimpleRangeCheck();

            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator ActionLoop()
    {
        for(; ; )
        {
            SimpleAct();

            yield return new WaitForSeconds(.1f);
        }
    }

    void SimpleAct()
    {
        destSetter.target = null;

        if (SimpleRangeCheck())
        {
            pathScript.endReachedDistance = effectiveRange;

            Vector2 dir = player.transform.position - transform.position;
            float angle = Vector3.SignedAngle(Vector3.up, dir, new Vector3(0, 0, 1));//Mathf.Atan2(crosshairDelta.y, crosshairDelta.x) * Mathf.Rad2Deg;
                                                                                     //Debug.Log(angle);

            Vector3 rotVec = Vector3.zero;
            rotVec.z = angle;
            transform.rotation = Quaternion.identity;
            transform.Rotate(rotVec);

            if (!attacking)
            {
                Debug.Log("enemyFire");
                StartCoroutine(Attack(false));//enemies dont crit
            }
        }
        else
        {
            pathScript.endReachedDistance = effectiveRange;
            destSetter.target = player.transform;
        }

    }

    public override void EquipWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        visManager.BecomeWeapon(newWeapon);
    }

    bool CheckMelee()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= meleeRange;
    }



}
