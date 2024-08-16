using System;
using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnergyCubeSpawner : NetworkBehaviour
{
    public static EnergyCubeSpawner Singleton;
    public int activeEnergyCubeCount = 0;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxActiveEnergyCubeAcount = 50;

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
        NetworkManager.Singleton.OnServerStarted += SpawnEnergyCubeStart;
    }

    private void SpawnEnergyCubeStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnEnergyCubeStart;
        NetworkObjectPool.Singleton.OnNetworkSpawn();

        for (int i = 0; i < 30; i++)
        {
            SpawnEnergyCube();
        }

        StartCoroutine(SpawnEnergyCubeCoroutine());
    }

    void SpawnEnergyCube()
    {
        NetworkObject obj =
            NetworkObjectPool.Singleton.GetNetworkObject(prefab, GetRandomPlaceOnPlane(), quaternion.identity);
        obj.GetComponent<EnergyCube>().prefab = prefab;
        obj.Spawn(true);
        Singleton.activeEnergyCubeCount++;
    }

    IEnumerator SpawnEnergyCubeCoroutine()
    {
        while (true)
        {
            if (activeEnergyCubeCount < maxActiveEnergyCubeAcount)
            {
                SpawnEnergyCube();
            }

            yield return new WaitForSeconds(2);
        }
    }

    Vector3 GetRandomPlaceOnPlane()
    {
        Vector3 randomPlace = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));
        return randomPlace;
    }

    // Update is called once per frame
    void Update()
    {
        // print(activeEnergyCubeCount);
    }
}