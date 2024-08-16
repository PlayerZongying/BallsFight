using System;
using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnergyCube : NetworkBehaviour
{
    public GameObject prefab;

    public ushort energyRecharge = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!NetworkManager.Singleton.IsServer) return;

        PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
        if (pc)
        {
            pc.GainEnergy(energyRecharge);
            // NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);

            NetworkObject.Despawn();
            EnergyCubeSpawner.Singleton.activeEnergyCubeCount--;
        }
    }
}