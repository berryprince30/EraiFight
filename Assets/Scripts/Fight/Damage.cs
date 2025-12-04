using UnityEngine;
using UnityEngine.UI;
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
        if (stream.IsWriting)
        {
            stream.SendNext(CurHP);  // 로컬 HP 전송
        }
        else
        {
            CurHP = (float)stream.ReceiveNext();  // 원격 HP 수신 및 업데이트
            UpdateDamageBar();  // HP 수신 시 UI 업데이트 호출
        }
    }

    void Update()
    {
        
    }
    
    [PunRPC]
    public void ReceiveDamage(float damage)
    {
        CurHP -= damage;
        CurHP = Mathf.Clamp(CurHP, 0, MaxHP);  // HP 범위 제한
        if (damage < 7.5f)
        {
            //AddState(PlayerStats.Sstun);
        }
        else
        {
            //AddState(PlayerStats.Lstun);
        }
        UpdateDamageBar();  // 데미지 후 UI 업데이트
        if (CurHP <= 0)
        {
            //AddState(PlayerStats.Die);
        }
    }

    public void GetDamage(float damage)
    {
        if (!photonView.IsMine) return;  // 로컬 플레이어만 데미지 처리 시작
        photonView.RPC("ReceiveDamage", RpcTarget.All, damage);  // 모든 클라이언트에 데미지 브로드캐스트
    }

    private void UpdateDamageBar()
    {
        // MasterClient 여부 대신 photonView.ViewID나 태그로 플레이어 구분 (예: 플레이어1은 낮은 ViewID)
        // 가정: 플레이어1 (낮은 ViewID)이 HP_1, 플레이어2가 HP_2
        int myViewID = photonView.ViewID;
        FightUI fightUI = FindObjectOfType<FightUI>();
        if (fightUI != null)
        {
            if (myViewID < 1002)  // 예시: ViewID가 낮은 쪽을 플레이어1로 가정 (Photon 기본 ViewID 범위)
            {
                Image HP_1 = fightUI.GetComponentInChildren<Image>();
                HP_1.fillAmount = CurHP / MaxHP;
            }
            else
            {
                Image HP_2 = fightUI.GetComponentInChildren<Image>();
                HP_2.fillAmount = CurHP / MaxHP;
            }
        }
        Debug.Log($"HP 업데이트: {CurHP}/{MaxHP}");
    }
}
