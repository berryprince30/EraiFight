using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoadingSceneController : MonoBehaviourPunCallbacks
{
    public string[] Tips;
    public Image progressBar;
    public static string nextScene;
    private bool isLocalLoadingComplete;
    int readyPlayers = 0;
    int totalPlayers = 2;
    public static void LoadScene(string i) // 용례 : LoadingSceneController.LoadScene("Scene Name")
    {
        nextScene = i;
        PhotonNetwork.LoadLevel("Loading");
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        isLocalLoadingComplete = false;
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation Aop = SceneManager.LoadSceneAsync(nextScene);
        Aop.allowSceneActivation = false; // 일부러 로딩 막기

        float timer = 0f;
        while(!Aop.isDone)
        {
            yield return null;

            if(Aop.progress < 0.9f)
            {
                progressBar.fillAmount = Aop.progress / 0.9f;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer * 0.5f);

                if (Aop.progress >= 0.9f && !isLocalLoadingComplete)
                {
                    isLocalLoadingComplete = true;
                    photonView.RPC("PlayerReady", RpcTarget.MasterClient);
                }

                // 모든 플레이어가 준비됐고, 내가 마스터면 활성화
                if (readyPlayers >= totalPlayers && PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("ActivateScene", RpcTarget.All);
                }
            }
        }
        yield return null;
    }

    [PunRPC]
    void PlayerReady()
    {
        readyPlayers++;
    }

    [PunRPC]
    void ActivateScene()
    {
        SceneManager.LoadSceneAsync(nextScene).allowSceneActivation = true; // 강제 활성화
        // 또는 기존 Aop이 살아있으면 Aop.allowSceneActivation = true;
    }
}
