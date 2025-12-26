// Damage.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Damage : Player, IPunObservable
{
    private Controll controll; // Assuming this exists elsewhere
    public float netCurHP;
    public float netMaxHP;

    void Start()
    {
        base.Start();
        controll = GetComponent<Controll>();
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

        else if (other.CompareTag("Arrow"))
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

        if (damage < 7.5f)
        {
            // controll.AddState(PlayerStats.Sstun);
        }
        else
        {
            // controll.AddState(PlayerStats.Lstun);
        }

        Debug.Log($"Damage taken: {damage} | CurHP: {CurHP} / {MaxHP}");
    }
}