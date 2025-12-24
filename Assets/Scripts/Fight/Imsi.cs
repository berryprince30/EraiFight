using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class Imsi : MonoBehaviour // HP확인용 임시 스크립트
{
    private List<Damage> players = new List<Damage>();
    public TMP_Text HP_One;
    public TMP_Text HP_One_net;
    public TMP_Text HP_Two;
    public TMP_Text HP_Two_net;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = FindObjectsOfType<Damage>().ToList();
        players.Sort((a, b) => a.photonView.Owner.ActorNumber.CompareTo(b.photonView.Owner.ActorNumber));
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Count == 2)
        {
            HP_One.text = players[0].CurHP.ToString("0");
            HP_One_net.text = players[0].netCurHP.ToString("0");
            HP_Two.text = players[1].CurHP.ToString("0");
            HP_Two_net.text = players[1].netCurHP.ToString("0");
        }
        else
        {
            players = FindObjectsOfType<Damage>().ToList();
            players.Sort((a, b) => a.photonView.Owner.ActorNumber.CompareTo(b.photonView.Owner.ActorNumber));
        }
    }
}
