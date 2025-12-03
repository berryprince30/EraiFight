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
    bool EndDownAttack = true;

    // 콤보 공격 관련
    int CheckComboFrame = 90;
    private List<(string input, int frame)> inputBuffer = new List<(string, int)>();
    private int currentFrame = 0;
    private Vector2 prevMoveInput;
    string[] seq3 = { "Z", "Up", "Left", "X"};
    string[] seq2 = { "Z", "Up", "Z" };
    string[] seq1 = { "Z", "Left", "Z" };
    
    void Start()
    {
        base.Start();

        HitBox = GetComponent<BoxCollider2D>();
        controll = GetComponentInParent<Controll>();
        anim = GetComponentInParent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (HitBox == null) 
        {
            HitBox = GetComponent<BoxCollider2D>();  // 재초기화 시도
        }

        if (stream.IsWriting)  // 로컬 플레이어가 데이터 전송
        {
            stream.SendNext(HitBox.offset);
            stream.SendNext(HitBox.size);
        }
        else  // 다른 클라이언트가 데이터 수신 및 적용
        {
            HitBox.offset = (Vector2)stream.ReceiveNext();
            HitBox.size = (Vector2)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        currentFrame++;

        Vector2 moveInput = controll.moveInput;
        bool inputAdded = false;

        // 방향 입력 press 감지 (threshold 0.5f로 입력 시작 감지)
        if (moveInput.x <= -0.5f && prevMoveInput.x > -0.5f)
        {
            AddToBuffer("Left");
            inputAdded = true;
        }
        if (moveInput.x >= 0.5f && prevMoveInput.x < 0.5f)
        {
            AddToBuffer("Right");
            inputAdded = true;
        }
        if (moveInput.y >= 0.5f && prevMoveInput.y < 0.5f)
        {
            AddToBuffer("Up"); // 위 화살표를 점프로 간주 (스페이스 바는 별도 입력으로 추가 필요 시 구현)
            inputAdded = true;
        }
        // Down은 콤보에 사용되지 않으므로 생략

        if (inputAdded)
        {
            CheckCombos();
        }

        prevMoveInput = moveInput;
    }

    // 평타
    public void OnAtk(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        
        if(context.performed)
        {
            AddToBuffer("Z");
            if (CheckCombos()) return;
            
            RemoveState(PlayerStats.Moving);
            RemoveState(PlayerStats.Guard);
            if(controll.moveInput.y >= -0.5f)
            {
                if(EndAttack)
                {
                    AddState(PlayerStats.Attacking);
                    AttackRoutine();
                    EndAttack = false;
                    Invoke("EndAttackTrue", 0.5f);
                }
            }
            else
            {
                if(EndDownAttack)
                {
                    AtkD();
                    EndDownAttack = false;
                    Invoke("EndDownAttackTrue", 0.5f);
                }
            }
        }
    }

    void Atk1()
    {
        Debug.Log("Attack1");
        anim.SetTrigger("Atk1");
        StartCoroutine(SetCollider(0.9f, 0.6f, 1.8f, 0.7f, 0.5f));
    }

    void Atk2()
    {
        Debug.Log("Attack2");
        anim.SetTrigger("Atk2");
        StartCoroutine(SetCollider(0.9f, 0.6f, 1.8f, 0.7f, 0.5f));
    }

    void Atk3()
    {
        Debug.Log("Attack3");
        anim.SetTrigger("Atk3");
        StartCoroutine(SetCollider(0.9f, 0.8f, 1.8f, 0.7f, 0.5f));
    }

    void EndAttackTrue()
    {
        EndAttack = true;
    }

    void EndDownAttackTrue()
    {
        EndDownAttack = true;
    }

    void AttackRoutine()
    {
        if(IsContainState(PlayerStats.Attacking))
        {
            switch (AttackIndex)
            {
                case 0: Atk1(); break;
                case 1: Atk2(); break;
                case 2: Atk3(); break;
                default: Debug.Log("씨1발 뭐ㅝ야"); break;
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

        yield return new WaitForSeconds(5f);

        RemoveState(PlayerStats.Attacking);
        AttackIndex = 0;
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
        if (!photonView.IsMine) return;

        if(context.performed)
        {
            AddToBuffer("X");
            if (CheckCombos()) return;

            if(!IsContainState(PlayerStats.Guard))
            {
                RemoveState(PlayerStats.Moving);
                RemoveState(PlayerStats.Attacking);
                AddState(PlayerStats.Guard);
                EndAttackTrue();
                EndDownAttackTrue();
                StopCoroutine(CancelAttack());

                Debug.Log("Guard");

                Invoke("CancelGuard", 0.75f); 
            }
        }
    }

    void CancelGuard()
    {
        Debug.Log("Guard Canceled");
        RemoveState(PlayerStats.Guard);
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

    // X 입력 (새로 추가: 콤보에 사용되는 x 버튼 입력)
    public void OnSkill(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;

        if (context.performed)
        {
            AddToBuffer("X");
            if (CheckCombos()) return; // 콤보 발동 시 skip (정상 스킬이 있으면 여기서 처리)
            // 정상 X 스킬이 없다면 아무것도 하지 않음 또는 필요 시 구현
            Debug.Log("Normal Skill X");
        }
    }

    // 입력 버퍼 추가 함수
    private void AddToBuffer(string input)
    {
        inputBuffer.Add((input, currentFrame));
        if (inputBuffer.Count > 10) // 버퍼 크기 제한
        {
            inputBuffer.RemoveAt(0);
        }
    }

    private bool CheckCombos() // 콤보 체크 함수 (긴 시퀀스 우선 체크)
    {
        // Cmd3: <- z x z 점프 ->
        if (CheckSequence(seq3))
        {
            Cmd3();
            inputBuffer.Clear();
            return true;
        }

        // Cmd2: <- <- 점프 z x
        if (CheckSequence(seq2))
        {
            Cmd2();
            inputBuffer.Clear();
            return true;
        }

        // Cmd1: z -> <- z
        if (CheckSequence(seq1))
        {
            Cmd1();
            inputBuffer.Clear();
            return true;
        }

        return false;
    }

    private bool CheckSequence(string[] sequence)
    {
        int len = sequence.Length;
        if (inputBuffer.Count < len) return false;

        for (int i = 0; i < len; i++)
        {
            if (inputBuffer[inputBuffer.Count - len + i].input != sequence[i])
                return false;
        }

        int startFrame = inputBuffer[inputBuffer.Count - len].frame;
        int endFrame = inputBuffer[inputBuffer.Count - 1].frame;
        return (endFrame - startFrame <= CheckComboFrame);
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

    IEnumerator SetCollider(float OfsX, float OfsY, float SizeX, float SizeY, float AttackTime)
    {
        yield return new WaitForSeconds(0.2f);

        SpriteRenderer sr = GetComponentInParent<SpriteRenderer>();
        int i = 0;

        if(sr.flipX) i = -1;
        else i = 1;

        HitBox.offset = new Vector2(OfsX * i, OfsY);
        HitBox.size = new Vector2(SizeX, SizeY);

        yield return new WaitForSeconds(AttackTime);

        HitBox.offset = new Vector2(0, 0);
        HitBox.size = new Vector2(0.1f, 0.1f);
    }

    void OnDrawGizmos()
    {
        if (HitBox != null)
        {
            Gizmos.color = Color.green;
            // 콜라이더의 월드 스페이스 위치 계산 (transform 적용)
            Vector3 center = transform.TransformPoint(HitBox.offset);
            Vector3 size = new Vector3(HitBox.size.x * transform.lossyScale.x, HitBox.size.y * transform.lossyScale.y, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}