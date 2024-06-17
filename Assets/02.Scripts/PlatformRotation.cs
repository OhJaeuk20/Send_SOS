using UnityEngine;

public class PlatformRotation : MonoBehaviour
{
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    public RotationDirection rotationDirection;
    public float rotationSpeed = 50f; // 회전 속도 (degree per second)
    public bool rotateAroundX, rotateAroundY, rotateAroundZ; // 각 축 주위 회전 여부

    void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // 선택된 축 주위로 회전
        if (rotateAroundX)
        {
            RotateAroundAxis(transform.right, rotationAmount);
        }
        if (rotateAroundY)
        {
            RotateAroundAxis(transform.up, rotationAmount);
        }
        if (rotateAroundZ)
        {
            RotateAroundAxis(transform.forward, rotationAmount);
        }
    }

    void RotateAroundAxis(Vector3 axis, float amount)
    {
        // 회전 방향에 따라 회전 방향 설정
        float direction = (rotationDirection == RotationDirection.Clockwise) ? 1f : -1f;
        transform.Rotate(axis, direction * amount, Space.World); // World 축 기준 회전으로 변경
    }
}
