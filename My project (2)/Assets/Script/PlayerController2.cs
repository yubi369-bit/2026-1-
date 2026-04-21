using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (attackPoint == null)
        {
            Debug.LogError("attackPoint 연결 안됨");
        }
    }

    void Update()
    {
        Move();
        Jump();
        Dash();
        AttackInput();
    }

    void Move()
    {
        if (isDashing) return;

        moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity =
            new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // ⭐ move = bool
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
                new Vector2(dir * dashSpeed, 0);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                isDashing = false;
                animator.SetBool("isDash", false);
            }
        }
    }

    // ⭐ 공격키 V
    void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("attack");
        }
    }

    // ⭐ 애니메이션 이벤트에서 실행
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
            EnemyMonster em =
                enemy.GetComponent<EnemyMonster>();

            if (em != null)
            {
                em.TakeDamage(attackDamage);
            }

            Enemy e =
                enemy.GetComponent<Enemy>();

            if (e != null)
            {
                e.TakeDamage(attackDamage);
            }
        }
    }

    // ⭐ Ground / Enemy 충돌
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

    // ⭐ Death 트리거
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Debug.Log("Death 태그 감지");

            if (!isInvincible)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");

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