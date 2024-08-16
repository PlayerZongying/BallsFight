using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public ulong shooterClientID;
    public ushort damage = 2;
    public float speed = 10f;
    public Vector3 dir = Vector3.zero;

    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * dir;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        
        if (!other.CompareTag("Player") &&  !other.CompareTag("Wall") ) return;

        if(other.CompareTag("Player") && other.GetComponentInParent<NetworkObject>().OwnerClientId != shooterClientID)
        {
            PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
            if (pc)
            {
                pc.ReduceEnergy(damage);
                NetworkObject.Despawn();
            }
        }

        else if(other.CompareTag("Wall"))
        {
            NetworkObject.Despawn();
        }
        

    }
}
