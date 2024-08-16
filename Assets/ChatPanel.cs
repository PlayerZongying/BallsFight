using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : NetworkBehaviour
{
    public static ChatPanel Singleton;
    public GameObject chatPanel;

    public TMP_InputField chatInputField;

    public TextMeshProUGUI chatTMP;

    public PlayerController pc;

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
        chatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Return))
        // {
        //     chatPanel.SetActive(!chatPanel.activeSelf);
        // }
    }

    public void ToggleChatPanel()
    {
        chatPanel.SetActive(!chatPanel.activeSelf);
        
    }
    
    public void SendText()
    {
        string textToSend = chatInputField.text;
        if (textToSend == "") return;
        // textToSend += "\n";
        // chatTMP.text += textToSend;

        textToSend = "Player" + pc.NetworkObject.OwnerClientId + ": " + textToSend;

        AddTextServerRpc(textToSend);
        chatInputField.text = "";
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddTextServerRpc(string text)
    {
        AddTextClientRpc(text);
    }
    
    [ClientRpc]
    public void AddTextClientRpc(string text)
    {
        AddText(text);
    }

    public void AddText(string text)
    {
        if(text == "") return;
        chatTMP.text += text + "\n";

    }


    public override void OnNetworkDespawn()
    {
        chatTMP.text = "";
        base.OnNetworkDespawn();
    }
}
