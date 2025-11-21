using UnityEngine;
using UnityEngine.UI;
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
    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(NicknameInput.text) || RoomNumberInput.text.Length != 5)
        {
            Error_five();
            return;
        }
        PhotonNetwork.ConnectUsingSettings();
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

        if (playerCount >= 2)
        {
            Debug.Log("방이 꽉 찼습니다!");
        }
        // 둘다 들어올 시 씬 넘어가기
        // 동기화 뭐시기 로딩 씬
        // 스폰하는게 그냥 Instantiate가 아니라 PhotonNetwork.Instantiate임.
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 끊겼을 시
    }

    void Update()
    {
        // PhotonNetwork.Disconnect();
    }

    public void Error_five()
    {
        // 다섯자 안되거나 넘으면 이거
        Debug.Log("에러 파이브");
    }
}
