using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyGuardAI : MonoBehaviour
{

    NavMeshAgent agent;
    float distanceToTarget = Mathf.Infinity;
    public Transform target; 
    bool isProvroked = false;
    public bool isDead = false;
    float chaseRange = 5f;
    public bool isAttacking = false;
    float turnSpeed = 5f;
    [SerializeField] GameObject fpsCam;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    private float range = 350f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvroked && !isDead)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvroked = true;
        }
    }
    void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget >= agent.stoppingDistance)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= agent.stoppingDistance)
        {
            AttackTarget();
        }
    }
    void ChaseTarget()
    {
        //GetComponent<Animator>().SetTrigger("Attack");
        //GetComponent<Animator>().SetBool("Walking", true);
        agent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            if (agent.remainingDistance < 6.1f)
            {
                //GetComponent<Animator>().SetBool("Walking", false);
                //GetComponent<Animator>().SetTrigger("Attack");
                isAttacking = true;
                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    Debug.Log(hit.transform.name);
                    Debug.DrawLine(fpsCam.transform.position, hit.transform.position, Color.red, 1f);

                    movement target = hit.transform.GetComponent<movement>();
                    if (target != null)
                    {

                        GameObject Bullet = Instantiate(bullet, transform.position, transform.rotation);
                        Bullet.GetComponent<Rigidbody>().velocity = transform.forward * Time.deltaTime * bulletSpeed;

                    }
                }
            }

        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}
