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

    public enum AnimState { IDLE, JUMP, LAND };
    public static AnimState state = AnimState.IDLE;

    public float moveSpeed = 5f;
    public int jumpPower = 10;
    public float sensitivity = 1f;

    private Rigidbody rb;
    private Animator anim;

    private float m_prevPosY = 0.0f;
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
        switch (state)
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
        }
        Jump();
        PostUpdate();
        if (isStun)
        {
            isStun = true;
            Invoke("WaketoStunned", 3.0f);
        }
    }

    private void FixedUpdate()
    {
        Move();
        fallCheck();
    }

    private void fallCheck() //바닥 방향으로 Ray
    {
        fallvelocity = rb.velocity.y;
        Vector3 rayPoint = characterBody.transform.position + Vector3.up;
        Vector3 rayDir = -characterBody.transform.up;

        Debug.DrawRay(rayPoint, rayDir * 10, Color.red);

        if (Physics.Raycast(rayPoint, rayDir, out RaycastHit hit, 10f, 1 << 8))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
        }
        else
        {
            Debug.Log("10의 거리 안에 바닥이 없음");
            if (fallvelocity < -30.0f)
            {
                isFall = true;
                anim.SetBool("IsFall", isFall);
            }
        }
    }

    void PostUpdate() //자신의 이전  Y좌표 저장
    {
        m_prevPosY = transform.position.y;
    }

    void WaketoStunned() //기절 해제
    {
        isStun = false;
        state = AnimState.IDLE;
    }

    private void Move() //캐릭터 움직임 제어
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;

        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (!isStun)
        {
            if (!isAir)
                anim.SetBool("IsMove", isMove);
            if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                characterBody.forward = moveDir;
                rb.MovePosition(transform.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("GROUND"))
        {
            isAir = false;

            if (isFall == true)
            {
                isFall = false;
                anim.SetBool("IsFall", isFall);
                isStun = true;
            }
            else
            {
                state = AnimState.LAND;
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAir)
            {
                if (!isStun)
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                    isAir = true;
                    anim.SetBool("IsMove", false);
                    state = AnimState.JUMP;
                }
            }
        }
    }
}
