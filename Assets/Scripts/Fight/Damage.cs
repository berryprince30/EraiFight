using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.InputSystem;

public class Damage : Player, IPunObservable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SetDamage()
    {
        photonView.RPC("checkUI", RpcTarget.AllBuffered, CurHP, MaxHP);
    }

    public void GetDamage(float Damage)
    {
        CurHP -= Damage;
        if(Damage < 7.5)
        {
            AddState(PlayerStats.Sstun); 
        }
        else
        {
            AddState(PlayerStats.Lstun); 
        }
        SetDamage();
    }
}
