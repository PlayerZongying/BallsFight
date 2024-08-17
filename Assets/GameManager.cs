using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Singleton;

    public List<PlayerController> players;

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

    public override void OnNetworkDespawn()
    {
        players.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckLastSurvivorServerRpc()
    {
        List<PlayerController> alivePlayers = new List<PlayerController>();
        foreach (var player in players)
        {
            if (player && player.energy.Value > 0)
            {
                alivePlayers.Add(player);
            }
        }

        // only one play has energy more than one
        if (alivePlayers.Count == 1)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { alivePlayers[0].NetworkObject.OwnerClientId }
                }
            };

            ClientWinningClientRpc(clientRpcParams);
        }
    }

    [ClientRpc]
    public void ClientWinningClientRpc(ClientRpcParams clientRpcParams = default)
    {
        WinLosePanel.Singleton.winPanel.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerLoseServerRpc(ulong clientId)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        ClientLosingClientRpc(clientRpcParams);
    }

    [ClientRpc]
    public void ClientLosingClientRpc(ClientRpcParams clientRpcParams = default)
    {
        WinLosePanel.Singleton.losePanel.SetActive(true);
    }
}