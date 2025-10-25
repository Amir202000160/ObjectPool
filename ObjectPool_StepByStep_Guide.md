# Unity Object Pool System - Step by Step Guide

This guide breaks down each line of code in the Unity Object Pool project, explaining every step of the implementation.

## 1. PooledObject.cs - Base Object Pool Component

### Step 1: Import Unity namespace
```csharp
using UnityEngine;
```
- Imports Unity's core functionality for MonoBehaviour and transform operations

### Step 2: Define PooledObject class
```csharp
public class PooledObject : MonoBehaviour
```
- Creates a public class that inherits from MonoBehaviour
- This allows the script to be attached to GameObjects

### Step 3: Declare private pool reference
```csharp
private ObjectPool pool;
```
- Creates a private field to store reference to the object pool that manages this object

### Step 4: Create public property for pool access
```csharp
public ObjectPool Pool { get => pool; set => pool = value; }
```
- Creates a public property with getter and setter for the pool reference
- Allows external classes to assign and retrieve the pool reference

### Step 5: Define Release method
```csharp
public void Release()
```
- Creates a public method that can be called to return this object to its pool

### Step 6: Return object to pool
```csharp
pool.ReturnToPool(this);
```
- Calls the pool's ReturnToPool method, passing this object as parameter
- This deactivates the object and makes it available for reuse

### Step 7: Update method for automatic cleanup
```csharp
void Update()
```
- Unity's built-in method called once per frame

### Step 8: Check if object is out of bounds
```csharp
if (transform.position.z > 20.0f)
```
- Checks if the object has moved beyond z-position of 20 units
- This prevents objects from traveling infinitely

### Step 9: Release out-of-bounds objects
```csharp
Release();
```
- Automatically returns the object to pool when it goes out of bounds
- Prevents memory waste and maintains performance

## 2. ObjectPool.cs - Pool Management System

### Step 10: Import collections namespace
```csharp
using System.Collections.Generic;
```
- Imports generic collections like Stack for object storage

### Step 11: Import Unity namespace
```csharp
using UnityEngine;
```
- Imports Unity's core functionality

### Step 12: Define ObjectPool class
```csharp
public class ObjectPool : MonoBehaviour
```
- Creates the main pool management class

### Step 13: Serialize initial pool size
```csharp
[SerializeField] private int initPoolSize;
```
- Creates an editable field in Unity Inspector for initial pool size
- Private but visible in Inspector due to SerializeField attribute

### Step 14: Serialize object prefab reference
```csharp
[SerializeField] private PooledObject objectToPool;
```
- Creates an Inspector field for the prefab to be pooled
- Must be a PooledObject or inherit from it

### Step 15: Declare stack for object storage
```csharp
private Stack<PooledObject> stack;
```
- Creates a Stack collection to store inactive pooled objects
- Stack provides LIFO (Last In, First Out) access pattern

### Step 16: Start method initialization
```csharp
private void Start()
```
- Unity's Start method, called once before first frame update

### Step 17: Initialize pool setup
```csharp
SetupPool();
```
- Calls the pool setup method to create initial objects

### Step 18: Define SetupPool method
```csharp
private void SetupPool()
```
- Private method to handle pool initialization

### Step 19: Initialize stack collection
```csharp
stack = new Stack<PooledObject>();
```
- Creates new Stack instance to store pooled objects

### Step 20: Declare instance variable
```csharp
PooledObject instance = null;
```
- Creates variable to hold each instantiated object during setup

### Step 21: Loop for initial object creation
```csharp
for (int i = 0; i < initPoolSize; i++)
```
- Loops from 0 to initPoolSize to create initial pool objects

### Step 22: Instantiate pooled object
```csharp
instance = Instantiate(objectToPool, transform.position, transform.rotation);
```
- Creates new instance of the prefab at pool's position and rotation

### Step 23: Assign pool reference to instance
```csharp
instance.Pool = this;
```
- Sets the pool reference in the instantiated object
- Allows the object to communicate back to this pool

### Step 24: Deactivate initial objects
```csharp
instance.gameObject.SetActive(false);
```
- Deactivates the GameObject to make it invisible and non-functional
- Objects in pool should be inactive until needed

### Step 25: Add object to stack
```csharp
stack.Push(instance);
```
- Pushes the inactive object onto the stack for later retrieval

### Step 26: Define GetPooledObject method
```csharp
public PooledObject GetPooledObject()
```
- Public method to retrieve an available object from the pool

### Step 27: Check if pool is empty
```csharp
if (stack.Count == 0)
```
- Checks if there are no available objects in the pool

