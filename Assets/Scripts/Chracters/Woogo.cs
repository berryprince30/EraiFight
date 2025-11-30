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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Body"))
        {
            Debug.Log("닿았다 ㅎㅎ");
        }
    }
}
