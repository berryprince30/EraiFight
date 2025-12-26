using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Items : MonoBehaviourPun
{
    public int ItemIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Body")
        {
            if(ItemIndex == 0)
            {
                Item1(other.gameObject);   
            }
            else if(ItemIndex == 1)
            {
                Item2(other.gameObject);
            }
            else if(ItemIndex == 2)
            {
                Item3(other.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void Item1(GameObject gameObject) // 힐
    {
        Damage damage = gameObject.GetComponent<Damage>();
        damage.CurHP += 15;
        damage.netCurHP += 15;
        if(damage.CurHP > damage.MaxHP)
        {
            damage.CurHP = damage.MaxHP;
        }
        if(damage.netCurHP > damage.netMaxHP)
        {
            damage.netCurHP = damage.netMaxHP;
        }
    }

    void Item2(GameObject gameObject) // 점프강화
    {
        Controll controll = gameObject.GetComponent<Controll>();
        StartCoroutine(ReinforceJump(controll));
    }

    void Item3(GameObject gameObject) // 이속 증가
    {
        Controll controll = gameObject.GetComponent<Controll>();
        StartCoroutine(ReinforceSpeed(controll));
    }

    IEnumerator ReinforceJump(Controll controll)
    {
        controll.jumpPower += 7;

        yield return new WaitForSeconds(7.5f);

        controll.jumpPower -= 7;

        yield return null;
    }

    IEnumerator ReinforceSpeed(Controll controll)
    {
        controll.moveSpeed += 3.5f;

        yield return new WaitForSeconds(7.5f);

        controll.moveSpeed -= 3.5f;

        yield return null;
    }
}
