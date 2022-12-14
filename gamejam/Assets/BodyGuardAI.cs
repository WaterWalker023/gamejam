using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyGuardAI : MonoBehaviour
{
    NavMeshAgent agent;
    float distanceToTarget = Mathf.Infinity;
    public Transform target; 
    public bool isDead = false;
    float Timetorotate = 4f;
    private float turnSpeed = 5f;
    public Transform[] waypoints;
    private int waypointIndex;
    private bool isPatrolling;
    float viewRadius = 15f;
    float viewangle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public float edgeIterations = 4f;
    public float edgeDistance = 0.5f;
    bool B_playerInRange;
    Vector3 targetPosition;
    Vector3 playerLastPosition = Vector3.zero;
    bool playerNear;
    float waitTime = 4;
    bool caughtplayer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isPatrolling = true;
        agent.SetDestination(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        view();

        if (!isPatrolling)
        {
            ChaseTarget();
        }
        else
        {
            patroling();
        }
    }
    void ChaseTarget()
    {
        //GetComponent<Animator>().SetTrigger("Attack");
        //GetComponent<Animator>().SetBool("Walking", true);
        Debug.Log("jaaaaa");
        playerLastPosition = Vector3.zero;
        playerNear = false;

        if (!caughtplayer)
        {
            Move(9);
            agent.SetDestination(target.position);
        }
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            caughtplayer = true;
            if(waitTime <= 0 && caughtplayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)>= 6f)
            {
                isPatrolling = true;
                playerNear = false;
                Move(6);
                waitTime = 4;
                Timetorotate = turnSpeed;
                agent.SetDestination(waypoints[waypointIndex].position);
            }
            else
            {
                if(waitTime <= 0 && caughtplayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    waitTime -= Time.deltaTime;
                    AttackTarget();
                }
            }
        }
    }

    void AttackTarget()
    {
        FaceTarget();
        if (agent.remainingDistance < agent.stoppingDistance)
        {
           //ga naar het game over scherm
        }
    }
    private void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }
    private void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }
    void nextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[waypointIndex].position);
    }
    void view()
    {
        Collider[] targetinrange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < targetinrange.Length; i++)
        {
            Transform target = targetinrange[i].transform;
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToPlayer)< viewangle / 2)
            {
                if(!Physics.Raycast(transform.position, dirToPlayer, distanceToTarget, obstacleMask))
                {
                    B_playerInRange = true;
                    isPatrolling = false;
                }
                else
                {
                    B_playerInRange = false;
                }
            }
            if(Vector3.Distance(transform.position, target.position)> viewRadius)
            {
                B_playerInRange = false;
            }
        }
        if (B_playerInRange)
        {
            targetPosition = target.transform.position;
        }
    }
    private void patroling()
    {
        if (playerNear)
        {
            if (Timetorotate <= 0)
            {
                Move(6);
                FaceTarget();
            }
            else
            {
                Stop();
                Timetorotate -= Time.deltaTime;
            }
        }
        else
        {
            playerNear = false;
            playerLastPosition = Vector3.zero;
            agent.SetDestination(waypoints[waypointIndex].position);
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(waitTime <= 0)
                {
                    nextWaypoint();
                    Move(6);
                }
                else
                {
                    Stop();
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,  1);
    }
}
