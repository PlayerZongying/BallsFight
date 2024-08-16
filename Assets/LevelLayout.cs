using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayout : MonoBehaviour
{
    public static LevelLayout Singleton;

    public List<Transform> spawnPoints;

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
