using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    public DictItem[] items;

    /*
    SerializableDictionary() : base() 
    {
        if (items != null)
        {
            Initialize();
        }
    }
    SerializableDictionary(int capacity) : base(capacity) 
    {
        if (items != null)
        {
            Initialize();
        }
    }
    */

    public void Initialize()
    {
        this.Clear();
        foreach (DictItem item in items)
        {
            if (!this.ContainsKey(item.key))
                this.Add(item.key, item.value);
        }        
    }

    public void OnBeforeSerialize()
    {
        items = new DictItem[this.Count];
        
        int i = 0;
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            items[i] = new DictItem(pair.Key, pair.Value);
            i++;
        }
        
    }

    public void OnAfterDeserialize()
    {
        Initialize();
    }



    [Serializable]
    public class DictItem
    {
        /*
        [SerializeField]
        string name;
        */
        public TKey key;
        public TValue value;

        public DictItem(TKey key_, TValue value_)
        {
            key = key_;
            value = value_;
        }
    }


}


