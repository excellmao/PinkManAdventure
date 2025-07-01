using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pooling : Singleton<Object_Pooling>
{
    Dictionary<GameObject, List<GameObject>> pool = new Dictionary<GameObject, List<GameObject>>();

    public GameObject GetPrefabs(GameObject defautprefabs)
    {
        if (pool.ContainsKey(defautprefabs))
        {
            foreach (GameObject o in pool[defautprefabs])
            {
                if(o.activeSelf) continue;
                return o;
            }
            GameObject g = Instantiate(defautprefabs, this.transform.position, Quaternion.identity);
            pool[defautprefabs].Add(g);
            g.SetActive(false);
            return g;
        }
        List<GameObject> newpool = new List<GameObject>();
        GameObject newGo = Instantiate(defautprefabs, this.transform.position, Quaternion.identity);
        newpool.Add(newGo);
        newGo.SetActive(false);
        pool.Add(defautprefabs, newpool);
        return newGo;
    }
}
