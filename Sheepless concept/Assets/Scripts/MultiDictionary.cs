using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class stores more Value for every Key. Value set doesn't contain duplicate elements.
/// </summary>
/// <typeparam name="TKey">Type of the key.</typeparam>
/// <typeparam name="TValue">Type of the value</typeparam>
public class MultiDictionary<TKey, TValue> 
{
    Dictionary<TKey, HashSet<TValue>> dictionary;

    public MultiDictionary(){ dictionary = new Dictionary<TKey, HashSet<TValue>>(); }
    public MultiDictionary(int capacity){ dictionary = new Dictionary<TKey, HashSet<TValue>>(capacity); }

    public void Add(TKey key, TValue value)
    {
        HashSet<TValue> hashSet;

        if (!dictionary.TryGetValue(key, out hashSet))
        {
            hashSet = new HashSet<TValue>();
            hashSet.Add(value);
            dictionary.Add(key, hashSet);
        }
        else
            hashSet.Add(value);
    }

    public void Remove(TKey key, TValue value)
    {
        HashSet<TValue> hashSet;

        if (dictionary.TryGetValue(key, out hashSet))
        {
            hashSet.Remove(value);

            if (hashSet.Count == 0)
                dictionary.Remove(key);
        }
    }

    public IEnumerable<TKey> Keys
    {
        get
        {
            return dictionary.Keys;
        }
    }


    public HashSet<TValue> GetValue(TKey key)
    {
        HashSet<TValue> hashSet;

        if (!dictionary.TryGetValue(key, out hashSet))
        {
            hashSet = new HashSet<TValue>();
        }
        return hashSet;
    }
}
