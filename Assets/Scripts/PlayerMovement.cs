using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public GameObject winObject;
    public bool allowDiagonalMovement = false;
   
    public float moveSpeed = 5f;
    public float directionChangeDelay = 0.1f;
    
    public string pelletTag = "Pellet";
    public string wallTag = "Wall";
    public string powerPelletTag = "PowerPellet";
    public string enemyTag = "Enemy";
   
    public int powerPelletScore = 10;
    public Text scoreText;
    
    public ChildActivationManager childManager;


    private Rigidbody2D rb;
    private Animator animator;
    private bool isOnPellet = false;
    private Vector2 currentMoveDir = Vector2.zero;  
    private Vector2 handleInputDir = Vector2.zero;     
    private Vector2 nextDir = Vector2.zero;        
    private string lastDir = "Up";                
    private Vector2 lastPos;
    private int score = 0;

    
    private float dirChangeTimer;                  
    private bool isWaitingForAnim;                 

    [SerializeField] private AudioDirector audioDirector;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
        ConfigureCollider();
        
        SetAnimationState("Up");
        lastPos = rb.position;
        currentMoveDir = Vector2.up; 
        UpdateScoreDisplay();
        
        dirChangeTimer = 0;
        isWaitingForAnim = false;
    }

    void Update()
    {
        if(score >= 266)
        {
            winObject.gameObject.SetActive(true);
        }
        HandleInput();         
        UpdateDirChangeDelay(); 
    }

    void FixedUpdate()
    {
        HandleMovement();       
        UpdateMovementAudio();  
    }


    private void HandleInput()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateAnimationImmediately("Left");
            handleInputDir = Vector2.left;         
            StartDirChangeDelay();              
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UpdateAnimationImmediately("Right");
            handleInputDir = Vector2.right;
            StartDirChangeDelay();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            UpdateAnimationImmediately("Up");
            handleInputDir = Vector2.up;
            StartDirChangeDelay();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UpdateAnimationImmediately("Down");
            handleInputDir = Vector2.down;
            StartDirChangeDelay();
        }
    }

   
    private void StartDirChangeDelay()
    {
        isWaitingForAnim = true;
        dirChangeTimer = directionChangeDelay; 
    }

   
    private void UpdateDirChangeDelay()
    {
        if (!isWaitingForAnim) return;

        
        dirChangeTimer -= Time.deltaTime;
        if (dirChangeTimer <= 0)
        {
            
            nextDir = handleInputDir;
            isWaitingForAnim = false;
            handleInputDir = Vector2.zero; 
        }
    }

   
    private void HandleMovement()
    {
        
        if (nextDir != Vector2.zero)
        {
            
            if (CanMoveInDir(nextDir))
            {
                currentMoveDir = nextDir; 
            }
            nextDir = Vector2.zero; 
        }

        
        if (!CanMoveInDir(currentMoveDir))
        {
            currentMoveDir = Vector2.zero;
            return;
        }
       
        if (currentMoveDir != Vector2.zero)
        {
            Vector2 targetPos = rb.position + currentMoveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }
 

    private void UpdateAnimationImmediately(string dir)
    {
        SetAnimationState(dir);
        lastDir = dir;
    }

    private void SetAnimationState(string dir)
    {
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);
        animator.SetBool("Left", false);
        animator.SetBool("Right", false);

        switch (dir)
        {
            case "Up": animator.SetBool("Up", true);
                break;
            case "Down": animator.SetBool("Down", true);
                break;
            case "Left": animator.SetBool("Left", true);
                break;
            case "Right": animator.SetBool("Right", true);
                break;
        }
    }

    
    private bool CanMoveInDir(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, dir, 0.6f, LayerMask.GetMask("Wall"));
        return hit.collider == null;
    }

    private void ConfigureCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null) collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;
    }

    private void UpdateMovementAudio()
    {
        bool isMoving = Vector2.Distance(lastPos, rb.position) > 0.01f;
        lastPos = rb.position;

        if (isMoving)
            audioDirector.PlayMoveSound(isOnPellet);
        else 
            audioDirector.StopMoveSound();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(pelletTag))
        {
            AddScore(1);
            isOnPellet = true;
            audioDirector.PlayEatPelletSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag(powerPelletTag))
        {
            AddScore(powerPelletScore);
            audioDirector.PlayEatPelletSound();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(pelletTag)) isOnPellet = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.collider.CompareTag(wallTag))
        {
            currentMoveDir = Vector2.zero;
            nextDir = Vector2.zero; 
            isWaitingForAnim = false; 
            audioDirector.PlayHitWallSound();
        }
        
        else if (other.collider.CompareTag(enemyTag))
        {
            
            Vector2 initPos = new Vector3(19, 1, 0);
            transform.position = initPos;
            rb.position = initPos;

            
            currentMoveDir = Vector2.up;
            nextDir = Vector2.zero;
            handleInputDir = Vector2.zero;
            rb.velocity = Vector2.zero;
            isWaitingForAnim = false; 

            UpdateAnimationImmediately("Up");
          
            childManager.DeactivateChild();
        }
    }

}