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

        if(TimeMax <= -987)
        {
            Win987();
        }
    }

    
    // 일시적으로 모든 컴에서 사용
    // 사용 예 : photonView.RPC("checkUI", RpcTarget.All, 60, 100);
    [PunRPC]
    public void CheckUI(float CurHP, float MaxHP)
    {
        if (!photonView.IsMine) return;
        
        if(PhotonNetwork.IsMasterClient)
        {
            HP_1.fillAmount = CurHP / MaxHP;
        }
        else
        {
            HP_2.fillAmount = CurHP / MaxHP;   
        }
    }

    void Win987()
    {
        Time.timeScale = 0;
        Seconds.text = "+987";
        GameSet();
    }

    void GameSet()
    {
        Debug.Log("[987이 마이너스가 되는 결론에는 절대 도달할 수 없다.]");
    }
}
