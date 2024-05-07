using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    public GameObject Player { get; }

    public Dictionary<Stats.Types, float> StatSheet { get; }
}
