using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerReal : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashTime = 0.2f;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.7f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;

    private Rigidbody2D rb;
    private Animator animator;

    private float moveInput;
    private bool isGround;

    private bool isDashing = false;
    private float dashTimer;

    private bool isInvincible = false;
    float invincibleTimer = 0f;
    public float invincibleDuration = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        Dash();
        AttackInput();
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                Debug.Log("무적 종료");
            }
        }
    }

    void Move()
    {
        if (isDashing) return;

        moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity =
            new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // move = bool
        animator.SetBool("move", moveInput != 0);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x, jumpForce);

            isGround = false;
        }

        animator.SetBool("isJump", !isGround);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashTime;

            animator.SetBool("isDash", true);
        }

        if (isDashing)
        {
            float dir = transform.localScale.x;

            rb.linearVelocity =
                new Vector2(dir * dashSpeed, rb.linearVelocity.y);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                isDashing = false;
                animator.SetBool("isDash", false);
            }
        }
    }

    // 공격키 V
    void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("attack");
        }
    }

    //공격에서 실행
    public void Attack()
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayers
            );

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("감지됨: " + enemy.name);

            Enemy e =
                enemy.GetComponentInParent<Enemy>();

            if (e != null)
            {
                e.TakeDamage(attackDamage);
            }

            EnemyMonster em =
                enemy.GetComponentInParent<EnemyMonster>();

            if (em != null)
            {
                em.TakeDamage(attackDamage);
            }

            EnemyPatrol ep =
                enemy.GetComponentInParent<EnemyPatrol>();

            if (ep != null)
            {
                ep.TakeDamage(attackDamage);
            }
        }
    }

    // Ground / Enemy 충돌
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                Die();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    // Death + Item 처리
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Death
        if (collision.CompareTag("Death"))
        {
            if (!isInvincible)
            {
                Die();
            }
        }

        if (collision.CompareTag("Item"))
        {
            Debug.Log("무적 아이템 먹음");

            isInvincible = true;
            invincibleTimer = 10f;

            Destroy(collision.gameObject);
        }


        // ⭐ Item1 = 이동속도 증가
        if (collision.CompareTag("Item1"))
        {
            Debug.Log("속도 증가 아이템");

            moveSpeed += 2f;

            Destroy(collision.gameObject);
        }

        // ⭐ Item2 = 점프력 증가
        if (collision.CompareTag("Item2"))
        {
            Debug.Log("점프 증가 아이템");

            jumpForce += 2f;

            Destroy(collision.gameObject);
        }
    }

    void Die()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}