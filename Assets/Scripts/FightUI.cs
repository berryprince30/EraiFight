using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FightUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public Image HP_1;
    public Image HP_2;
    public TMP_Text Seconds;
    public float TimeMax;
    void Awake()
    {
        Seconds.text = TimeMax.ToString("0");
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeMax -= Time.deltaTime;
        Seconds.text = TimeMax.ToString("0");
    }

    [PunRPC] 
    // 일시적으로 모든 컴에서 사용
    // 사용 예 : photonView.RPC("checkUI", RpcTarget.All, 60, 100);
    void checkUI(float CurHP1, float MaxHP1, float CurHP2, float MaxHP2) 
    {
        HP_1.fillAmount = CurHP1 / MaxHP1;
        HP_2.fillAmount = CurHP2 / MaxHP2;
    }
}
