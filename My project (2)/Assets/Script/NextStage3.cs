using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage3 : MonoBehaviour
{
    public string nextSceneTrigger; //stage3


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("3Stage");
        }
    }
}
