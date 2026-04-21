using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Move")]
    public float moveSpeed = 2f;

    [Header("HP")]
    public int maxHP = 1;

    private int currentHP;

    private Transform targetPoint;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;

        // 시작 목표 설정
        targetPoint = pointB;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (pointA == null || pointB == null) return;

        float step = moveSpeed * Time.deltaTime;

        // ⭐ MoveTowards 방식 (가장 안정적)
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            step
        );

        // 방향 뒤집기
        if (targetPoint.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // ⭐ 도착 판정 (여기가 핵심 수정)
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            if (targetPoint == pointA)
                targetPoint = pointB;
            else
                targetPoint = pointA;
        }
    }

    // ⭐ 공격 맞으면 실행됨
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " 사망");

        Destroy(gameObject);
    }
}