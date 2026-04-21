using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 3;

    private int currentHP;

    private Animator animator;

    void Start()
    {
        currentHP = maxHP;

        animator = GetComponent<Animator>();

        Debug.Log(gameObject.name + " HP 시작: " + currentHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(
            gameObject.name +
            " 데미지 받음, 현재 HP: " +
            currentHP
        );

        // ⭐ hit 애니메이션 실행
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " 파괴됨");

        Destroy(gameObject);
    }
}