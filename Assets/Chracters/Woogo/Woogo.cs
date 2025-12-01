using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.InputSystem;

public class Woogo : Player, IPunObservable
{
    float Damage;
    BoxCollider2D HitBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HitBox = GetComponent<BoxCollider2D>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAtk(InputAction.CallbackContext context)
    {
        Debug.Log("깃이그노어 병신");
        StartCoroutine(AttackCoroutine());
    }

    void Atk1()
    {
        
    }

    void Atk2()
    {
        
    }

    void Atk3()
    {
        
    }

    IEnumerator AttackCoroutine()
    {
        Debug.Log("ss");
        yield return null;
    }

    void AtkD()
    {
        
    }

    void Cmd1()
    {
        
    }

    void Cmd2()
    {
        
    }
    
    void Cmd3()
    {
        
    }


    void Special()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Body"))
        {
            Debug.Log("닿았다 ㅎㅎ");
        }
    }
}
