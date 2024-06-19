using System.Collections;
using UnityEngine;

public class E_TankCtrl : MonoBehaviour
{
    public enum State
    {
        PATROL, ATTACK
    }
    public State state = State.PATROL;

    public float attackDist = 100.0f;

    private Transform enemyTr;
    private Transform playerTr;
    private E_TankFire enemyFire;
    private WaitForSeconds ws;

    private void Awake()
    {
        enemyTr = GetComponent<Transform>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTr = player.GetComponent<Transform>();
        enemyFire = GetComponent<E_TankFire>();
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckEnemyState());
        StartCoroutine(EnemyAction());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CheckEnemyState()
    {
        while (true)
        {
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }

    IEnumerator EnemyAction()
    {
        while (true)
        {
            yield return ws;
            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    break;
                case State.ATTACK:
                    if (enemyFire.isFire == false)
                    {
                        enemyFire.isFire = true;
                    }
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyTr == null)
        {
            enemyTr = GetComponent<Transform>();
        }

        // 공격 범위
        Gizmos.color = new Color(1, 0, 0, 0.2f); // 반투명 빨간색
        Gizmos.DrawSphere(enemyTr.position, attackDist);
    }
}
