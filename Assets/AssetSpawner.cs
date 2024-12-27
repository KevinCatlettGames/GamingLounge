using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetSpawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    private int currentIndex = 0;
    private GameObject lastObject;
    public float waitTime = 3f; 
    private void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        if (lastObject != null)
        {
            Destroy(lastObject);
        }
        lastObject = Instantiate(prefabs[currentIndex], transform.position, Quaternion.identity);
        if (currentIndex >= prefabs.Count-1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
        
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(Spawning());
    }
}
