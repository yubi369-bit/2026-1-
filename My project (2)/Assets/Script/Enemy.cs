using UnityEngine;

public class Enemy : MonoBehaviour
{
    //hp쪽
    public int maxHP = 3;
    private int currentHP;

    //애니메이션 쪽
    private Animator animator; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // ⭐ 맞았을 때 애니 실행
        animator.SetTrigger("hit");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
