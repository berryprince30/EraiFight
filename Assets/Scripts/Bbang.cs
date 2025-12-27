using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Bbang : MonoBehaviourPun
{
    public float moveSpeed;
    public Vector3 startPos;
    public Vector3 moveDirection = Vector3.right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(twoSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Dest")
        {
            Destroy(this.gameObject);
        }
    }

    public void SelectDirection(bool flip)
    {
        if(!flip)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
        }
    }

    IEnumerator twoSeconds()
    {
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);

        yield return null;
    }
}
