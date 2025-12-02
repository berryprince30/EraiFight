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
    Controll controll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HitBox = GetComponent<BoxCollider2D>();
        controll = GetComponentInParent<Controll>();
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
        if(context.performed)
        {
            if(controll.moveInput.y >= 0)
            {
                StartCoroutine(AttackCoroutine());   
            }
            else
            {
                AtkD();
            }
        }
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
        Debug.Log("Down");
    }

    public void Guard(InputAction.CallbackContext context)
    {
        Debug.Log("Guard");
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

    void SetCollider()
    {
        
    }
}