### Step 28: Create new instance when pool is empty
```csharp
PooledObject newInstance = Instantiate(objectToPool, transform.position, transform.rotation);
```
- Instantiates a new object if pool is exhausted
- Ensures pool never fails to provide an object

### Step 29: Assign pool to new instance
```csharp
newInstance.Pool = this;
```
- Sets pool reference for the newly created object

### Step 30: Return new instance
```csharp
return newInstance;
```
- Returns the newly created object to the caller

### Step 31: Get object from stack
```csharp
PooledObject nextInstance = stack.Pop();
```
- Retrieves the most recently added object from the stack

### Step 32: Activate retrieved object
```csharp
nextInstance.gameObject.SetActive(true);
```
- Activates the GameObject to make it visible and functional

### Step 33: Return activated object
```csharp
return nextInstance;
```
- Returns the activated object to the caller

### Step 34: Define ReturnToPool method
```csharp
public void ReturnToPool(PooledObject pooledObject)
```
- Public method to return an object back to the pool

### Step 35: Push object back to stack
```csharp
stack.Push(pooledObject);
```
- Adds the returned object back to the stack for reuse

### Step 36: Deactivate returned object
```csharp
pooledObject.gameObject.SetActive(false);
```
- Deactivates the object to hide it from the scene

### Step 37: Reset object position
```csharp
pooledObject.transform.position = transform.position;
```
- Resets the object's position to the pool's position
- Ensures clean state for next use

## 3. Bullet.cs - Projectile Behavior

### Step 38: Import Unity namespace
```csharp
using UnityEngine;
```
- Imports Unity's core functionality

### Step 39: Define Bullet class
```csharp
public class Bullet : MonoBehaviour
```
- Creates the bullet behavior class

### Step 40: Update method for movement
```csharp
void Update()
```
- Unity's Update method called every frame

### Step 41: Move bullet upward
```csharp
transform.Translate(Vector3.up * Time.deltaTime * 20f);
```
- Moves the bullet upward (positive Y direction) at 20 units per second
- Uses Time.deltaTime for frame-rate independent movement

## 4. PlayerMove.cs - Player Controller

### Step 42: Import Unity namespace
```csharp
using UnityEngine;
```
- Imports Unity's core functionality

### Step 43: Define PlayerMove class
```csharp
public class PlayerMove : MonoBehaviour
```
- Creates the player movement and shooting controller

### Step 44: Declare horizontal input variable
```csharp
public float InputHorinzontal;
```
- Public variable to store horizontal input value

### Step 45: Declare movement speed
```csharp
public float speed = 10.0f;
```
- Public variable for player movement speed (10 units per second)

### Step 46: Declare movement range
```csharp
public float xRange = 20.0f;
```
- Public variable for horizontal movement boundaries

### Step 47: Declare projectile pool reference
```csharp
public ObjectPool ProjectiledObject;
```
- Public reference to the object pool for bullets
- Assigned in Unity Inspector

### Step 48: Start method
```csharp
void Start()
```
- Unity's Start method (currently empty)

### Step 49: Update method for input handling
```csharp
void Update()
```
- Unity's Update method called every frame

### Step 50: Call movement method
```csharp
Moveee();
```
- Calls the custom movement method every frame

### Step 51: Check for space key input
```csharp
if (Input.GetKeyDown(KeyCode.Space))
```
- Checks if spacebar was pressed this frame

### Step 52: Fire projectile on input
```csharp
Fire();
```
- Calls the Fire method when spacebar is pressed

### Step 53: Define movement method
```csharp
public void Moveee()
```
- Custom method to handle player movement and boundaries

### Step 54: Check left boundary
```csharp
if (transform.position.x < -xRange)
```
- Checks if player has moved beyond left boundary

### Step 55: Clamp to left boundary
```csharp
transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
```
- Sets player position to left boundary if exceeded
- Maintains Y and Z positions

### Step 56: Check right boundary
```csharp
else if (transform.position.x > xRange)
```
- Checks if player has moved beyond right boundary

### Step 57: Clamp to right boundary
```csharp
transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
```
- Sets player position to right boundary if exceeded

### Step 58: Get horizontal input
```csharp
InputHorinzontal = Input.GetAxis("Horizontal");
```
- Gets horizontal input value (-1 to 1) from keyboard/controller

### Step 59: Apply movement
```csharp
transform.Translate(Vector3.right * InputHorinzontal * Time.deltaTime * speed);
```
- Moves player horizontally based on input, delta time, and speed

### Step 60: Define Fire method
```csharp
public void Fire()
```
- Method to handle projectile firing

### Step 61: Get projectile from pool
```csharp
PooledObject projectile = ProjectiledObject.GetPooledObject();
```
- Retrieves an available projectile from the object pool

