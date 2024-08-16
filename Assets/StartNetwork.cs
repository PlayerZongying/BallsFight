using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNetwork : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Reload()
    {
        // Ensure this is called only by the server
        // if (IsServer)
        {
            // Get the current active scene
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Load the scene using Netcode's SceneManager to ensure all clients reload the scene
            NetworkManager.SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
        }
    }
}