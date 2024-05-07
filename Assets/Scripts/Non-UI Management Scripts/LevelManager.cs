using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    List<GameObject> activeEnemies;
    public int maxEnemyCount;

    public System.Action collectDropCaller;
    //public GameObject enemyPrefab;

    public float spawnFreq;
    public int groupSpawnSize;

    public float spawnOffset;

    public Vector3 spawnRanges;

    public Transform playerTransform;

    float levelTimer;

    public int accumulatedCurrency
    {
        get
        {
            return (int)levelTimer;
        }
    }

    Coroutine timer, spawner;
    public int Difficulty
    {
        get
        {
            return (int) Mathf.Min(enemyMaxLevel - playerMaxCurse, levelTimer * difficultyPerSecond) + playerCurse;
        }
    }
    [SerializeField]
    float difficultyPerSecond;//difficultyPerSecond
    [SerializeField]
    int enemyMaxLevel;
    int playerCurse, playerMaxCurse;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        GameManager.Pooler.FillPools();

        CalculateSpawnBounds();
    }

    public void Initialize()
    {
        if(activeEnemies != null)
        {
            ClearLevel();
        }
        else
        {
            activeEnemies = new List<GameObject>();
        }
        //do the horizontal and vertical ranges here and add the offset to them

        levelTimer = 0;
        if(timer != null)
        {
            StopCoroutine(timer);
        }
        timer = StartCoroutine(Timer());
        if(spawner != null)
        {
            StopCoroutine(spawner);
        }
        spawner = StartCoroutine(Spawner());


        playerCurse = (int)PlayerInteractions.instance.stats[Stats.Types.Curse];
        playerMaxCurse = PlayerInteractions.instance.MaxCurse();
    }
    IEnumerator Spawner()
    {
        for(; ; )
        {
            EnemySpawner();
            yield return new WaitForSeconds(1/spawnFreq);
        }
    }

    IEnumerator Timer()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(1);
            levelTimer += 1;
            UIManager.instance.SetLevelTimer(levelTimer);
        }
    }

    void EnemySpawner()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = GenerateSpawnPosition();
        }
        while (IsPositionOutOfBounds(spawnPosition));//instead of a dowhile, just take into account the bounds when generating the position but this is faster to code rn


        GameObject enemy = GiveEnemyObject();

        enemy.transform.position = spawnPosition;
        activeEnemies.Add(enemy);
        enemy.SetActive(true);

        //Stats.Types stype = Stats.Types.MaxHealth;
        //Debug.Log(string.Format("Stat: " + stype.ToString() + ", Value: {0}, Level: {1}", enemy.GetComponent<SimpleEnemy>().stats[stype], enemy.GetComponent<SimpleEnemy>().statLevels[stype]));
    }

    Vector3 GenerateSpawnPosition()
    {
        Vector3 position = Vector3.zero;
        bool spawnOnSide = Random.Range(0, 2) == 1 ? true : false;//decide if its gonna be a side spawn or bottom/top
        int secondaryAxisSign = Random.Range(0, 2) == 1 ? 1 : -1;//theres probably a way of doing this much better but decides whether its on either left/right or top/bottom
        //maybe just revert the axis sign depending on the out of bounds situation of the coords so that you dont repeat anything and just use the generation of nums anyway
        if (spawnOnSide)
        {
            position.x = spawnRanges.x * secondaryAxisSign; 
            position.y = Random.Range(-(spawnRanges.y), spawnRanges.y);
        }
        else
        {
            position.x = Random.Range(-(spawnRanges.x), spawnRanges.x);
            position.y = spawnRanges.y * secondaryAxisSign;
        }
        position.z = playerTransform.position.z;

        return position + playerTransform.position;
    }

    void CalculateSpawnBounds()//for this to work properly the camera needs to be on top of the player. so maybe just force the camp on top of player in here first
    {
        Vector3 camPosAbovePlayer = playerTransform.position;
        camPosAbovePlayer.z = Camera.main.transform.position.z;
        Camera.main.transform.position = camPosAbovePlayer;

        Vector3 camTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        camTopRight.z = 1;

        spawnRanges = camTopRight - playerTransform.position;
        spawnRanges.x += spawnOffset;
        spawnRanges.y += spawnOffset;

        /*
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = playerTransform.position + spawnRanges;

        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = playerTransform.position - spawnRanges;
        */

    }

    bool IsPositionOutOfBounds(Vector3 position)
    {
        return false;
    }

    GameObject GiveEnemyObject()
    {
        //look into pool for free objects, if none present instantiante
        //GameObject enemy = Instantiate(enemyPrefab);
        GameObject enemy = GameManager.Pooler.poolDict[ObjectPooler.Poolable.BasicEnemy].GiveObject();
        enemy.GetComponent<SimpleEnemy>().player = playerTransform.gameObject;
        return enemy;
    }

    public void SetDead(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

    }

    public void LootRoll(int lootLevel)
    {

    }

    void ClearLevel()
    {
        foreach(GameObject enemy in activeEnemies)
        {
            enemy.GetComponent<SimpleEnemy>().Collect();
        }

        activeEnemies.Clear();

        collectDropCaller?.Invoke();

    }

    public void SubscribeDrop(System.Action toDo)
    {
        collectDropCaller += toDo;
    }

    public void UnsubscribeDrop(System.Action toDo)
    {
        collectDropCaller -= toDo;
    }
}
