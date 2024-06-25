using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    public Transform cameraArm;

    public enum AnimState { IDLE, MOVE, JUMP, FALL, LAND, CRASH };
    public static AnimState state = AnimState.IDLE;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip crashSound;

    public float moveSpeed = 5f;
    public int jumpPower = 10;
    public float sensitivity = 1f;

    private Rigidbody rb;
    private Animator anim;
    private AudioSource audioSource;
    private GameObject BGManager;
    private GameObject USManager;

    private bool isMove = false;
    private bool isAir = false;
    private bool isFall = false;
    private bool isStun = false;
    private bool isCaught = false;

    private float fallvelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = characterBody.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        BGManager = GameObject.Find("BGM");
        USManager = GameObject.Find("UISound");
    }

    void Update()
    {
        Jump();
        if (isStun && !IsInvoking("WaketoStunned"))
        {
            StartCoroutine(WaketoStunned());
        }

        if (BGManager != null)
        {
            BGManager.transform.position = characterBody.transform.position;
        }
        if (USManager != null)
        {
            USManager.transform.position = characterBody.transform.position;
        }
    }

    private void FixedUpdate()
    {
        UpdateAnimationState();
        Move();
        fallCheck();
    }

    private void fallCheck()
    {
        fallvelocity = rb.velocity.y;
        if (fallvelocity < -50.0f && state != AnimState.CRASH)
        {
            isFall = true;
            state = AnimState.FALL;
        }
    }

    private void UpdateAnimationState()
    {
        switch (state)
        {
            case AnimState.IDLE:
                anim.SetInteger("State", 0);
                break;
            case AnimState.MOVE:
                anim.SetInteger("State", 1);
                break;
            case AnimState.JUMP:
                anim.SetInteger("State", 2);
                break;
            case AnimState.FALL:
                anim.SetInteger("State", 3);
                break;
            case AnimState.LAND:
                anim.SetInteger("State", 4);
                break;
            case AnimState.CRASH:
                anim.SetInteger("State", 5);
                break;
        }
    }

    IEnumerator WaketoStunned()
    {
        yield return new WaitForSeconds(3.0f);
        isStun = false;
        state = AnimState.IDLE;

        if (isMove)
        {
            state = AnimState.MOVE;
        }
        else
        {
            state = AnimState.IDLE;
        }
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
                if (!isAir)
                    state = AnimState.MOVE;
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

    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("GROUND") && rb.velocity.y <= 0)
        {
            isAir = false;
            isFall = false;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("GROUND"))
        {
            if (isFall)
            {
                isFall = false;
                isStun = true;
                state = AnimState.CRASH;
                PlaySound(crashSound);
                StartCoroutine(WaketoStunned());
            }
            else if (state != AnimState.CRASH)
            {
                state = AnimState.LAND;
                PlaySound(landSound);
            }
        }
        else if (coll.CompareTag("BOUNCE_PAD"))
        {
            isFall = false;
            isAir = true;
            fallvelocity = 0;
            state = AnimState.JUMP;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAir && !isStun)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isAir = true;
            state = AnimState.JUMP;
            PlaySound(jumpSound);
        }
    }

    public void SetCaughtState(bool caught)
    {
        isCaught = caught;
    }
    public void SetFallSpeed(float newFallSpeed)
    {
        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = newFallSpeed;
        rb.velocity = currentVelocity;
    }

    public void ResetFallSpeed()
    {
        rb.mass = 1; // or another property that defines falling speed
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}