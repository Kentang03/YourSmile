using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        enemiesSpawnedSoFar = 0;
        currentEnemyCount = 0;

        currentRoom = roomChangedEventArgs.room;

        //if the room is a corridor or the entrance then return
        if(currentRoom.roomNodeType.isCorridorEW || currentRoom.roomNodeType.isCorridorNS || currentRoom.roomNodeType.isEntrance) return;

        if(currentRoom.isClearedOfEnemies) return;

        //Get random number of enemies to spawn
        enemiesToSpawn = currentRoom.GetNumberOfEnemiesToSpawn(GameManager.Instance.GetCurrentDungeonLevel());

        //Get room enemy spawn parameters
        roomEnemySpawnParameters = currentRoom.GetRoomEnemySpawnParameters(GameManager.Instance.GetCurrentDungeonLevel());

        //if no enemies to spawn return
        if (enemiesToSpawn == 0)
        {
            //Mark the room as cleared
            currentRoom.isClearedOfEnemies = true;
            
            return;
        }

        //Get concurrent number of enemies to spawn
        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();

        //Lock doors
        currentRoom.instantiatedRoom.LockDoors();

        //Spawn enemies
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        //Set gameState engaging enemies
        if(GameManager.Instance.gameState == GameState.playingLevel)
        {
            GameManager.Instance.previousGameState = GameState.playingLevel;
            GameManager.Instance.gameState = GameState.engagingEnemies;
        }

        StartCoroutine(SpawnEnemiesRoutine());

    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        //Create an instance of the helper class used to select a random enemy
        RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(currentRoom.enemiesByLevelList);

        //Check we have somewhere to spawn enemies
        if (currentRoom.spawnPositionArray.Length > 0)
        {
            //Loop through to create all the enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                //wait until current enemy count is less than max concurrent enemies
                while (currentEnemyCount >= enemyMaxConcurrentSpawnNumber)
                {
                    yield return null;
                }

                Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

                //Create enemy - get next enemy type to spawn
                CreateEnemy(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPosition));

                yield return new WaitForSeconds(GetEnemySpawnInterval());
            }
        }
    }

    private float GetEnemySpawnInterval()
    {
        return (Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval));
    }

    private int GetConcurrentEnemies()
    {
        return (Random.Range(roomEnemySpawnParameters.minConcurrentEnemies, roomEnemySpawnParameters.minConcurrentEnemies));
    }

    private void CreateEnemy(EnemyDetailsSO enemyDetails, Vector3 position)
    {
        //keep track of the number of enemies spawned so far
        enemiesSpawnedSoFar++;

        //add one to the current enemy count - this is reduced when an enemy destroyed
        currentEnemyCount++;

        //Get current dungeon level
        DungeonLevelSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

        //Instantiate enemy
        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, position, Quaternion.identity, transform);

        //Initalize Enemy
        enemy.GetComponent<Enemy>().EnemyInitialization(enemyDetails, enemiesSpawnedSoFar, dungeonLevel);

        //Subscribe to enemy destroyed event
        enemy.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;
    }

    private void Enemy_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;

        //reduce current enemy count
        currentEnemyCount--;

        if (currentEnemyCount <= 0 && enemiesSpawnedSoFar == enemiesToSpawn)
        {
            currentRoom.isClearedOfEnemies = true;

            //Set Game State
            if (GameManager.Instance.gameState == GameState.engagingEnemies)
            {
                GameManager.Instance.gameState = GameState.playingLevel;
                GameManager.Instance.previousGameState = GameState.engagingEnemies;
            }
            
            else if (GameManager.Instance.gameState == GameState.engagingBoss)
            {
                GameManager.Instance.gameState = GameState.bossStage;
                GameManager.Instance.previousGameState = GameState.engagingBoss;
            }

            //unlock doors
            currentRoom.instantiatedRoom.UnlockDoors(GameSettings.doorUnlockDelay);

            //Trigger room enemies defeated event
            StaticEventHandler.CallRoomEnemiesDefeatedEvent(currentRoom);
        }
    }
}
