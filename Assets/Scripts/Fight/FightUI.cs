// FightUI.cs (mostly unchanged, but ensure it's not Photon-dependent unless needed; UI is local)
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FightUI : MonoBehaviourPunCallbacks
{
    public Image HP_1;
    public Image HP_2;
    public TMP_Text Seconds;
    public float TimeMax;
    private List<Damage> players = new List<Damage>();
    private bool gameOver = false;

    // End Phase
    public GameObject WinPanel;
    public TMP_Text WhoWin;
    public Image WinImg;
    public Image LoseImg;
    public Sprite[] WinLoseImgs;

    void Start()
    {
        Seconds.text = TimeMax.ToString("0");
        players = FindObjectsOfType<Damage>().ToList();
        players.Sort((a, b) => a.photonView.Owner.ActorNumber.CompareTo(b.photonView.Owner.ActorNumber));
    }

    void Update()
    {
        if (gameOver) return;

        // Time sync: For simplicity, run locally; for precision, master client could RPC updates
        TimeMax -= Time.deltaTime;
        Seconds.text = TimeMax.ToString("0");

        if (TimeMax <= -987)
        {
            Win987();
        }

        if (players.Count == 2)
        {
            HP_1.fillAmount = GetHPFill(players[0]);
            HP_2.fillAmount = GetHPFill(players[1]);

            // Check for winner
            if (players[0].CurHP <= 0 || players[0].netCurHP <= 0 
            || players[1].CurHP <= 0 || players[1].netCurHP <= 0)
            {
                EndPhase();
            }
        }
        else
        {
            players = FindObjectsOfType<Damage>().ToList();
            players.Sort((a, b) => a.photonView.Owner.ActorNumber.CompareTo(b.photonView.Owner.ActorNumber));
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
        gameOver = true;
        Time.timeScale = 0; // Caution: In multiplayer, this affects local only; use Photon to sync game state
        Seconds.text = "+987";
        GameSet();
    }

    void GameSet()
    {
        Debug.Log("[987이 마이너스가 되는 결론에는 절대 도달할 수 없다.]");
    }

    void EndPhase()
    {
        WinPanel.SetActive(true);
        gameOver = true;
        if (players[0].CurHP <= 0 || players[0].netCurHP <= 0)
        {
            // Player 2 wins
            PhotonView winnerPV = players[1].GetComponent<PhotonView>();
            Debug.Log("Player 2 Wins");
            WhoWin.text = winnerPV.Owner.NickName + " Wins!";
        }
        else if (players[1].CurHP <= 0 || players[1].netCurHP <= 0)
        {
            // Player 1 wins
            PhotonView winnerPV = players[0].GetComponent<PhotonView>();
            Debug.Log("Player 1 Wins");
            WhoWin.text = winnerPV.Owner.NickName + " Wins!";
        }
        Time.timeScale = 0;
    }

    public void ReloadGame()
    {
        // 로컬 타임스케일 복구 (중요)
        Time.timeScale = 1f;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            DisconnectAndLoadSelect();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("[FightUI] Left Room");
        DisconnectAndLoadSelect();
    }

    void DisconnectAndLoadSelect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
            LoadSelectScene();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[FightUI] Disconnected: " + cause);
        LoadSelectScene();
    }

    void LoadSelectScene()
    {
        SceneManager.LoadScene("Select");
    }
}