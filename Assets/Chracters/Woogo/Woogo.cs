using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.InputSystem;

public class Woogo : Player, IPunObservable
{
    float DamageF;
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

    void Cmd1() // z -> <- z
    {
        Debug.Log("Cmd1");
    }

    void Cmd2() // <- <- 점프(스패이스 바 || 위 화살표) z x
    {
        Debug.Log("Cmd2");
    }
    
    void Cmd3() //  <- z x z 점프(스패이스 바 || 위 화살표) ->
    {
        Debug.Log("Cmd3");
    }


    void Special() // 이건 냅둬
    {
        Debug.Log("Special");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Body"))
        {
            Debug.Log("닿았다 ㅎㅎ");
            Damage BodyDamage = collision.gameObject.GetComponent<Damage>();
            BodyDamage.GetDamage(DamageF);
        }
    }

    void SetCollider()
    {
        
    }
}
