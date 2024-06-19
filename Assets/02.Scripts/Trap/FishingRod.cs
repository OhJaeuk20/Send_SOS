using UnityEngine;
using System.Collections;

public class FishingRod : MonoBehaviour
{
    public Transform fishingRodEnd; // 낚싯대 끝 부분 (자식 오브젝트)
    public Transform targetPosition; // 플레이어를 이동시킬 위치
    public float holdDuration = 2f; // 플레이어를 잡아둘 시간
    public float moveSpeed = 5f; // 낚싯대가 이동할 속도

    private bool playerCaught = false;
    private GameObject caughtPlayer;
    private float holdTimer = 0f;
    private Vector3 originalPosition;

    // 새로운 변수 추가: 현재 낚싯대가 움직이고 있는지 여부
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position; // 낚싯대의 원래 위치를 저장
    }

    public void CatchPlayer(GameObject player)
    {
        if (!playerCaught && !isMoving) // 움직이는 중이 아닐 때만 플레이어를 잡을 수 있도록
        {
            playerCaught = true;
            caughtPlayer = player;

            // 플레이어의 움직임 제한
            Rigidbody rb = caughtPlayer.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // 물리 작동을 비활성화하여 위치를 고정
                rb.velocity = Vector3.zero; // 속도 초기화
                rb.angularVelocity = Vector3.zero; // 각속도 초기화
            }

            // 플레이어를 낚싯대 끝 좌표계로 이동시킴
            caughtPlayer.transform.position = fishingRodEnd.position;
            caughtPlayer.transform.SetParent(fishingRodEnd);

            // 플레이어의 위치를 항상 낚싯대 끝 부분에 고정
            caughtPlayer.transform.localPosition = Vector3.zero;

            // 플레이어의 입력 제한 설정
            SetPlayerInputEnabled(true);

            holdTimer = holdDuration;
        }
    }

    void SetPlayerInputEnabled(bool isCaught)
    {
        PlayerMove playerMove = caughtPlayer.GetComponent<PlayerMove>();
        if (playerMove != null)
        {
            playerMove.SetCaughtState(isCaught);
        }
    }

    void Update()
    {
        if (playerCaught && !isMoving) // 움직이는 중이 아닐 때만 업데이트
        {
            holdTimer -= Time.deltaTime;
            if (holdTimer <= 0f)
            {
                StartCoroutine(MoveToTarget());
            }
        }
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true; // 움직임 시작

        // 플레이어의 물리 효과를 비활성화하여 위치를 고정시킴
        Rigidbody rb = caughtPlayer.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero; // 속도 초기화
            rb.angularVelocity = Vector3.zero; // 각속도 초기화
        }

        // 낚싯대를 목표 위치로 이동시킴
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 플레이어를 낚싯대 끝 좌표에서 떨어뜨림
        caughtPlayer.transform.SetParent(null); // 부모 설정 해제

        // 낚싯대를 원래 위치로 텔레포트
        transform.position = originalPosition;

        // 플레이어의 물리 효과를 다시 활성화하여 움직임을 허용
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        isMoving = false; // 움직임 종료
        playerCaught = false; // 플레이어가 다시 잡힐 수 있도록 초기화

        // 플레이어의 입력 제한 해제
        SetPlayerInputEnabled(false);
    }
}
