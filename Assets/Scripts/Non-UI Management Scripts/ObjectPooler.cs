using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public enum Poolable { BasicEnemy, SniperPellet, AutoPellet, BurstPellet, ScatterPellet};

    public ObjectPool[] objectPools;

    //public ObjectPool[] enemyPools;

    public Dictionary<Poolable, ObjectPool> poolDict;
    //public Dictionary<Poolable, ObjectPool> objPoolDict;
    //public Dictionary<Poolable, ObjectPool> enemyPoolDict;

    /*
    void Start()
    {
        FillPools();//make this call in somewhere else and remove the monobehaviour from the objectpooler class
    }
    */

    public void FillPools()
    {
        poolDict = new Dictionary<Poolable, ObjectPool>();

        foreach(ObjectPool objectPool in objectPools)
        {
            objectPool.CreatePool();
            poolDict.Add(objectPool.type, objectPool);

        }
    }


    [System.Serializable]
    public class ObjectPool
    {
        public string poolName;
        public Poolable type;
        public GameObject poolObject;
        //int poolDepth;
        public int capacity;

        public GameObject poolParent;

        Queue<GameObject> pool;

        public void CreatePool()
        {
            if(poolParent == null)
            {
                poolParent = new GameObject(poolName);
            }

            pool = new Queue<GameObject>();
            for (int i = 0; i < capacity; i++)
            {
                GameObject go = Instantiate(poolObject, poolParent.transform);
                go.SetActive(false);
                pool.Enqueue(go);
            }
        }

        public GameObject GiveObject()//gives the object as deactivated
        {
            if(pool.Count == 0)
            {
                GameObject go = Instantiate(poolObject, null);
                go.SetActive(false);
                return go;
            }
            GameObject obj = pool.Dequeue();
            obj.transform.parent = null;
            return obj;
        }

        public void ReturnObject(GameObject obj)//takes active object, deactivates it and puts back in pool
        {
            if (pool.Count == capacity)
            {
                Destroy(obj);
                return;
            }
            obj.SetActive(false);
            obj.transform.parent = poolParent.transform;
            pool.Enqueue(obj);
        }

        public void NewEntry(GameObject obj)//does the same as above so might remove and use above to store objects that had to be created on runtime, or maybe not store them idk
        {
            obj.SetActive(false);
            obj.transform.parent = poolParent.transform;
            pool.Enqueue(obj);
        }

        public int PoolCount()
        {
            return pool.Count;
        }

        public float Fullness()
        {
            return pool.Count / (float)capacity;
        }

    }




}
