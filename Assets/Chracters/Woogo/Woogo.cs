using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.InputSystem;

public class Woogo : Player, IPunObservable
{
    // 범용
    float DamageF;
    BoxCollider2D HitBox;
    Controll controll;
    Animator anim;

    // 평타 관련
    int AttackIndex = 0;
    bool Canceling = false;
    bool EndAttack = true;

    // 콤보 공격 관련
    int CheckComboFrame = 30;
    
    void Start()
    {
        HitBox = GetComponent<BoxCollider2D>();
        controll = GetComponentInParent<Controll>();
        anim = GetComponentInParent<Animator>();

        base.Start();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    void Update()
    {
        
    }

    // 평타
    public void OnAtk(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RemoveState(PlayerStats.Moving);
            RemoveState(PlayerStats.Guard);
            if(controll.moveInput.y >= 0 && EndAttack)
            {
                AttackRoutine();
                AddState(PlayerStats.Attacking);
                EndAttack = false;
                Invoke("EndAttackTrue", 0.33f);

            }
            else
            {
                AtkD();
            }
        }
    }

    void Atk1()
    {
        Debug.Log("Attack1");
        anim.SetTrigger("Atk1");
    }

    void Atk2()
    {
        Debug.Log("Attack2");
        anim.SetTrigger("Atk2");
    }

    void Atk3()
    {
        Debug.Log("Attack3");
        anim.SetTrigger("Atk3");
    }

    void EndAttackTrue()
    {
        EndAttack = true;
    }

    void AttackRoutine()
    {
        if(IsContainState(PlayerStats.Attacking))
        {
            switch (AttackIndex)
            {
                case 0:
                    Atk1();
                    break;
                case 1:
                    Atk2();
                    break;
                case 2:
                    Atk3();
                    break;
                default:
                    Atk1();
                    break;
            }
        }

        AttackIndex++;

        if(AttackIndex >= 3)
        {
            AttackIndex = 0;
        }

        if(!Canceling)
        {
            StartCoroutine(CancelAttack());   
        }
    }

    IEnumerator CancelAttack()
    {
        Canceling = true;

        yield return new WaitForSeconds(1.5f);

        RemoveState(PlayerStats.Attacking);
        Canceling = false;
        
        yield return null;
    }

    // 아래 공격
    void AtkD()
    {
        Debug.Log("Down");
        anim.SetTrigger("Skill1");
    }

    // 가드
    public void Guard(InputAction.CallbackContext context)
    {
        RemoveState(PlayerStats.Moving);
        RemoveState(PlayerStats.Attacking);
        AddState(PlayerStats.Guard);
        EndAttackTrue();
        StopCoroutine(CancelAttack());

        Debug.Log("Guard");

        Invoke("CancelGuard", 0.75f);
    }

    void CancelGuard()
    {
        Debug.Log("Guard Canceled");
    }



    void Cmd1() // z -> <- z
    {
        Debug.Log("Cmd1");
    }

    void Cmd2() // <- <- 점프(스패이스 바 || 위 화살표) z x
    {
        Debug.Log("Cmd2");
    }
    
    void Cmd3() // <- z x z 점프(스패이스 바 || 위 화살표) ->
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
