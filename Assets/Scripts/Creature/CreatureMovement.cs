using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator creature;
    [SerializeField] private Transform creatureTransform;

    private int idleSpeed = 0;
    private float walkspeed = 4.2f;
    private float sprintspeed = 11;

    private float timer;
    private float idleAnimtime = 2.6f;
    private float roarAnimtime = 5.5f;


    [SerializeField] private Transform[] patrolPoints;
    private int patrolIndex;
    private int currentPatrolIndex;

    private Transform player;
    

    private float chaseRange = 20f;
    private float walkChase = 12f;
    private float stopChase = 100f;
    private float attackRange = 8f;
    private float distancefromPlayer;

    private float distancefromPatrolPoint;
    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        patrolIndex = Random.Range(0, patrolPoints.Length);
        StartCoroutine(GotoFirstPoint());
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.speed = idleSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        distancefromPlayer = Vector3.Distance(player.position, creature.transform.position);
        distancefromPatrolPoint = Vector3.Distance(creature.transform.position, navMeshAgent.destination);
        AttackPlayer();
        atPatrolPoint();
    }

    void atPatrolPoint()
    {
        if (creature.GetBool("isChasing") == false)
        {
            if (navMeshAgent.remainingDistance < .5f && navMeshAgent.remainingDistance != 0)
            { 
                creature.SetBool("atPatrolPoint", true);
                creature.SetBool("isPatrolling", false);
                navMeshAgent.speed = idleSpeed;
                GoToNextPoint();
            }
        }
    }

    IEnumerator GotoFirstPoint()
    {
        creature.SetBool("isPatrolling", true);
        navMeshAgent.destination = patrolPoints[patrolIndex].position;
        currentPatrolIndex = patrolIndex;
        yield return new WaitForSeconds(idleAnimtime);
        navMeshAgent.speed = walkspeed;
        StopAllCoroutines();
    }
    //delay for Idle animation
    IEnumerator IdleafterChase()
    {
        navMeshAgent.speed = idleSpeed;
        yield return new WaitForSeconds(roarAnimtime);
        GoToNextPoint();
        navMeshAgent.speed = walkspeed;
        StopAllCoroutines();
    }

    void GoToNextPoint()
    {
        creature.SetBool("isPatrolling", true);
        if(patrolPoints.Length == 0)
        {
            return;
        }        
        creature.SetBool("atPatrolPoint", false);
        navMeshAgent.destination = patrolPoints[pickNewPatrolPoint()].position;
        navMeshAgent.speed = walkspeed;
        
        
    }

    int pickNewPatrolPoint()
    {
        while(patrolIndex == currentPatrolIndex)
        {
            patrolIndex = Random.Range(0, patrolPoints.Length);
        }
        currentPatrolIndex = patrolIndex;
        return patrolIndex; 
    }

    void AttackPlayer()
    {
        //initializes attack player sequence
        if (distancefromPlayer < chaseRange && creature.GetBool("isChasing") == false)
        {
            creature.SetBool("atPatrolPoint", false);
            creature.SetBool("isPatrolling", false);
            creature.SetBool("isChasing", true);
            creature.SetBool("firstEncounter", true);
            navMeshAgent.destination = player.position;
        }

        if(creature.GetBool("firstEncounter") == true)
        {
            timer += Time.deltaTime;
        }

        if (timer <= roarAnimtime && creature.GetBool("isChasing") == true)
        {
            navMeshAgent.speed = idleSpeed;
        }
        if (timer > roarAnimtime && creature.GetBool("isChasing") == true)
        {
            //changes creature speed to walk when chasing and player is very close
            if (distancefromPlayer < walkChase)
            {
                creature.SetBool("firstEncounter", false);
                creature.SetBool("playerClose", true);
                navMeshAgent.speed = walkspeed;
            }
            else
            {
                creature.SetBool("firstEncounter", false);
                creature.SetBool("playerClose", false);
                navMeshAgent.speed = sprintspeed;
            }
        }
        if(distancefromPlayer < attackRange)
        {
            creature.SetBool("isAttacking", true);
        }
        else
        {
            creature.SetBool("isAttacking", false);
        }

        //updates player location while chasing
        if (creature.GetBool("isChasing") == true)
        {
            navMeshAgent.destination = player.position;
        }
        //ends chase sequence // player is far away
        if (distancefromPlayer >= stopChase && creature.GetBool("isChasing") == true)
        {
            creature.SetBool("isChasing", false);
            creature.SetBool("playerClose", false);
            StartCoroutine(IdleafterChase());
            timer = 0f;
            
        }
    }

    
    
}
