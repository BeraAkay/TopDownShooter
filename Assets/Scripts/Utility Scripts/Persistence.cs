using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Persistence<TKey, TValue> 
{
    /*
    public void SetPlayerUniversals()
    {
        string statString = "";

        PlayerPrefs.SetString("UniversalStats", statString);
    }

    public string EncodeStats(int[] statLevels)// needs unit test
    {
        // Idea is to encode { 0, 1 , 2, 3, 10 } as "5*00-01-02-03-10" 5 for array size, and double digit for each value as stats are represented by levels of said stat and wont go over 2 digits
        
        string statString = "";

        statString += statLevels.Length.ToString();

        foreach(int stat in statLevels)
        {
            statString += "*" + stat.ToString("D2");
        }

        return statString;
    }

    public int[] DecodeStats(string statString)// needs unit test
    {
        int length = 0;
        while (statString[length] != '*')
        {
            length++;
        }
        int startPos = length + 1;

        if(!int.TryParse(statString.Substring(0, length), out length))
        {
            throw new System.ArgumentException("Invalid size definition in input.", statString);
        }
        
        
        int[] statLevels = new int[length];

        for(int i = 0; i < length; i++)
        {
            if (!int.TryParse(statString.Substring(startPos + (i * 3), startPos + (i*3) + 2), out statLevels[i]))
            {
                throw new System.ArgumentException("Invalid stat in input at stat index " + i.ToString() + ".", statString);
            }

        }
        return statLevels;
    }

    */
    public static void Save(Dictionary<TKey, TValue> dict, string id)
    {
        Pairs pairs = new Pairs(dict);
        string json = JsonUtility.ToJson(pairs);
        PlayerPrefs.SetString(id, json);
    }

    public static Dictionary<TKey, TValue> Load(string id)
    {
        string json = PlayerPrefs.GetString(id);
        if (json == "")
            return null;
        Pairs pairs = JsonUtility.FromJson<Pairs>(json);
        return pairs.ToDict();
    }

    public static void Wipe(string id)
    {
        PlayerPrefs.DeleteKey(id);
    }


    [Serializable]//this whole thing is just for jsonutility to be able to serialize the dicts. maybe i shouldve used newtonsoft as the forums said.
    public struct Pairs
    {
        [Serializable]
        public struct Pair
        {
            public TKey Key;
            public TValue Value;
            public Pair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public List<Pair> items;

        public Pairs(Dictionary<TKey, TValue> dict)
        {
            items = new List<Pair>();
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                items.Add(new Pair(pair.Key, pair.Value));
            }
        }

        public readonly Dictionary<TKey, TValue> ToDict()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (Pair pair in items)
            {
                dict.Add(pair.Key, pair.Value);
            }
            return dict;
        }
    }

}
