using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float fallMultiplier = 2.5f;                         //최대 점프 파워
    public float lowJumpMultiplier = 2.0f;                      //최소 점프 파워

    public Rigidbody2D rb;                                        //플레이어 강체 확인

    public bool isGround = true;

    private Vector2 moveInput;
    public float moveSpeed = 3f;                               //이속
    public float jumpForce = 5.0f;
    private Animator myAnimator;
    public float dashSpeed = 14f;
    public float dashTime = 0.18f;

    private bool isDashing = false;
    private float dashTimer;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("move", false);
    }



    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;            //하강시 중력 증가
        }   
        else if(rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))                        //상승중 점프 버튼을 때면 낮게 점프
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //점프 기능
        if (Input.GetButtonDown("Jump") && isGround && !isDashing)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGround = false;
        }

        //점프 모션
        if (!isGround)
        {
            myAnimator.SetBool("isJump", true);
        }
        else
        {
            myAnimator.SetBool("isJump", false);
        }
        //움직임 애니메이션
        if (moveInput.x != 0)
        {
            myAnimator.SetBool("move", true);
        }
        else
        {
            myAnimator.SetBool("move", false);
        }


        //좌우 바꾸기,이동
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
            rb.linearVelocity =
        new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        // 대쉬 시작
        if (Input.GetKeyDown(KeyCode.LeftShift)
         && !isDashing
         && isGround)
        {
            isDashing = true;
            dashTimer = dashTime;
        }
        //대쉬 문
        if (isDashing)
        {
            myAnimator.SetBool("isDash", true);
        }
        else
        {
            myAnimator.SetBool("isDash", false);
        }
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            float direction = transform.localScale.x;

            rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);

            if (dashTimer <= 0)
            {
                isDashing = false;
            }

            return; // 대쉬 중에는 일반 이동 막기
        

       

            //공격
            if (Input.GetKeyDown(KeyCode.V))
        {
            myAnimator.SetTrigger("attack");
        }

        

        
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

}
