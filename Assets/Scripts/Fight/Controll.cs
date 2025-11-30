using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;

public class Controll : Player, IPunObservable
{
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    PhotonView PV;

    Vector3 networkPos;
    float networkSpeedX;
    bool networkFlip;

    // Input System 변수
    Vector2 moveInput;   // Move 액션에서 받음
    bool jumpInput;      // Jump 액션에서 받음

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

    // ---------------------------
    // Input System Callbacks
    // ---------------------------

    // PlayerInput 컴포넌트에서 자동 호출됨
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpInput = true;
    }

    // ---------------------------

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 보내야 할 거 : 콜라이더 | 속도 | 위치 | 플립 | 애니메이터 
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(rigid.linearVelocity.x);
            stream.SendNext(spriteRenderer.flipX);
        }
        else
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

    // ==============================
    //        LOCAL CONTROL
    // ==============================

    void Walk()
    {
        float h = moveInput.x;  // Input System 값

        rigid.linearVelocity = new Vector2(h * moveSpeed, rigid.linearVelocity.y);

        if (h != 0)
        {
            spriteRenderer.flipX = h < 0;
            anim.SetBool("Walk", true);   
        }
        else
        {
            anim.SetBool("Walk", false);  
        }

    }

    void Jump()
    {
        if (jumpInput && isGround)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpPower);
            anim.SetBool("Walk", false);
            anim.SetBool("Jump", true);
        }

        jumpInput = false; // 매 프레임 초기화
    }

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

    // ==============================
    //     REMOTE PLAYER SYNC
    // ==============================

    void SyncRemotePlayer()
    {
        transform.position = Vector3.Lerp(transform.position, networkPos, Time.deltaTime * 10f);

        // anim.SetFloat("Speed", Mathf.Abs(networkSpeedX));
        spriteRenderer.flipX = networkFlip;
    }
}
