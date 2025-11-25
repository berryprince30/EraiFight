using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class LoadingSceneController : MonoBehaviourPunCallbacks
{
    public Image progressBar;
    public TMP_Text TipTXT;
    public string[] Tips;

    public static string nextScene;
    private bool isLocalLoadingComplete = false;

    int readyPlayers = 0;
    int totalPlayers;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        PhotonNetwork.LoadLevel("Loading");
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(LoadingFakeProgress());
        
        int a = Random.Range(0, 13);
        TipTXT.text = "Tip : "+ Tips[a];
    }

    IEnumerator LoadingFakeProgress()
    {
        float fakeProgress = 0f;

        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.25f; // 대충 가짜 로딩속도
            progressBar.fillAmount = fakeProgress;

            if (fakeProgress >= 0.95f && !isLocalLoadingComplete)
            {
                isLocalLoadingComplete = true;
                photonView.RPC("PlayerReady", RpcTarget.MasterClient);
            }

            yield return null;
        }
    }

    [PunRPC]
    void PlayerReady()
    {
        readyPlayers++;

        if (readyPlayers >= totalPlayers)
        {
            PhotonNetwork.LoadLevel(nextScene);
        }
    }
}
