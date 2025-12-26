using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MovingMan : MonoBehaviourPunCallbacks
{
    [Header("Movement Settings")]
    public Vector3 moveDirection = Vector3.right;
    public float moveDistance = 3f;
    public float moveSpeed = 1f;

    private Vector3 startPos;
    private bool canMove = false;

    void Start()
    {
        startPos = transform.position;
        moveDirection.Normalize();

        CheckPlayerCount();
    }

    void Update()
    {
        // MasterClient만 이동 계산
        if (!PhotonNetwork.IsMasterClient) return;
        if(!canMove)
        {
            CheckPlayerCount();   
        }
        else
        {
            float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            transform.position = startPos + moveDirection * offset;
        }
    }

    void CheckPlayerCount()
    {
        if (PhotonNetwork.InRoom &&
            PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            canMove = true;
        }
    }
}
