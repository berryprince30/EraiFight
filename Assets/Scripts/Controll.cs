using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class Controll : MonoBehaviourPunCallbacks, IPunObservable
{
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public PhotonView PV;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        string mynickname = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine)
        {
            
        }
    }
}
