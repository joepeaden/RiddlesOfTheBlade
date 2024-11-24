using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public int enemyPoolSize;
    private List<GameObject> enemies;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemies = CreatePool(enemyPrefab, enemies, enemyPoolSize);
    }

    private List<GameObject> CreatePool(GameObject prefab, List<GameObject> listToAssign, int count)
    {
        listToAssign = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < count; i++)
        {
            tmp = Instantiate(prefab, transform);
            tmp.SetActive(false);
            listToAssign.Add(tmp);
        }

        return listToAssign;
    }

    private GameObject GetPooledObject(List<GameObject> theList, GameObject prefab)
    {
        for (int i = 0; i < theList.Count; i++)
        {
            if (!theList[i].activeInHierarchy)
            {
                return theList[i];
            }
        }

        GameObject newObject = Instantiate(prefab);
        theList.Add(newObject);
        return newObject;
    }

    public GameObject GetEnemy()
    {
        return GetPooledObject(enemies, enemyPrefab);
    }
}
