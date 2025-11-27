using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class enemyAI : MonoBehaviour
{
    public enum enemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Death
    }
    public enemyState currentState = enemyState.Idle;

    //for attack
    // movement lock during attack
    public float attackLockDuration = 1f;
    private float movementLockEndTime = 0f;

    public float attackCooldown = 3f;
    private float lastAttackTime = 0f;

    public int facingDirection = 1;

    [Header("AI Settings")]

    public float Speed = 10f;
    public float chaseRange = 5f;
    public float attackRange = 2f;
    public int HP = 100;

    public Transform[] patrolPoints;
    private int patrolIndex = 0;

    [Header("Attack Damage")]
    public int attackDamage = 10;


    public Transform player;

    public Animator anim;


    //bool flags
    private bool canMove = true;
    private bool canAttack = true;
    private bool isDead = false;
    //pause each patrol points
    private float patrolWaitTime = 5f;  
    private float patrolWaitCounter = 0f;
    private bool isWaiting = false;



    void Start()
    {
        lastAttackTime = -attackCooldown;
        anim.SetBool("Alive", true);
    }   

    // Update is called once per frame
    void Update()
    {

        if (!canMove && Time.time >= movementLockEndTime)
        {
            canMove = true;
        }

            switch (currentState)
        {
            case enemyState.Idle:
                Idle();
                break;
            case enemyState.Patrol:
                Patrol();
                break;
            case enemyState.Chase:
                Chase();
                break;
            case enemyState.Attack:
                Attack();
                break;
            case enemyState.Death:
                Death();
                break;
        }

        StateTransitions();
    }

    private void OnDrawGizmosSelected()
    {
        //yellow = chase range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        //red = attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.2f);
                }
            }

        }
    }


    void Idle()
    {
        //animation sprite
        anim.SetFloat("isMoving", 0f);

    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        if (!canMove || isDead)
            return;

        if (isWaiting)
        {
            anim.SetFloat("isMoving", 0f);

            // count down wait
            patrolWaitCounter -= Time.deltaTime;

            if (patrolWaitCounter <= 0f)
            {
                isWaiting = false; // done waiting
            }
            return; // stop moving while waiting
        }

        Transform targetPoint = patrolPoints[patrolIndex];

        // Flip based on patrol target
        if (targetPoint.position.x > transform.position.x && facingDirection == -1)
        {
            Flip();
        }
        else if (targetPoint.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }


        // Move toward patrol point
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, Speed * Time.deltaTime);

        anim.SetFloat("isMoving", Speed);

        // If reached patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

            // start waiting
            isWaiting = true;
            patrolWaitCounter = patrolWaitTime;
        }
    }

    void Chase()
    {
        if (player.GetComponent<playerMovements>().isDead)
            return;

        if (player == null)
            return;


        if (!canMove || isDead)
            return;


        // Flip based on player position
        if (player.position.x > transform.position.x && facingDirection == -1)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        

        //follow player
        transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);

        anim.SetFloat("isMoving", Speed);

    }
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        if (player.GetComponent<playerMovements>().isDead)
            return;

        // stop movement immediately while attacking
        anim.SetFloat("isMoving", 0f);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Attacked Player");
            anim.SetTrigger("ATTACK");
            lastAttackTime = Time.time;

            // Detect player in attack range
            Vector2 attackPos = transform.position + new Vector3(facingDirection * 1f, 0, 0);
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPos, 0.5f, LayerMask.GetMask("Player"));

            if (hitPlayer != null)
            {
                playerMovements player = hitPlayer.GetComponent<playerMovements>();
                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                }
            }

            // Lock movement during attack
            canMove = false;
            movementLockEndTime = Time.time + attackLockDuration;
        }
    }
    void Death()
    {

        if (HP > 0)
        {
            anim.SetBool("Alive", true);
            isDead = false;
            canMove = true;
            canAttack = true;
        }

        if (isDead) return;

        if (HP <= 0)
        {
            anim.SetBool("Alive", false);
            anim.SetTrigger("Death");
            canMove = false;
            canAttack = false;
            isDead = true;
        }
    }


    void StateTransitions() {
        //check for player
        if(player == null) 
            return;

        if (player.GetComponent<playerMovements>().isDead)
        {
            currentState = enemyState.Patrol;
            return;
        }

        //get the distance value as float
        float Distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case enemyState.Idle:
               
                if (Distance < chaseRange)
                    currentState = enemyState.Chase;
                else
                    currentState = enemyState.Patrol;

                if (HP <= 0)
                    currentState = enemyState.Death;
                break;

            case enemyState.Patrol:

                if (Distance < chaseRange)
                    currentState = enemyState.Chase;

                if (HP <= 0)
                    currentState = enemyState.Death;
                break;

            case enemyState.Chase:
                
                if (Distance<attackRange)
                    currentState = enemyState.Attack;

                else if(Distance > chaseRange)
                    currentState = enemyState.Patrol;

                if (HP <= 0)
                    currentState = enemyState.Death;
                break;  

            case enemyState.Attack:

                if (Distance > attackRange)
                    currentState = enemyState.Chase;

                if (HP <= 0)
                    currentState = enemyState.Death;
                    break;
            case enemyState.Death:
                if(HP > 0)
                    currentState = enemyState.Chase;
                break;
        }
       
    }
    //for taking damage something
    public void TakeDamage(int damage)
    {
        if (isDead) return; 

        HP -= damage;
        Debug.Log("Enemy takes " + damage + " damage! HP left: " + HP);

        if (HP <= 0)
        {
            HP = 0;
            currentState = enemyState.Death;
            Death();
        }
    }


}
