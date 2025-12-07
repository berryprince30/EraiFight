using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Damage : Player, IPunObservable
{
    Controll controll;
    public float netCurHP;
    public float netMaxHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();

        controll = GetComponent<Controll>();
        
        netCurHP = CurHP;
        netMaxHP = MaxHP;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)  // 로컬 플레이어가 데이터 전송
        {
            stream.SendNext(CurHP);
            stream.SendNext(MaxHP);
        }
        else  // 다른 클라이언트가 데이터 수신 및 적용
        {
            netCurHP = (float)stream.ReceiveNext();
            netMaxHP = (float)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float Damage)
    {
        CurHP -= Damage;
        if(Damage < 7.5)
        {
            //controll.AddState(PlayerStats.Sstun);
        }
        else
        {
            //controll.AddState(PlayerStats.Lstun); 
        }
        Debug.Log(CurHP + " | " + MaxHP + " | " + netCurHP + " | " + netMaxHP);
    }
}
