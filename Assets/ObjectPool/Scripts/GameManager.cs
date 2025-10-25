using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class GameManager : MonoBehaviour
{
    
    [Header("Setup")]
    [Tooltip("The Enemy Prefab must have an EnemyController script attached.")]
    public EnemyController enemyPrefab;
    
    [Header("Wave Status")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int activeEnemies = 0;

    public static GameManager gameManager;
    
    // The actual Object Pool instance for managing enemies
    private IObjectPool<EnemyController> enemyPool;

    // --- Initialization (Pooling Setup) ---

    private void Awake()
    {
        // 1. Initialize the Object Pool
        // We set the maximum size high (e.g., 100) to ensure we don't accidentally exceed it early on.
        enemyPool = new ObjectPool<EnemyController>(
            createFunc: CreatePooledEnemy,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyPooledObject,
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 100
        );
    }
    
    private void Start()
    {
        // Start the first wave immediately
        StartNextWave();
    }

    // --- Pool Methods (Called by IObjectPool) ---
    
    private EnemyController CreatePooledEnemy()
    {
        // 1. Instantiate the prefab
        EnemyController enemy = Instantiate(enemyPrefab);
        
        // 2. Initialize the enemy with the callback it needs.
        // This is the crucial link: when the enemy calls its Die() method, 
        // it triggers this Manager's EnemyDied() method.
        
        
        // 3. Set the initial state to inactive (pool convention)
        enemy.gameObject.SetActive(false);
        
        return enemy;
    }
    
    private void OnTakeFromPool(EnemyController enemy)
    {
        // Called when enemyPool.Get() is called.
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnToPool(EnemyController enemy)
    {
        // Called when enemyPool.Release() is called.
        // We set it inactive and reset its color/state.
        enemy.gameObject.SetActive(false);
        enemy.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnDestroyPooledObject(EnemyController enemy)
    {
        // Called if the pool is destroyed or when objects are evicted from the pool.
        Destroy(enemy.gameObject);
    }

    // --- Wave Management Logic ---
    
    /// <summary>
    /// Starts the next wave, calculates the new enemy count, and initiates spawning.
    /// </summary>
    private void StartNextWave()
    {
        currentWave++;
        
        if (currentWave > 1)
        {
            // For wave 2 onwards, double the enemy count.
            enemiesPerWave *= 2;
        }

        Debug.Log($"--- Starting Wave {currentWave} with {enemiesPerWave} enemies! ---");
        
        // Use a coroutine for a slight delay before spawning
       SpawnEnemies();
    }

    
    private void SpawnEnemies()
    {
        activeEnemies = enemiesPerWave;
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // Get an enemy from the pool (triggers OnTakeFromPool)
            EnemyController enemy = enemyPool.Get();
            
            // Re-initialize its position/state (if not already handled in OnTakeFromPool)
            // Note: The EnemyController's Initialize method handles the position reset 
            // and re-associates the Die callback, even though it's already set 
            // in CreatePooledEnemy.
           
        }
    }
    
    // --- Enemy Death Callback ---

    /// <summary>
    /// Called by the EnemyController when it dies.
    /// This method is the core trigger for wave progression.
    /// </summary>
    /// <param name="enemy">The enemy instance that died.</param>
    public void EnemyDied(EnemyController enemy)
    {
        // 1. Immediately return the object to the pool (triggers OnReturnToPool)
        enemyPool.Release(enemy);
        
        // 2. Decrement the active count
        activeEnemies--;
        
        Debug.Log($"Enemy died. Active enemies remaining: {activeEnemies}");
        
        // 3. Check for wave completion
        if (activeEnemies <= 0)
        {
            Debug.Log($"Wave {currentWave} complete! Waiting for next wave...");
            StartNextWave();
        }
    }
}
