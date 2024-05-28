using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    // 회전 속도를 조절하기 위한 변수
    public float rotationSpeed = 20.0f;

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        // y축을 중심으로 제자리 회전
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
