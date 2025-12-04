using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Damage : Player, IPunObservable
{
    public PhotonView uiPhotonView;
    Controll controll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();

        uiPhotonView = FindObjectOfType<FightUI>().photonView;
        controll = GetComponent<Controll>();
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
        uiPhotonView.RPC("CheckUI", RpcTarget.All, CurHP, MaxHP);
        Debug.Log("씨발 뭐지");
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
