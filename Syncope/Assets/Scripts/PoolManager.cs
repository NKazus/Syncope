using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static Dictionary<string, LinkedList<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, LinkedList<GameObject>>();
    }

    public static GameObject getGameObjectFromPool(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            poolDictionary[prefab.name] = new LinkedList<GameObject>();
        }
        GameObject result;
        if(poolDictionary[prefab.name].Count > 0)
        {
            result = poolDictionary[prefab.name].First.Value;
            poolDictionary[prefab.name].RemoveFirst();
            result.SetActive(true);
            return result;
        }
        result = GameObject.Instantiate(prefab);
        result.name = prefab.name;
        return result;
    }

    public static void putGameObjectToPool(GameObject target)
    {
        poolDictionary[target.name].AddFirst(target);
        target.SetActive(false);
    }
}
