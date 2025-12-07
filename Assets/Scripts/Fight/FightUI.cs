using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FightUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public Image HP_1;
    public Image HP_2;
    public TMP_Text Seconds;
    public float TimeMax;
    private List<Damage> players = new List<Damage>();
    void Awake()
    {
        Seconds.text = TimeMax.ToString("0");
    }

    void Start()
    {
        players = FindObjectsOfType<Damage>().ToList();
        players.Sort((a, b) => a.photonView.Owner.ActorNumber.CompareTo(b.photonView.Owner.ActorNumber));
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

        if (players.Count == 2)
        {
            HP_1.fillAmount = GetHPFill(players[0]);
            HP_2.fillAmount = GetHPFill(players[1]);
        }
    }

    private float GetHPFill(Damage playerDamage)
    {
        if (playerDamage == null) return 0f;

        if (playerDamage.photonView.IsMine)
        {
            return playerDamage.CurHP / playerDamage.MaxHP;
        }
        else
        {
            return playerDamage.netCurHP / playerDamage.netMaxHP;
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
