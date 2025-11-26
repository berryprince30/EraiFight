using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class SelectManage : MonoBehaviourPunCallbacks
{
    public int SelectIndex;
    public bool IsSelect;
    public Image SelectImage;
    public Sprite[] CharImages;
    public GameObject[] CharInfo;
    public GameObject CheckBtn;
    public GameObject ESCBtn;
    Keyboard kb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kb = Keyboard.current;
        IsSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (kb != null && kb.escapeKey.wasPressedThisFrame && ESCBtn.activeSelf)
        {
            CloseCharInfo();
        }
        
    }

    public void Selct(int a)
    {
        SelectIndex = a;
        SelectImage.sprite = CharImages[SelectIndex];
        CheckBtn.SetActive(true);
        IsSelect = true;
    }

    public void LookCharInfo()
    {
        CharInfo[SelectIndex].SetActive(true);
        ESCBtn.SetActive(true);
        CheckBtn.SetActive(false);
    }

    public void CloseCharInfo()
    {
        CharInfo[SelectIndex].SetActive(false);
        CheckBtn.SetActive(true);
        ESCBtn.SetActive(false);
    }
}
