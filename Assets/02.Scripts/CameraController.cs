using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 200f; //마우스감도

    private float MouseY;
    private float MouseX;

    void Update()
    {
        Rotate();
        UpdateCameraPosition();
    }
    private void Rotate()
    {

        MouseX += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;

        MouseY -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        MouseY = Mathf.Clamp(MouseY, -90f, 90f); //Clamp를 통해 최소값 최대값을 넘지 않도록함

        transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f);// 각 축을 한꺼번에 계산
    }

    private void UpdateCameraPosition()
    {
        transform.position = player.position + transform.up * 5;
    }
}
//public class CameraController : MonoBehaviour
//{
//    public Transform player;
//    public float mouseSensitivity = 200f; // 마우스 감도
//    public float cameraWidth = -23f;
//    public float cameraHeight = 4f;
//    public float camera_fix = 3f; //카메라 충돌보정 수치
//    public LayerMask collisionLayers; // 충돌 감지를 위한 레이어

//    Vector3 dir;

//    private float MouseY;
//    private float MouseX;
//    private float cameraDist;

//    void Start()
//    {
//        cameraDist = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight);
//        dir = new Vector3(0, cameraHeight, cameraWidth).normalized;
//    }

//    void Update()
//    {
//        Rotate();
//        UpdateCameraPosition();
//    }

//    private void Rotate()
//    {
//        MouseX += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
//        MouseY -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
//        MouseY = Mathf.Clamp(MouseY, -90f, 90f); // Clamp를 통해 최소값 최대값을 넘지 않도록 함

//        transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f); // 각 축을 한꺼번에 계산
//    }

//    private void UpdateCameraPosition()
//    {
//        transform.position = player.position + Vector3.up;

//        Vector3 rayTarget = transform.up * cameraHeight + transform.forward * cameraWidth;

//        RaycastHit hitInfo;
//        if (Physics.Raycast(transform.position, rayTarget, out hitInfo, cameraDist, collisionLayers))
//        {
//            Camera.main.transform.position = hitInfo.point;
//            Camera.main.transform.Translate(dir * -1 * camera_fix);

//        }
//        else
//        {
//            Camera.main.transform.localPosition = Vector3.zero;
//            Camera.main.transform.Translate(dir * cameraDist);
//            Camera.main.transform.Translate(dir * -1 * camera_fix);
//        }
//    }
//}