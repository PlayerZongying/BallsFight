using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Canvas inWorldCanvas;
    public Camera playerCamera;
    public BallsFightPlayerInputactions playerControls;
    private InputAction move;
    private InputAction fire;

    private Vector2 moveDirection = Vector2.zero;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // rb.isKinematic = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        moveDirection = move.ReadValue<Vector2>().normalized;

        // transform.position += speed * Time.deltaTime * new Vector3(moveDirection.x, 0, moveDirection.y);
        
        
        playerCamera.transform.position = rb.transform.position + Vector3.up * 20;
        inWorldCanvas.transform.position = rb.transform.position + Vector3.up * 3;
        inWorldCanvas.transform.rotation =
            Quaternion.LookRotation(playerCamera.transform.forward, playerCamera.transform.up);
        
    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;
        rb.velocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if(!IsOwner) return;
        print("Fire!!!");
    }
}