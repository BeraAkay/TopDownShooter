using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public enum Types { Melee, Sniper, Scatter, SingleShot, Burst };
    Dictionary<Types, int> typeCounts;
    Dictionary<Types, int> typeCaps;

    [SerializeField]
    Cap[] typeInfo;
    void Start()
    {
        CreateCounter();
    }

    void CreateCounter()
    {
        typeCounts = new Dictionary<Types, int>();
        typeCaps = new Dictionary<Types, int>();

        foreach(Cap info in typeInfo)
        {
            typeCounts[info.type] = 0;
            typeCaps[info.type] = info.cap;
        }
    }

    bool UnderCap(Types type)
    {
        return typeCounts[type] < typeCaps[type];
    }

    public bool AddCount(Types type)
    {
        if (UnderCap(type))
        {
            typeCounts[type]++;
            return true;
        }
        return false;
    }

    public void RemoveCount(Types type)
    {
        typeCounts[type]--;
    }

    [Serializable]
    public class Cap
    {
        public Types type;
        public int cap;
    }
}
