using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FightManage : MonoBehaviour
{
    public GameObject[] Char_Pres;
    int MasterIndex;
    int ClientIndex;
    public TMP_Text MasterNick;
    public TMP_Text ClientNick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CharSpwan();
        SetNickname();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharSpwan()
    {
        Vector3 Mvec = new Vector3(-5, 0, 0);
        Vector3 Cvec = new Vector3(5, 0, 0);
        if(PhotonNetwork.IsMasterClient) // -5 0 0
        {
            PhotonNetwork.Instantiate(Char_Pres[0].name, Mvec, Quaternion.identity);
        }
        else // 5 0 0
        {
            PhotonNetwork.Instantiate(Char_Pres[1].name, Cvec, Quaternion.identity);
        }
    }

    void SetNickname()
    {
        MasterNick.text = PhotonNetwork.PlayerList[0].NickName;
        ClientNick.text = PhotonNetwork.PlayerList[1].NickName;
    }
}
