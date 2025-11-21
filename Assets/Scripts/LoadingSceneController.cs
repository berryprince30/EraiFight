using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoadingSceneController : MonoBehaviour
{
    public string[] Tips;
    public Image progressBar;
    public static string nextScene;
    public static void LoadScene(string i) // 용례 : LoadingSceneController.LoadScene("Scene Name")
    {
        nextScene = i;
        PhotonNetwork.LoadLevel("Loading");
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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
                progressBar.fillAmount = Aop.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(Aop.progress >= 1.0f || progressBar.fillAmount >= 1.0f)
                {
                    Aop.allowSceneActivation = true; // 로딩 제한 건거 풀기
                    yield break;
                }
            }
        }
        yield return null;
    }
}
