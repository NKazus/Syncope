using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static Dictionary<string, LinkedList<GameObject>> _poolDictionary;

    private void Start()
    {
        _poolDictionary = new Dictionary<string, LinkedList<GameObject>>();
    }

    public static GameObject getGameObjectFromPool(GameObject prefab)
    {
        if (!_poolDictionary.ContainsKey(prefab.name))
        {
            _poolDictionary[prefab.name] = new LinkedList<GameObject>();
        }
        GameObject result;
        if(_poolDictionary[prefab.name].Count > 0)
        {
            result = _poolDictionary[prefab.name].First.Value;
            _poolDictionary[prefab.name].RemoveFirst();
            result.SetActive(true);
            return result;
        }
        result = GameObject.Instantiate(prefab);
        result.name = prefab.name;
        return result;
    }

    public static void putGameObjectToPool(GameObject target)
    {
        _poolDictionary[target.name].AddFirst(target);
        target.SetActive(false);
    }
}
