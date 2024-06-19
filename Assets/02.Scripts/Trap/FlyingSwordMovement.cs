using UnityEngine;

public class FlyingSwordMovement : MonoBehaviour
{
    // 시작 위치
    private Vector3 startPosition;

    // 날아가는 거리와 속도
    public float flyingDistance = 5.0f; // 날아가는 거리
    public float flyingSpeed = 5.0f; // 날아가는 속도

    // 회전 관련 변수
    private Quaternion startRotation; // 시작 회전
    private Quaternion targetRotation; // 목표 회전
    private float rotationAngle = 180.0f; // 회전 각도 (도)
    public float rotationSpeed = 90.0f; // 회전 속도 (각도/초)

    private bool isFlyingOut = true; // 날아가는 중인지 여부 (처음에는 날아가는 상태)

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (isFlyingOut)
        {
            // 1. 물체가 -z 축 방향으로 날아감
            Vector3 targetPosition = startPosition - transform.forward * flyingDistance;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flyingSpeed * Time.deltaTime);

            // 날아가기 완료 후 x 축 기준으로 180도 회전 상태로 전환
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                Rotate();
            }
        }
        else
        {
            // 3. 회전 진행
            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

            // 회전 완료 후 다시 처음 위치로 돌아가기 상태로 전환
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1.0f)
            {
                MoveBack();
            }
        }
    }

    // 2. x 축 기준으로 180도 회전하는 메서드
    private void Rotate()
    {
        isFlyingOut = false;
        // x 축 기준으로 180도 회전 설정
        targetRotation = Quaternion.Euler(0, rotationAngle, 0) * startRotation;
    }

    // 4. 처음 위치로 날아가는 상태로 전환하는 메서드
    private void MoveBack()
    {
        isFlyingOut = true;
        // 처음 위치로 이동
        Vector3 targetPosition = startPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, flyingSpeed * Time.deltaTime);

        // 처음 위치로 도달하면 초기 회전 상태로 돌아감
        if (transform.position == targetPosition)
        {
            transform.rotation = startRotation;
        }
    }
}
