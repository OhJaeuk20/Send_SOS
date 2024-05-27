using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void FixedUpdate()
    {
        fallCheck();
    }

    private void fallCheck()
    {
        fallvelocity = rb.velocity.y;
        Vector3 rayPoint = characterBody.transform.position + Vector3.up;
        Vector3 rayDir = -characterBody.transform.up;

        Debug.DrawRay(rayPoint, rayDir * 10, Color.red);

        if (Physics.Raycast(rayPoint, rayDir, out RaycastHit hit, 10f, 1 << 8))
        {
            if (hit.transform.CompareTag("BOUNCE_PAD"))
            {
                isFall = false;
                anim.SetBool("IsFall", isFall);
            }
        }
        else
        {
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

    void PostUpdate()
    {
        m_prevPosY = transform.position.y;
    }

    void WaketoStunned()
    {
        isStun = false;
        state = AnimState.IDLE;
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isMove = moveInput.magnitude != 0;

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

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("GROUND"))
        {
            isAir = false;

            if (isFall)
            {
                isFall = false;
                isAir = false;
                state = AnimState.CRASH;
                isStun = true;
            }
            else
            {
                isFall = false;
                isAir = false;
                state = AnimState.LAND;
            }
        }
        else if (coll.CompareTag("BOUNCE_PAD"))
        {
            isFall = false;
            isAir = true;
            fallvelocity = 0;
            isMove = false;
            state = AnimState.JUMP;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAir && !isStun)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isAir = true;
            isFall = false;
            state = AnimState.JUMP;
        }
    }
}
