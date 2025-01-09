using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//NavMeshAgent�R���|�[�l���g���A�^�b�`����Ă��Ȃ��ꍇ�A�^�b�`
[RequireComponent(typeof(NavMeshAgent))]

public class AiController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase
    }

    [SerializeField]
    [Tooltip("���񂷂�n�_�̔z��")]
    private Transform[] waypointArray;

    [SerializeField]
    [Tooltip("�ǂ�������Ώ�")]
    private GameObject player;

    // NavMeshAgent�R���|�[�l���g������ϐ�
    private NavMeshAgent navMeshAgent;
    // ���݂̖ړI�n
    private int currentWaypointIndex = 0;
    // �G�̏��
    private EnemyState currentState;

    // �v���C���[�����E�ɑ�����͈�
    private float sightRange = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgent���擾
        navMeshAgent = GetComponent<NavMeshAgent>();
        // ������J�n
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

    // ���񃍃W�b�N
    private void Patrol()
    {
        // �ړI�n�_�܂ł̋���(remainingDistance)���ړI�n�̎�O�܂ł̋���(stoppingDistance)�ȉ��ɂȂ�����
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // �ړI�n�̔ԍ����P�X�V�i�E�ӂ���]���Z�q�ɂ��邱�ƂŖړI�n�����[�v�������j
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointArray.Length;
            // �ړI�n�����̏ꏊ�ɐݒ�
            navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
        }
    }

    // �v���C���[��ǂ������郍�W�b�N
    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // �v���C���[�����͈͊O�ɏo���珄��ɖ߂�
        if (distanceToPlayer > sightRange)
        {
            currentState = EnemyState.Patrol;
            navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
        }
    }

    // �v���C���[�����m���郍�W�b�N
    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // �v���C���[�����F�͈͂ɓ�������ǐՂ��J�n
        if (distanceToPlayer <= sightRange)
        {
            currentState = EnemyState.Chase;
        }
    }
}
