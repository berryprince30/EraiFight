using UnityEngine;
using System.Collections;
using Photon.Pun;

public class MapManager : MonoBehaviourPunCallbacks
{
    [Header("Item Settings")]
    public string[] Items;

    [Header("Spawn Time")]
    public float minSpawnTime = 8f;
    public float maxSpawnTime = 22f;

    [Header("Spawn Area")]
    public Vector2 minPos;
    public Vector2 maxPos;

    void Start()
    {
        // MasterClient만 아이템 소환
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnItemsCoroutine());
        }
    }

    IEnumerator SpawnItemsCoroutine()
    {
        float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
        yield return new WaitForSeconds(waitTime);

        Vector3 spawnPos = new Vector3(
            Random.Range(minPos.x, maxPos.x), 8, 0f
        );

        PhotonNetwork.Instantiate(
            Items[Random.Range(0, Items.Length)],
            spawnPos,
            Quaternion.identity
        );
        
        StartCoroutine(SpawnItemsCoroutine());
    }
}