### Step 62: Set projectile position
```csharp
projectile.transform.position = transform.position;
```
- Sets the projectile's position to player's current position

## 5. EnemyController.cs - Enemy Behavior

### Step 63: Import annotations namespace
```csharp
using JetBrains.Annotations;
```
- Imports JetBrains annotations (may not be necessary)

### Step 64: Import Unity namespace
```csharp
using UnityEngine;
```
- Imports Unity's core functionality

### Step 65: Define EnemyController class
```csharp
public class EnemyController : MonoBehaviour
```
- Creates the enemy behavior class

### Step 66: Awake method for initialization
```csharp
void Awake()
```
- Unity's Awake method, called before Start

### Step 67: Find GameManager reference
```csharp
GameManager.gameManager = FindObjectOfType<GameManager>();
```
- Finds and assigns the GameManager instance in the scene
- Uses static reference for easy access

### Step 68: Update method for movement
```csharp
void Update()
```
- Unity's Update method called every frame

### Step 69: Move enemy forward
```csharp
transform.Translate(Vector3.forward * Time.deltaTime * -2f);
```
- Moves enemy backward (negative Z direction) at 2 units per second

### Step 70: Collision detection method
```csharp
void OnCollisionEnter(Collision collision)
```
- Unity's collision detection method

### Step 71: Check for bullet collision
```csharp
if (collision.gameObject.CompareTag("Bullet"))
```
- Checks if the colliding object has the "Bullet" tag

### Step 72: Notify GameManager of enemy death
```csharp
GameManager.gameManager.EnemyDied(this);
```
- Calls the GameManager's EnemyDied method, passing this enemy instance
- Triggers enemy removal and wave progression logic

## 6. GameManager.cs - Game Flow and Advanced Pooling

### Step 73: Import Unity namespace
```csharp
using UnityEngine;
```
- Imports Unity's core functionality

### Step 74: Import Unity Pool namespace
```csharp
using UnityEngine.Pool;
```
- Imports Unity's built-in object pooling system

### Step 75: Import Collections namespace
```csharp
using System.Collections;
```
- Imports coroutine support

### Step 76: Define GameManager class
```csharp
public class GameManager : MonoBehaviour
```
- Creates the main game management class

### Step 77: Header attribute for organization
```csharp
[Header("Setup")]
```
- Creates a header in Unity Inspector for better organization

### Step 78: Tooltip for enemy prefab
```csharp
[Tooltip("The Enemy Prefab must have an EnemyController script attached.")]
```
- Provides helpful tooltip in Unity Inspector

### Step 79: Enemy prefab reference
```csharp
public EnemyController enemyPrefab;
```
- Public reference to the enemy prefab for instantiation

### Step 80: Wave status header
```csharp
[Header("Wave Status")]
```
- Creates another header section in Inspector

### Step 81: Current wave counter
```csharp
[SerializeField] private int currentWave = 0;
```
- Private field for tracking current wave number, visible in Inspector

### Step 82: Enemies per wave setting
```csharp
[SerializeField] private int enemiesPerWave = 5;
```
- Private field for number of enemies in current wave

### Step 83: Active enemy counter
```csharp
[SerializeField] private int activeEnemies = 0;
```
- Private field tracking currently active enemies

### Step 84: Static GameManager reference
```csharp
public static GameManager gameManager;
```
- Static reference for easy access from other scripts

### Step 85: Unity's object pool declaration
```csharp
private IObjectPool<EnemyController> enemyPool;
```
- Declares Unity's built-in object pool for enemy management

### Step 86: Awake method for pool initialization
```csharp
private void Awake()
```
- Unity's Awake method for early initialization

### Step 87: Initialize Unity object pool
```csharp
enemyPool = new ObjectPool<EnemyController>(
```
- Creates new Unity ObjectPool instance with configuration

### Step 88: Create function parameter
```csharp
createFunc: CreatePooledEnemy,
```
- Assigns function to create new enemies when pool is empty

### Step 89: Get action parameter
```csharp
actionOnGet: OnTakeFromPool,
```
- Assigns function called when enemy is retrieved from pool

### Step 90: Release action parameter
```csharp
actionOnRelease: OnReturnToPool,
```
- Assigns function called when enemy is returned to pool

### Step 91: Destroy action parameter
```csharp
actionOnDestroy: OnDestroyPooledObject,
```
- Assigns function called when pooled object is destroyed

### Step 92: Collection check parameter
```csharp
collectionCheck: false,
```
- Disables collection checking for performance

### Step 93: Default capacity parameter
```csharp
defaultCapacity: 5,
```
- Sets initial pool size to 5 objects

### Step 94: Maximum size parameter
```csharp
maxSize: 100
```
- Sets maximum pool size to 100 objects

