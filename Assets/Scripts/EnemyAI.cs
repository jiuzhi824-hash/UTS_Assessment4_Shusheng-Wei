using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,     
        Chase,    
      
    }
   
    public float normalSpeed = 3f;
    public float chaseSpeed = 4.5f;
    public float scaredSpeed = 2f;

    
    public float detectionRadius = 5f;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;

    
    public Transform[] patrolPoints;
    public float minDirectionChangeTime = 2f;
    public float maxDirectionChangeTime = 5f;

    public EnemyState currentState = EnemyState.Patrol;

    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private float directionChangeTimer;
    private int currentPatrolIndex = 0;
    private Vector2 startPosition;
    private Transform player;
  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        startPosition = transform.position;

       
        ChangeDirectionRandomly();

        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
       
        UpdateStateMachine();

      
    }

    void FixedUpdate()
    {
       
        MoveAccordingToState();
    }

    private void UpdateStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolState();
                break;
            case EnemyState.Chase:
                ChaseState();
                break;
           
                
        }
    }

    private void PatrolState()
    {
        
        if (CanSeePlayer())
        {
            currentState = EnemyState.Chase;
            return;
        }

      
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0)
        {
            if (patrolPoints.Length > 0)
            {
                
                MoveToNextPatrolPoint();
            }
            else
            {
              
                ChangeDirectionRandomly();
            }

            directionChangeTimer = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
        }

       
        if (IsPathBlocked())
        {
            ChangeDirectionRandomly();
        }
    }

    private void ChaseState()
    {
        
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Patrol;
            return;
        }

        
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        currentDirection = directionToPlayer;

       
        if (IsPathBlocked())
        {
            
            FindAlternativeDirection();
        }
    }
    private void MoveAccordingToState()
    {
        float speed = normalSpeed;

        switch (currentState)
        {
            case EnemyState.Chase:
                speed = chaseSpeed;
                break;
        }

       
        Vector2 newPosition = rb.position + currentDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRadius) return false;

     
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, obstacleLayer);

       
        return hit.collider == null || hit.collider.CompareTag("Player");
    }

    private bool IsPathBlocked()
    {
       
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 0.6f, obstacleLayer);
        return hit.collider != null;
    }

    private void ChangeDirectionRandomly()
    {
     
        Vector2[] possibleDirections = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

      
        for (int i = 0; i < possibleDirections.Length; i++)
        {
            Vector2 temp = possibleDirections[i];
            int randomIndex = Random.Range(i, possibleDirections.Length);
            possibleDirections[i] = possibleDirections[randomIndex];
            possibleDirections[randomIndex] = temp;
        }

     
        foreach (Vector2 direction in possibleDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, obstacleLayer);
            if (hit.collider == null)
            {
                currentDirection = direction;
                return;
            }
        }

       
        currentDirection = -currentDirection;
    }

    private void FindAlternativeDirection()
    {
       
        Vector2[] testDirections = {
            new Vector2(currentDirection.y, currentDirection.x),  
            new Vector2(-currentDirection.y, -currentDirection.x), 
            -currentDirection // 相反方向
        };

        foreach (Vector2 direction in testDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, obstacleLayer);
            if (hit.collider == null)
            {
                currentDirection = direction;
                return;
            }
        }

      
        ChangeDirectionRandomly();
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

       
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        Vector2 directionToPoint = (patrolPoints[currentPatrolIndex].position - transform.position).normalized;
        currentDirection = directionToPoint;
    }


  
}