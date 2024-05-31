using UnityEngine;

public class PlatformUpDownMover : MonoBehaviour
{
    public float speed = 2.0f; // 플랫폼 이동 속도
    public float height = 3.0f; // 플랫폼이 이동할 최대 높이
    public bool startMovingUp = true; // 시작할 때 위로 이동할지 여부

    private Vector3 startPosition;
    private bool movingUp;

    void Start()
    {
        // 시작 위치 저장
        startPosition = transform.position;
        // 초기 이동 방향 설정
        movingUp = startMovingUp;
    }

    void Update()
    {
        // 플랫폼 이동 로직
        if (movingUp)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (transform.position.y >= startPosition.y + height)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= startPosition.y - height)
            {
                movingUp = true;
            }
        }
    }
}