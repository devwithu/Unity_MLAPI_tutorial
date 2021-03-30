using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;


public class PlayerShooting : NetworkBehaviour
{
    public ParticleSystem bulletParticleSystem;
    private ParticleSystem.EmissionModule em;

    private NetworkVariable<bool> shooting =
        new NetworkVariable<bool>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly},
            false);
    //private bool shooting = false;
    private float fireRate = 10f;

    private float shootTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        em = bulletParticleSystem.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            shooting.Value = Input.GetMouseButton(0);
            shootTimer += Time.deltaTime;
            if(shooting.Value && shootTimer >= 1f/fireRate)
            {
                shootTimer = 0;
                ShootServerRpc();
            }
        }
        em.rateOverTime = shooting.Value ? 10f : 0f;
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        Ray ray = new Ray(bulletParticleSystem.transform.position, bulletParticleSystem.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var player = hit.collider.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(10f);
            }
        }
    }
}