### Step 95: Start method for game initialization
```csharp
private void Start()
```
- Unity's Start method called after Awake

### Step 96: Start first wave
```csharp
StartNextWave();
```
- Begins the first wave of enemies

### Step 97: Define create pooled enemy method
```csharp
private EnemyController CreatePooledEnemy()
```
- Method called by pool when new enemy needs to be created

### Step 98: Instantiate enemy prefab
```csharp
EnemyController enemy = Instantiate(enemyPrefab);
```
- Creates new instance of enemy prefab

### Step 99: Set initial inactive state
```csharp
enemy.gameObject.SetActive(false);
```
- Deactivates newly created enemy (pool convention)

### Step 100: Return created enemy
```csharp
return enemy;
```
- Returns the created enemy to the pool

### Step 101: Define take from pool method
```csharp
private void OnTakeFromPool(EnemyController enemy)
```
- Method called when enemy is retrieved from pool

### Step 102: Activate retrieved enemy
```csharp
enemy.gameObject.SetActive(true);
```
- Activates the enemy GameObject

### Step 103: Define return to pool method
```csharp
private void OnReturnToPool(EnemyController enemy)
```
- Method called when enemy is returned to pool

### Step 104: Deactivate returned enemy
```csharp
enemy.gameObject.SetActive(false);
```
- Deactivates the enemy GameObject

### Step 105: Reset enemy color
```csharp
enemy.GetComponent<Renderer>().material.color = Color.white;
```
- Resets enemy's material color to white (clean state)

### Step 106: Define destroy pooled object method
```csharp
private void OnDestroyPooledObject(EnemyController enemy)
```
- Method called when pooled object needs to be destroyed

### Step 107: Destroy enemy GameObject
```csharp
Destroy(enemy.gameObject);
```
- Permanently destroys the enemy GameObject

### Step 108: Define start next wave method
```csharp
private void StartNextWave()
```
- Method to handle wave progression

### Step 109: Increment wave counter
```csharp
currentWave++;
```
- Increases the current wave number by 1

### Step 110: Check if past first wave
```csharp
if (currentWave > 1)
```
- Checks if this is wave 2 or higher

### Step 111: Double enemy count
```csharp
enemiesPerWave *= 2;
```
- Doubles the number of enemies for increased difficulty

### Step 112: Log wave start
```csharp
Debug.Log($"--- Starting Wave {currentWave} with {enemiesPerWave} enemies! ---");
```
- Outputs wave information to console for debugging

### Step 113: Spawn enemies
```csharp
SpawnEnemies();
```
- Calls method to spawn all enemies for this wave

### Step 114: Define spawn enemies method
```csharp
private void SpawnEnemies()
```
- Method to handle enemy spawning

### Step 115: Set active enemy count
```csharp
activeEnemies = enemiesPerWave;
```
- Sets the active enemy counter to current wave's enemy count

### Step 116: Enemy spawning loop
```csharp
for (int i = 0; i < enemiesPerWave; i++)
```
- Loops to spawn all enemies for this wave

### Step 117: Get enemy from pool
```csharp
EnemyController enemy = enemyPool.Get();
```
- Retrieves an enemy from the Unity object pool

### Step 118: Define enemy died method
```csharp
public void EnemyDied(EnemyController enemy)
```
- Public method called when an enemy is destroyed

### Step 119: Return enemy to pool
```csharp
enemyPool.Release(enemy);
```
- Returns the dead enemy to the pool for reuse

### Step 120: Decrement active enemy count
```csharp
activeEnemies--;
```
- Decreases the active enemy counter by 1

### Step 121: Log enemy death
```csharp
Debug.Log($"Enemy died. Active enemies remaining: {activeEnemies}");
```
- Outputs remaining enemy count to console

### Step 122: Check for wave completion
```csharp
if (activeEnemies <= 0)
```
- Checks if all enemies in current wave are defeated

### Step 123: Log wave completion
```csharp
Debug.Log($"Wave {currentWave} complete! Waiting for next wave...");
```
- Outputs wave completion message to console

### Step 124: Start next wave
```csharp
StartNextWave();
```
- Automatically begins the next wave when current wave is complete

## Summary

This Unity Object Pool system demonstrates two different pooling approaches:

1. **Custom Object Pool** (`ObjectPool.cs` + `PooledObject.cs`): Manual implementation using Stack collection
2. **Unity's Built-in Pool** (`GameManager.cs`): Using Unity's `IObjectPool<T>` interface

The system efficiently manages bullets and enemies, preventing garbage collection issues and maintaining smooth performance during gameplay. Each enemy death triggers wave progression, with increasing difficulty through doubled enemy counts.
