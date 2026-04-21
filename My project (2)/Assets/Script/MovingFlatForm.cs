using UnityEngine;

public class MovingFlatForm : MonoBehaviour
{
    public float moveHeight = 2f; //위 아래 이동 거리
    public float speed = 2f;    //발판 속도

    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * moveHeight;

        transform.position = new Vector3(startPos.x, startPos.y + y, startPos.z);
    }
}
