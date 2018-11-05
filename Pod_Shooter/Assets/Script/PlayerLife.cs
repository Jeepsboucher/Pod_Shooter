using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class PlayerLife : NetworkBehaviour {
    [SerializeField]
    public RectTransform healthBar;
    public const int maxLife = 100;
    private NetworkStartPosition[] spawnPoints;

    [SyncVar(hook ="UpdateLife")]
    int currentlife = maxLife;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
	}

    public void TakeDamage(int damage)
    {
        if (isServer)
        {
            currentlife -= damage;
            if(currentlife <= 0)
            {
                currentlife = 0;
                Die();
            }
        }
    }

    private void UpdateLife(int newLife)
    {
        healthBar.sizeDelta = new Vector2(newLife, healthBar.sizeDelta.y);
    }

    private void Die()
    {
        RpcReSpawn();
    }
    [ClientRpc]
    private void RpcReSpawn()
    {
        if (isLocalPlayer)
        {
            currentlife = maxLife;
            Vector3 spawnPoint = Vector3.zero;
            if(spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
