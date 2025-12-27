// Damage.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Damage : Player, IPunObservable
{
    Controll controll; // Assuming this exists elsewhere
    Animator anim;
    Rigidbody2D rigid;
    public float netCurHP;
    public float netMaxHP;
    public int PIndex;

    void Start()
    {
        base.Start();
        controll = GetComponent<Controll>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        netCurHP = CurHP;
        netMaxHP = MaxHP;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)  // Local player sends data
        {
            stream.SendNext(CurHP);
            stream.SendNext(MaxHP);
        }
        else  // Remote clients receive and apply data
        {
            netCurHP = (float)stream.ReceiveNext();
            netMaxHP = (float)stream.ReceiveNext();
        }
    }

    void Update()
    {
        // Add death check if needed
        if (photonView.IsMine && CurHP <= 0)
        {
            P_anim.SetTrigger("Die");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return; // Only process on the local owner (victim)

        if (other.CompareTag("Attack"))
        {
            // Ensure it's not self-attack (e.g., own hitbox)
            if (other.transform.root != transform.root) // Or check photonView.Owner
            {
                AttackData attackData = other.GetComponent<AttackData>();
                if (attackData != null)
                {
                    GetDamage(attackData.damageAmount);
                }
            }
        }

        if (other.CompareTag("Arrow"))
        {
            // Ensure it's not self-attack (e.g., own hitbox)
            if (other.transform.root != transform.root) // Or check photonView.Owner
            {
                AttackData attackData = other.GetComponent<AttackData>();
                if (attackData != null)
                {
                    GetDamage(attackData.damageAmount);
                    Destroy(other.gameObject);
                }
            }
        }
    }

    public void GetDamage(float damage)
    {
        CurHP -= damage;
        CurHP = Mathf.Clamp(CurHP, 0, MaxHP); // Prevent negative HP

        StartCoroutine(GetStun());

        PhotonNetwork.Instantiate("HitParticle", transform.position, Quaternion.identity);
    }

    IEnumerator GetStun()
    {
        controll.AddState(PlayerStats.Sstun);
        anim.SetTrigger("Sstun");
        GetGetStun();
        
        yield return new WaitForSeconds(0.3f);

        controll.RemoveState(PlayerStats.Sstun);
    }

    void GetGetStun()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        int dir;

        if(sr.flipX) dir = 1;
        else dir = -1;

        rigid.linearVelocity = new Vector2(0, rigid.linearVelocity.y); // 기존 관성 제거
        rigid.AddForce(Vector2.right * dir * 5, ForceMode2D.Impulse);
    }
}