using UnityEngine;
using System.Collections;

public class CherryController : MonoBehaviour
{
   
    public GameObject cherryPrefab;
    public float spawnDelay = 5f;
    public float moveSpeed = 3f;
    public Transform levelCenter;
    public float leftBoundary = -10f;
    public float rightBoundary = 10f;
    public float bottomBoundary = -10f;
    public float topBoundary = 10f;

    
    public float destroyAreaXMin = 1f;
    public float destroyAreaXMax = 19f;
    public float destroyAreaYMin = 1f;
    public float destroyAreaYMax = 17f;

    private GameObject currentCherry;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnCherryAfterDelay(spawnDelay));
    }

    private IEnumerator SpawnCherryAfterDelay(float delay)
    {
        isSpawning = true;
        yield return new WaitForSeconds(delay);
        if (cherryPrefab != null) SpawnCherry();
        isSpawning = false;
    }

    private void SpawnCherry()
    {
     
        int directionType = Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

      
        switch (directionType)
        {
            case 0: 
                spawnPos = new Vector3(leftBoundary - 2f, Random.Range(bottomBoundary, topBoundary), 0);
                break;
            case 1: 
                spawnPos = new Vector3(rightBoundary + 2f, Random.Range(bottomBoundary, topBoundary), 0);
                break;
            case 2: 
                spawnPos = new Vector3(Random.Range(leftBoundary, rightBoundary), bottomBoundary - 2f, 0);
                break;
            case 3: 
                spawnPos = new Vector3(Random.Range(leftBoundary, rightBoundary), topBoundary + 2f, 0);
                break;
        }

       
        currentCherry = Instantiate(cherryPrefab, spawnPos, Quaternion.identity);
        SpriteRenderer cherryRenderer = currentCherry.GetComponent<SpriteRenderer>();
        if (cherryRenderer != null) cherryRenderer.sortingOrder = 100;
        //currentCherry.layer = LayerMask.NameToLayer("Cherry");

        
        StartCoroutine(MoveCherry(currentCherry, directionType));
    }

    private IEnumerator MoveCherry(GameObject cherry, int directionType)
    {
        if (cherry == null) yield break;

        Vector3 startPos = cherry.transform.position;
        Vector3 targetPos = Vector3.zero;

      
        switch (directionType)
        {
            case 0: 
                targetPos = new Vector3(rightBoundary + 2f, levelCenter.position.y, 0);
                startPos.y = levelCenter.position.y;
                break;
            case 1: 
                targetPos = new Vector3(leftBoundary - 2f, levelCenter.position.y, 0);
                startPos.y = levelCenter.position.y;
                break;
            case 2: 
                targetPos = new Vector3(levelCenter.position.x, topBoundary + 2f, 0);
                startPos.x = levelCenter.position.x;
                break;
            case 3: 
                targetPos = new Vector3(levelCenter.position.x, bottomBoundary - 2f, 0);
                startPos.x = levelCenter.position.x;
                break;
        }

        cherry.transform.position = startPos;
        float distance = Vector3.Distance(startPos, targetPos);
        float moveTime = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;
            cherry.transform.position = Vector3.Lerp(startPos, targetPos, t);

           
            CheckIfOutOfTargetArea(cherry);

            yield return null;
        }

        DestroyCherry();
    }

 
    private void CheckIfOutOfTargetArea(GameObject cherry)
    {
        if (cherry == null) return;

        Vector3 cherryPos = cherry.transform.position;
        
        bool isXOutOfRange = cherryPos.x < destroyAreaXMin || cherryPos.x > destroyAreaXMax;
        bool isYOutOfRange = cherryPos.y < destroyAreaYMin || cherryPos.y > destroyAreaYMax;

        if (isXOutOfRange && isYOutOfRange)
        {
            DestroyCherry();
        }
    }

    public void DestroyCherry()
    {
        if (currentCherry != null)
        {
            Destroy(currentCherry);
            currentCherry = null;
            if (!isSpawning) StartCoroutine(SpawnCherryAfterDelay(spawnDelay));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyCherry();
        }
    }
}