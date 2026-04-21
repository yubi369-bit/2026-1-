using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f;   // 이동속도
    public int hp = 1;             // 체력
    public float detectRange = 5f; // 감지 거리

    Transform player;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Player 자동 찾기
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        DetectAndMove();
    }

    void DetectAndMove()
    {
        if (player == null) return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        // 감지 범위 안이면 추적
        if (distance <= detectRange)
        {
            float dir =
                player.position.x - transform.position.x;

            rb.linearVelocity = new Vector2(
                Mathf.Sign(dir) * moveSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            // 감지 범위 밖이면 멈춤
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );
        }
    }

    // 데미지 받기
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}