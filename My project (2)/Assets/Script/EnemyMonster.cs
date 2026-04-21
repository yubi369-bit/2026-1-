using UnityEngine;

public class EnemyMonster : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectRange = 5f;

    public int maxHP = 1;
    private int currentHP;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player =
            GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
    }

    void Update()
    {
        if (player == null) return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distance > detectRange)
        {
            rb.linearVelocity =
                new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float dir =
            Mathf.Sign(
                player.position.x - transform.position.x
            );

        rb.linearVelocity =
            new Vector2(
                dir * moveSpeed,
                rb.linearVelocity.y
            );
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}