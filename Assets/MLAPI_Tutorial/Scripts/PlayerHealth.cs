using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using Random = UnityEngine.Random;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<float> health = new NetworkVariable<float>(100f);
    private MeshRenderer[] _renderers;
    private CharacterController cc;
    
    private void Start()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
        cc = GetComponent<CharacterController>();

    }

    public void TakeDamage(float damage)
    {
        health.Value -= damage;
        if (health.Value <= 0)
        {
            health.Value = 100f;
            Vector3 pos = new Vector3(Random.Range(-10, 10), 4, Random.Range(-10, 10));
            RespawnClientRpc(pos);
        }
    }

    [ClientRpc]
    void RespawnClientRpc(Vector3 position)
    {
        StartCoroutine(Respawn(position));
    }

    IEnumerator Respawn(Vector3 position)
    {
        foreach (var meshRenderer in _renderers)
        {
            meshRenderer.enabled = false;
        }
        yield return new WaitForSeconds(1f);
        cc.enabled = false;
        transform.position = position;
        cc.enabled = true;
        
        foreach (var meshRenderer in _renderers)
        {
            meshRenderer.enabled = true;
        }
    }
}
