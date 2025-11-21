using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class Controll : MonoBehaviourPunCallbacks, IPunObservable 
{
    // 이동, 점프, 피격, 기본공격만, 스킬은 따로
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public PhotonView PV;

    Vector3 networkPos;
    float networkSpeed;
    bool networkFlip;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        string mynickname = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // 데이터 전달
    {
        if (stream.IsWriting)
        {
            // 내가 조종하는 캐릭터 → 값을 보냄
            stream.SendNext(transform.position);
            stream.SendNext(rigid.linearVelocity.x);
            stream.SendNext(spriteRenderer.flipX);
        }
        else
        {
            // 남의 캐릭 → 값을 받음
            networkPos = (Vector3)stream.ReceiveNext();
            networkSpeed = (float)stream.ReceiveNext();
            networkFlip = (bool)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update() // 실제 데이터 받아와서 연산 및 업데이트
    {
        if(PV.IsMine)
        {
            
        }
        else
        {
            
        }
    }
}
