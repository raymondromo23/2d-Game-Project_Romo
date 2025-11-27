using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CatMovements : MonoBehaviour
{
    public enum catState
    {
        Idle,
        Patrol,
        LickingBalls,
        GettingPet,
    }
    public catState currentState = catState.Idle;
    public Animator anim;

    public int facingDirection = 1;

    public float speed = 5f;
    public Transform[] patrolPoints;
    private int patrolIndex = 0;


    void Update()
    {
        switch (currentState)
        {
            case catState.Idle:
                Idle();
                break;
            case catState.Patrol:
                Patrol();
                break;
            case catState.LickingBalls:
                LickBalls();
                break;
            case catState.GettingPet:
                GettingPet();
                break;
        }

        stateTransitions();
    }

    void Idle()
    {
        

    }

    void Patrol()
    {
        Transform targetPoint = patrolPoints[patrolIndex];

        anim.SetFloat("isMoving", 0f);
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
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        anim.SetFloat("isMoving", speed);

        // If reached patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length; 
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    void LickBalls()
    {


    }
    void GettingPet()
    {
        anim.SetFloat("isMoving", 0f);
        anim.SetTrigger("Pet");
        Debug.Log("Cat is being petted...");
    }
    public IEnumerator GetPetRoutine()
    {
        currentState = catState.GettingPet;
        

        yield return new WaitForSeconds(2f);

        currentState = catState.Patrol;
    }

    void stateTransitions()
    {

        switch (currentState) {
            case catState.Idle:
                currentState = catState.Patrol;

                break;
            case catState.Patrol:
                

                break;

            case catState.LickingBalls:


                break;

        }





    
    
    }



    }



