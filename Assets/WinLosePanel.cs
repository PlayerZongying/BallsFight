using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLosePanel : MonoBehaviour
{
    public static WinLosePanel Singleton;
    public GameObject winPanel;
    public GameObject losePanel;
    // Start is called before the first frame update

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
