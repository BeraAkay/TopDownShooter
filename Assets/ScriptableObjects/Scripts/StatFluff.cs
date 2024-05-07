using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatFluff : ScriptableObject
{
    [SerializeField]
    Fluff[] fluffs;

    public Fluff Get(Stats.Types type)
    {
        return fluffs[(int)type];
    }

    [System.Serializable]
    public class Fluff
    {
        [SerializeField]
        Stats.Types type;
        public string name;
        public string abbreviation;
        public Sprite sprite;
        public string description;
    }
}
