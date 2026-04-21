using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.7f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded;
    private float moveInput;

    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        AttackInput();
    }

    void Move()
    {
        moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity =
            new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            0.2f,
            groundLayer
        );

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("공격 실행");

        Collider2D[] hitEnemies =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayers
            );

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("적 감지: " + enemy.name);

            EnemyMonster e =
                enemy.GetComponent<EnemyMonster>();

            if (e != null)
            {
                Debug.Log("데미지 적용");
                e.TakeDamage(attackDamage);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy 충돌");

            if (!isInvincible)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager
            .GetActiveScene().buildIndex
        );
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}