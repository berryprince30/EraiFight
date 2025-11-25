using UnityEngine;
using UnityEngine.UI;

public class SelectManage : MonoBehaviour
{
    int SelectIndex;
    public Image SelectImage;
    public Sprite[] CharImages;
    public GameObject[] CharInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selct(int a)
    {
        SelectIndex = a;
        SelectImage.sprite = CharImages[SelectIndex];
    }

    public void LookCharInfo()
    {
        CharInfo[SelectIndex].SetActive(true);
    }

    public void CloseCharInfo()
    {
        CharInfo[SelectIndex].SetActive(false);
    }
}
