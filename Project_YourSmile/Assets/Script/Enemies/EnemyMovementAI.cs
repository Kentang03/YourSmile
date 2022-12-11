using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
   #region Tooltip
   [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed")]
   #endregion 
   [SerializeField] private MovementDetailsSO movementDetails;
   private Enemy enemy;
   private Stack<Vector3> movementSteps = new Stack<Vector3>();
   private Vector3 playerReferencePosition;
   private Coroutine moveEnemyRoutine;
   private float currentEnemyPathRebuildCooldown;
   private WaitForFixedUpdate waitForFixedUpdate;
   [HideInInspector] public float moveSpeed;
   private bool chasePlayer = false;

   private void Awake() 
   {
        enemy = GetComponent<Enemy>();
        moveSpeed = movementDetails.GetMoveSpeed();
   }

   private void Start() 
   {
        //Create waitforfixed update for use in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        //Reset player reference position
        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
   }

   private void Update()
   {
        MoveEnemy();
   }

   private void MoveEnemy()
   {
        //Movement cooldown timer
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        //Check distance to player to see if enemy should start chasing
        if(!chasePlayer && Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.enemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }

        //If not close enough to chase player then return
        if(!chasePlayer) return;

        //if the movement cooldown timer reached or player has movre than require distance
        //then rebuild the enemy path and move the enemy
        if(currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePosition, GameManager.Instance.GetPlayer().GetPlayerPosition()) > GameSettings.playerMoveDistanceToRebuildPath))
        {
            //reset path to rebuild cooldown timer
            currentEnemyPathRebuildCooldown = GameSettings.enemyPathRebuildCooldown;

            //reset player reference position
            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            //Move the enemy uisng AStar pathfinding - Trigger rebuild of path to player
            CreatePath();

            //if a path has been found move the enemy
            if(movementSteps != null)
            {
                if(moveEnemyRoutine != null)
                {
                    //Trigger idle event
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }

                //Move enemy along the path using a coroutine
                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }
        }
    }

    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while (movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            //while not very close to continue to move - when close move onto the next step
            while(Vector3.Distance(nextPosition, transform.position) > 0.2f)
            {
                //Trigger movement event
                enemy.movementToPositionEvent.CallMovementToPositionEvent(nextPosition, transform.position, moveSpeed, (nextPosition - transform.position).normalized);

                yield return waitForFixedUpdate; //moving the enemy using 2D physics so wait until next fixed update
            }

            yield return waitForFixedUpdate;
        }

        enemy.idleEvent.CallIdleEvent();
    }

    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();
        Grid grid = currentRoom.instantiatedRoom.grid;
        
        //Get player position on the grid
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom);
        //Get enemy position on the grid
        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);

        //Build a path for the enemy to move on
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        //Take off first steop on path - this is the grid square the enemy is already on
        if(movementSteps != null) 
        {
            movementSteps.Pop();
        }
        else 
        {
            //Trigger idle event - no path
            enemy.idleEvent.CallIdleEvent();
        }
                
    }

    private Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom)
    {
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

        Vector3Int playerCellPosition = currentRoom.instantiatedRoom.grid.WorldToCell(playerPosition);

        Vector2Int adjustedPlayerCellPosition = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x, playerCellPosition.y - currentRoom.templateLowerBounds.y);

        int obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y]; 
        

        //if the player isnt on a cell square marked as an obstacle then return that position
        if(obstacle != 0)
        {
            return playerCellPosition;
        }

        else
        {
            for(int i = -1; i <=1; i++)
            {
                for(int j = -1; j <=1; j++)
                {
                    if(i== 0 && j == 0) continue;

                    try
                    {
                        obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x + i, adjustedPlayerCellPosition.y + j];
                        if (obstacle != 0)
                        {
                            return new Vector3Int(playerCellPosition.x + i, playerCellPosition.y + j, 0);
                        }
                       
                    }
                    
                    catch
                    {
                        continue;
                    }
                }
            }

            return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];
        }
    }

    #region Validation
#if UNITY_EDITOR

     /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif
    #endregion
    
}
