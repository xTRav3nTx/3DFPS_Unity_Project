using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureMovement : MonoBehaviour
{
    public Gun gun;
    public CreatureRayCast ray;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator creature;
    [SerializeField] private Transform creatureTransform;

    private int idleSpeed = 0;
    private float walkChaseSpeed = 4f;
    private float walkspeed = 2.2f;
    private float sprintspeed = 9;
    private float attackwalkSpeed = 2f;

    private float timer;
    private float idleAnimtime = 2.6f;
    private float roarAnimtime = 5.5f;
    private float hitAnimtime = 1.5f;
    private float attackAnimtime = 2.3f;


    [SerializeField] private Transform[] patrolPoints;
    private int patrolIndex;
    private int currentPatrolIndex;

    private Transform player;
    

    private float chaseRange = 20f;
    private float walkChase = 9f;
    private float stopChase = 100f;
    private float attackRange = 4f;
    private float distancefromPlayer;
    public bool isAttacking;

    public float attackPower = 15f;

    private float distancefromPatrolPoint;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        patrolIndex = Random.Range(0, patrolPoints.Length);
        StartCoroutine(GotoFirstPoint());
    }


    // Start is called before the first frame update
    void Start()
    {
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
        navMeshAgent.destination = patrolPoints[currentPatrolIndex].position;
        navMeshAgent.speed = walkspeed;
        StopAllCoroutines();
    }

    IEnumerator SlowWalkwhileAttacking()
    {
        navMeshAgent.speed = attackwalkSpeed;
        yield return new WaitForSeconds(attackAnimtime/2f);
        isAttacking = true;
        yield return new WaitForSeconds(attackAnimtime/2f);
        isAttacking = false;
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
    void playerIsClose()
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
    }

    void shotByPlayer()
    {
        if(gun.shotCreature && creature.GetBool("isChasing") == false)
        {
            creature.SetBool("atPatrolPoint", false);
            creature.SetBool("isPatrolling", false);
            creature.SetBool("isChasing", true);
            creature.SetBool("firstEncounter", true);
            navMeshAgent.destination = player.position;
        }
    }


    void AttackPlayer()
    {
        playerIsClose();
        shotByPlayer();
        AttackingPlayer();

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
            if(!isAttacking)
            {
                attackingChaseSpeed();
            }
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
            gun.shotCreature = false;
        }
    }

    void attackingChaseSpeed()
    {
        //changes creature speed to walk when chasing and player is very close
        if (distancefromPlayer < walkChase)
        {
            creature.SetBool("firstEncounter", false);
            creature.SetBool("playerClose", true);
            navMeshAgent.speed = walkChaseSpeed;
        }
        else
        {
            creature.SetBool("firstEncounter", false);
            creature.SetBool("playerClose", false);
            navMeshAgent.speed = sprintspeed;
        }
    }

    void AttackingPlayer()
    {
        if (distancefromPlayer < attackRange && ray.lookingAtPlayer)
        {
            creature.SetBool("isAttacking", true);
            StartCoroutine(SlowWalkwhileAttacking());
        }
        else
        {
            creature.SetBool("isAttacking", false);
        }
    }

    
    
}
