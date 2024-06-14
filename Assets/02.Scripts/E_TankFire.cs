using System.Collections;
using UnityEngine;

public class E_TankFire : MonoBehaviour
{
    public Transform turretTr;
    public Transform fireTr;
    public GameObject explosionPrefab; // 폭발 프리팹 추가
    public GameObject FirePrefab;
    public bool isFire = false;

    private AudioSource audio;
    private Transform playerTr;
    private float nextFire = 0.0f;
    private readonly float fireRate = 3.0f;
    private readonly float damping = 2.0f;

    private GameObject currentFireEffect; // 현재 발사 효과 인스턴스를 저장할 변수

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTr = player.GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isFire)
        {
            Quaternion rot = Quaternion.LookRotation((playerTr.position + Vector3.up) - turretTr.position);
            turretTr.rotation = Quaternion.Slerp(turretTr.rotation, rot, Time.deltaTime * damping);

            if (Time.time >= nextFire)
            {
                CreateFireEffect();
                CreateExplosion();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
        }
    }

    void CreateFireEffect()
    {
        Debug.Log("Fire!!!");
        Vector3 firePos = fireTr.position;
        // 기존 발사 효과가 있으면 삭제
        if (currentFireEffect != null)
        {
            Destroy(currentFireEffect);
        }
        // 발사 효과 생성
        currentFireEffect = Instantiate(FirePrefab, firePos, Quaternion.identity);
        // 발사 효과 재생이 끝나면 삭제
        ParticleSystem particleSystem = currentFireEffect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            Destroy(currentFireEffect, particleSystem.main.duration);
        }
        else
        {
            Debug.LogWarning("FirePrefab does not contain ParticleSystem component.");
        }
    }

    void CreateExplosion()
    {
        // 플레이어 근처에 폭발 프리팹 생성
        if (explosionPrefab != null)
        {
            Vector3 explosionPos = playerTr.position;
            Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
        }
    }
}
