using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//NavMeshAgentコンポーネントがアタッチされていない場合アタッチ
[RequireComponent(typeof(NavMeshAgent))]

public class AiController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase
    }

    [SerializeField]
    [Tooltip("巡回する地点の配列")]
    private Transform[] waypointArray;

    [SerializeField]
    [Tooltip("追いかける対象")]
    private GameObject player;

    // NavMeshAgentコンポーネントを入れる変数
    private NavMeshAgent navMeshAgent;
    // 現在の目的地
    private int currentWaypointIndex = 0;
    // 敵の状態
    private EnemyState currentState;

    // プレイヤーを視界に捉える範囲
    private float sightRange = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgentを取得
        navMeshAgent = GetComponent<NavMeshAgent>();
        // 巡回を開始
        currentState = EnemyState.Patrol;
        navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                CheckForPlayer();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;
        }
    }

    // 巡回ロジック
    private void Patrol()
    {
        // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // 目的地の番号を１更新（右辺を剰余演算子にすることで目的地をループさせれる）
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointArray.Length;
            // 目的地を次の場所に設定
            navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
        }
    }

    // プレイヤーを追いかけるロジック
    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // プレイヤーが一定範囲外に出たら巡回に戻る
        if (distanceToPlayer > sightRange)
        {
            currentState = EnemyState.Patrol;
            navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
        }
    }

    // プレイヤーを検知するロジック
    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // プレイヤーが視認範囲に入ったら追跡を開始
        if (distanceToPlayer <= sightRange)
        {
            currentState = EnemyState.Chase;
        }
    }
}
