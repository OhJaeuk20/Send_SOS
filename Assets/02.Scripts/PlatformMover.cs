using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public enum AxisDirection
    {
        X,
        Y,
        Z
    }

    public AxisDirection moveAxis = AxisDirection.Y; // 이동할 축 방향
    public bool movePositiveDirection = true; // 이동할 방향 (true: 정방향, false: 역방향)
    public float speed = 2.0f; // 플랫폼 이동 속도
    public float distance = 3.0f; // 이동할 최대 거리

    private Vector3 startPosition;
    private Vector3 moveDirection;
    private float currentDistance = 0.0f;

    void Start()
    {
        // 시작 위치 저장
        startPosition = transform.position;

        // 이동 방향 벡터 설정
        switch (moveAxis)
        {
            case AxisDirection.X:
                moveDirection = movePositiveDirection ? Vector3.right : Vector3.left;
                break;
            case AxisDirection.Y:
                moveDirection = movePositiveDirection ? Vector3.up : Vector3.down;
                break;
            case AxisDirection.Z:
                moveDirection = movePositiveDirection ? Vector3.forward : Vector3.back;
                break;
        }
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;

        // 이동 방향과 거리에 따라 이동
        if (movePositiveDirection)
        {
            transform.Translate(moveDirection * movement);
            currentDistance += movement;
            if (currentDistance >= distance)
            {
                currentDistance = distance;
                movePositiveDirection = false;
            }
        }
        else
        {
            transform.Translate(-moveDirection * movement);
            currentDistance -= movement;
            if (currentDistance <= 0)
            {
                currentDistance = 0;
                movePositiveDirection = true;
            }
        }
    }
}
