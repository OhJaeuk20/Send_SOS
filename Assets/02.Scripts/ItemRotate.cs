using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // 회전 속도 (초당 회전 각도)

    void Update()
    {
        // 매 프레임마다 Y축을 기준으로 회전
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
