using System;
using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    // Start is called before the first frame update

    public static BulletManager Singleton;

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

    [ServerRpc]public void SpawnBulletServerRpc(ulong clientId, Vector3 fireDir, Vector3 firePos)
    {
        quaternion bulletRotation = Quaternion.LookRotation(fireDir, Vector3.up);
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab, firePos, bulletRotation);
        obj.GetComponent<Bullet>().prefab = prefab;
        obj.GetComponent<Bullet>().shooterClientID = clientId;
        obj.GetComponent<Bullet>().dir = fireDir;
        obj.Spawn(true);
    }
    
    [ServerRpc]public void TestServerRpc(ulong clientId)
    {
        print("TestServerRpc from " + clientId);
    }
}