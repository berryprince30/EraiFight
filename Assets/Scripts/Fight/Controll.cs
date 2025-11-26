using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Controll : MonoBehaviourPunCallbacks, IPunObservable
{
    // 이동, 점프, 피격, 기본공격만, 스킬은 따로
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    PhotonView PV;

    // Networking values
    Vector3 networkPos;
    float networkSpeedX;
    bool networkFlip;

    // Movement values
    public float moveSpeed = 5f;
    public float jumpPower = 12f;
    bool isGround;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 내가 보내는 값
        {
            stream.SendNext(transform.position);
            stream.SendNext(rigid.linearVelocity.x); // Unity6.2 Rigidbody2D는 linearVelocity 사용
            stream.SendNext(spriteRenderer.flipX);
        }
        else // 남이 보낸 값 수신
        {
            networkPos = (Vector3)stream.ReceiveNext();
            networkSpeedX = (float)stream.ReceiveNext();
            networkFlip = (bool)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (PV.IsMine)
        {
            Walk();
            Jump();
        }
        else
        {
            SyncRemotePlayer();
        }
    }

    // ============================
    //        LOCAL CONTROL
    // ============================

    void Walk()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rigid.linearVelocity = new Vector2(h * moveSpeed, rigid.linearVelocity.y);

        if (h != 0)
            spriteRenderer.flipX = h < 0;

        anim.SetFloat("Speed", Mathf.Abs(h));
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
    }

    // 바닥 체크
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGround = false;
    }

    // ============================
    //     REMOTE PLAYER SYNC
    // ============================

    void SyncRemotePlayer()
    {
        // 위치 보간 (부드럽게)
        transform.position = Vector3.Lerp(transform.position, networkPos, Time.deltaTime * 10f);

        // 속도 기반 애니메이션
        anim.SetFloat("Speed", Mathf.Abs(networkSpeedX));

        // 방향 동기화
        spriteRenderer.flipX = networkFlip;
    }
}