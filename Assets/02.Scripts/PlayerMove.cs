using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    public Transform cameraArm;

    public enum AnimState { IDLE, JUMP,LAND };
    public static AnimState state = AnimState.IDLE;

    public float moveSpeed = 5f;
    public int jumpPower = 10;
    public float sensitity = 1f;

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
        cameraArm.position = new Vector3(characterBody.position.x, characterBody.position.y + 5, characterBody.position.z);

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
        LookAround();
        Jump();
        PostUpdate();
        if (isStun)
        {
            isStun = true;
            Invoke("WaketoStunned",3.0f);
        }
    }

    private void FixedUpdate()
    {
        Move();
        fallCheck();
    }
    
    private void fallCheck()
    {
        fallvelocity = rb.velocity.y;
        if (fallvelocity < -30.0f)
        {
            isFall = true;
            anim.SetBool("IsFall", isFall);
        }
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f) //카메라가 위로 회전하는 경우
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else //카메라가 아래로 회전하는 경우
        {
            x = Mathf.Clamp(x, 335f, 361f); 
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x * sensitity, camAngle.z);  
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
        bool isMove = moveInput.magnitude != 0;

        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (!isStun)
        {
            if(!isAir)
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
                if(!isStun)
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