using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField NicknameInput;
    public TMP_InputField RoomNumberInput;
    public GameObject ConnectButton;
    
    public TMP_Text ConnectTXT;
    bool CanStart;

    public SelectManage selectManage;
    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.AutomaticallySyncScene = true;
        CanStart = false;
        // DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(NicknameInput.text) || RoomNumberInput.text.Length != 5)
        {
            Error_five();
            return;
        }


        if(!selectManage.IsSelect)
        {
            Error_Select_Null();
            return;
        }

        PhotonNetwork.ConnectUsingSettings();
        ConnectTXT.text = "Connecting...";
    }

    public override void OnConnectedToMaster()
    {
        string RoomNum = RoomNumberInput.text;
        string Nick = NicknameInput.text;

        PhotonNetwork.LocalPlayer.NickName = Nick;
        PhotonNetwork.JoinOrCreateRoom(RoomNum, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공!");
        Room currentRoom = PhotonNetwork.CurrentRoom;
        int playerCount = currentRoom.PlayerCount;
        
        Debug.Log(playerCount);

        if (playerCount >= 2)
        {
            Debug.Log("방이 꽉 찼습니다!");
            photonView.RPC("UpdateStart", RpcTarget.All);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 끊겼을 시 작동
    }

    public void Error_five()
    {
        // 다섯자 안되거나 넘으면 이거
        Debug.Log("에러 파이브");
    }

    public void Error_Select_Null()
    {
        // 선택 안하면 이거
        Debug.Log("에러 셀렉트 널");
    }

    public void StartGame()
    {
        if (!CanStart) return;

        photonView.RPC("SetNextSceneName", RpcTarget.AllBuffered, "Fight");

        LoadingSceneController.LoadScene("Fight"); // 자동동기화로 모두 같이 이동
        
    }

    [PunRPC]
    void SetNextSceneName(string sceneName)
    {
        LoadingSceneController.nextScene = sceneName;
    }

    [PunRPC]
    void UpdateStart()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            ConnectTXT.text = "Click To Start";
            CanStart = true;
        }
        else
        {
            ConnectTXT.text = "Wait For Start";
        }
    }
}
