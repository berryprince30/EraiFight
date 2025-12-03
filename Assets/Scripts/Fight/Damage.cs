using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Damage : Player, IPunObservable
{
    public PhotonView uiPhotonView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();

        uiPhotonView = FindObjectOfType<FightUI>().photonView;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetDamageBar()
    {
        uiPhotonView.RPC("checkUI", RpcTarget.All, CurHP, MaxHP);
    }

    public void GetDamage(float Damage)
    {
        CurHP -= Damage;
        if(Damage < 7.5)
        {
            //AddState(PlayerStats.Sstun); 
        }
        else
        {
            //AddState(PlayerStats.Lstun); 
        }
        SetDamageBar();
    }
}
