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
        ConnectTXT.text = "Connecting...";
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

        if (playerCount >= 2 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("방이 꽉 찼습니다!");
            LoadingSceneController.LoadScene("Fight");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 끊겼을 시
        // StartScene으로 다시 로드
    }

    public void Error_five()
    {
        // 다섯자 안되거나 넘으면 이거
        Debug.Log("에러 파이브");
    }
}
