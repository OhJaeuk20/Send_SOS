using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    public Transform cameraArm;

    public enum AnimState { IDLE, JUMP, LAND, CRASH };
    public static AnimState state = AnimState.IDLE;

    public float moveSpeed = 5f;
    public int jumpPower = 10;
    public float sensitivity = 1f;

    private Rigidbody rb;
    private Animator anim;

    private float m_prevPosY = 0.0f;
    private bool isMove = false;
    private bool isAir = false;
    private bool isFall = false;
    private bool isStun = false;

    private float fallvelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = characterBody.GetComponent<Animator>();
        m_prevPosY = transform.position.y;
    }

    void Update()
    {
        switch (state) // 캐릭터 상태 결정
        {
            case AnimState.IDLE:
                anim.SetInteger("State", 0);
                break;
            case AnimState.JUMP:
                anim.SetInteger("State", 1);
                break;
            case AnimState.LAND:
                anim.SetInteger("State", 2);
                break;
            case AnimState.CRASH:
                anim.SetInteger("State", 3);
                break;
        }

        anim.SetBool("IsMove", isMove);
        anim.SetBool("IsFall", isFall);

        Move();
        Jump();
        PostUpdate();
        if (isStun)
        {
            isStun = true;
            Invoke("WaketoStunned", 3.0f);
        }
    }

    private void FixedUpdate() // 물리법칙에 영향 받는
    {
        fallCheck();
    }

    private void fallCheck() // 바닥 방향으로 Ray
    {
        fallvelocity = rb.velocity.y;
        Vector3 rayPoint = characterBody.transform.position + Vector3.up;
        Vector3 rayDir = -characterBody.transform.up;

        Debug.DrawRay(rayPoint, rayDir * 10, Color.red);

        if (Physics.Raycast(rayPoint, rayDir, out RaycastHit hit, 10f, 1 << 8))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
            if (hit.transform.CompareTag("BOUNCE_PAD")) // 바운스 패드와 충돌한 경우
            {
                isFall = false; // 낙하 상태 해제
                anim.SetBool("IsFall", isFall); // 애니메이션 상태 갱신
            }
        }
        else
        {
            Debug.Log("10의 거리 안에 바닥이 없음");
            if (fallvelocity < -30.0f)
            {
                isFall = true;
                anim.SetBool("IsFall", isFall);
            }
            else
            {
                isFall = false;
                state = AnimState.JUMP;
            }
        }
    }

    void PostUpdate() // 자신의 이전 Y좌표 저장
    {
        m_prevPosY = transform.position.y;
    }

    void WaketoStunned() // 기절 해제
    {
        isStun = false;
        state = AnimState.IDLE;
    }

    private void Move() // 캐릭터 움직임 제어
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMove = moveInput.magnitude != 0; // isMove 상태 업데이트

        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        if (!isStun)
        {
            if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                characterBody.rotation = Quaternion.Slerp(characterBody.rotation, targetRotation, sensitivity * 5f * Time.deltaTime);

                rb.MovePosition(transform.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                state = AnimState.IDLE;
            }
        }
    }

    private void OnTriggerEnter(Collider coll) // 캐릭터 발 밑 콜라이더 판정
    {
        if (coll.CompareTag("GROUND"))
        {
            Debug.Log("Ground!");
            isAir = false; // Ground와 충돌 시 isAir를 false로 설정

            if (isFall)
            {
                Debug.Log("CRASH!!!");
                isFall = false;
                isAir = false; // CRASH 상태이므로 isAir도 false로 설정
                state = AnimState.CRASH;
                isStun = true;
            }
            else
            {
                Debug.Log("LAND...");
                isFall = false;
                isAir = false;
                state = AnimState.LAND;
            }
        }
        else if (coll.CompareTag("BOUNCE_PAD"))
        {
            Debug.Log("Bounce!");
            isFall = false; // 낙하 상태 취소
            isAir = true;  // isAir를 true로 설정하여 공중 상태 유지
            fallvelocity = 0; // 속도를 초기화하여 일관된 점프 높이 보장
            //rb.AddForce(Vector3.up * jumpPower * 2f, ForceMode.Impulse);
            isMove = false;
            state = AnimState.JUMP;
        }
    }

    private void Jump() // 캐릭터 점프 구현
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAir && !isStun)
        {
            Debug.Log("Jump!!!");
            isMove = false;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isAir = true;
            isFall = false;
            state = AnimState.JUMP;
        }
    }
}
