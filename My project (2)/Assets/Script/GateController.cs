using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    private Collider2D doorCollider;
    private Animator animator;

    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckEnemies();
    }

    void CheckEnemies()
    {
        // ⭐ Enemy 태그 가진 오브젝트 찾기
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        // ⭐ 적이 하나도 없으면 문 열기
        if (enemies.Length == 0)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        // Collider 끄기 (통과 가능)
        if (doorCollider != null)
        {
            doorCollider.isTrigger = true;
        }

        // 애니메이션 있으면 실행
        if (animator != null)
        {
            animator.SetTrigger("open");
        }

        // 이 함수 계속 반복 실행 안 되게
        enabled = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int currentIndex =
                SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentIndex + 1);
        }
    }
}