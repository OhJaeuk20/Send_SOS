using UnityEngine;

public class Car_Obs : MonoBehaviour
{
    public Transform spawnPoint; // 시작지점
    public Transform destinationPoint; // 도착지점
    public GameObject objectToMove; // 이동할 오브젝트

    public float initialRespawnTime = 0f; // 초기 리스폰 시간
    public float respawnTime = 5f; // 일반 리스폰 시간
    public float speed = 2f; // 이동 속도
    public Vector3 rotation = new Vector3(0, 90, 0); // 오브젝트 회전 각도

    private GameObject currentObject; // 현재 이동 중인 오브젝트

    void Start()
    {
        Invoke("SpawnObject", initialRespawnTime);
    }

    void SpawnObject()
    {
        currentObject = Instantiate(objectToMove, spawnPoint.position, Quaternion.Euler(rotation));
        StartCoroutine(MoveObject());
    }

    System.Collections.IEnumerator MoveObject()
    {
        while (currentObject != null && Vector3.Distance(currentObject.transform.position, destinationPoint.position) > 0.1f)
        {
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, destinationPoint.position, speed * Time.deltaTime);
            yield return null;
        }

        if (currentObject != null)
        {
            Destroy(currentObject);
            yield return new WaitForSeconds(respawnTime);
            SpawnObject();
        }
    }
}
