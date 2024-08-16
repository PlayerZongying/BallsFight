using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.BossRoom.Infrastructure;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    public Rigidbody rb;
    public Canvas inWorldCanvas;
    public TextMeshProUGUI energyTMP;
    public Camera playerCamera;
    public BallsFightPlayerInputactions playerControls;
    private InputAction move;
    public float speed = 5;
    private InputAction fire;

    public NetworkVariable<ushort> energy = new(10, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private Vector2 moveDirection = Vector2.zero;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // rb.isKinematic = false;
        if (!IsServer)
        {
            energy.OnValueChanged += EnergyChange;
        }
    }

    void EnergyChange(ushort prevValue, ushort curValue)
    {
        UpdateScale(energy.Value);
    }

    private void Awake()
    {
        playerControls = new BallsFightPlayerInputactions();
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        fire.performed += Fire;
    }

    private void OnEnable()
    {
        // playerControls.Enable();
        move.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        // playerControls.Disable();
        move.Disable();
        fire.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        // rb.isKinematic = false;
        if (!IsOwner) return;
        print(NetworkObject.OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {
        // UI, showing energe count
        inWorldCanvas.transform.position = rb.transform.position + Vector3.up * 3;
        inWorldCanvas.transform.rotation =
            Quaternion.LookRotation(playerCamera.transform.forward, playerCamera.transform.up);
        UpdateEnergyDisplay();
        if (energy.Value == 0)
        {
            OnDisable();
        }


        // camara follows player
        if (!IsOwner) return;
        playerCamera.transform.position = rb.transform.position + Vector3.up * 20;

        moveDirection = move.ReadValue<Vector2>().normalized;

        // transform.position += speed * Time.deltaTime * new Vector3(moveDirection.x, 0, moveDirection.y);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.velocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        print("Fire!!!");
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = playerCamera.nearClipPlane;
        Vector3 worldPositionOnNearPlane = playerCamera.ScreenToWorldPoint(mousePosition);

        Vector3 fireDir = new Vector3(worldPositionOnNearPlane.x - rb.transform.position.x, 0,
            worldPositionOnNearPlane.z - rb.transform.position.z).normalized;
        Vector3 firePos = rb.transform.position + fireDir * energy.Value * 0.1f;
        
        
        SpawnBulletServerRpc(NetworkObject.OwnerClientId, fireDir, firePos);
        ReduceEnergyServerRpc(1);
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(ulong clientId, Vector3 fireDir, Vector3 firePos)
    {
        BulletManager.Singleton.SpawnBulletServerRpc(clientId, fireDir, firePos);
    }

    private void UpdateEnergyDisplay()
    {
        energyTMP.SetText(energy.Value.ToString());
    }

    public void GainEnergy(ushort inputEnergy)
    {
        if (!IsServer) return;
        energy.Value += inputEnergy;
        rb.transform.localScale += inputEnergy * 0.1f * Vector3.one;
    }

    public void ReduceEnergy(ushort inputEnergy)
    {
        if (!IsServer) return;
        if (energy.Value > inputEnergy)
        {
            energy.Value -= inputEnergy;
            rb.transform.localScale -= inputEnergy * 0.1f * Vector3.one;
        }
        else
        {
            energy.Value = 0;
            rb.transform.localScale = Vector3.zero;
        }
    }

    [ServerRpc]
    public void ReduceEnergyServerRpc(ushort inputEnergy)
    {
        // if (!IsServer) return;
        if (energy.Value > inputEnergy)
        {
            energy.Value -= inputEnergy;
            rb.transform.localScale -= inputEnergy * 0.1f * Vector3.one;
        }

        else
        {
            energy.Value = 0;
            rb.transform.localScale = Vector3.zero;
        }
    }
    void UpdateScale(ushort currentEnergy)
    {
        rb.transform.localScale = currentEnergy * 0.1f * Vector3.one;
    }

}