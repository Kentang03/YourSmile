using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    #region UNITS
    public const float pixelPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion

    #region ROOM SETTINGS
    public const float fadeInTime = 0.5f;
    public const int maxChildCorridors = 3; //Max number of child corridors leading from a room. 
    
    public const float doorUnlockDelay = 1f;
    #endregion

    #region ANIMATOR PARAMETERS
    //Animator Parameters - Player

    /*
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    */
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    //public static int isDashLeft = Animator.StringToHash("isMoving");
    //public static int isDashRight = Animator.StringToHash("isMoving");
    //public static int isDashUp = Animator.StringToHash("isMoving");
    //public static int isDashDown = Animator.StringToHash("isMoving");


    
    public static float baseSpeedForPlayerAnimations = 8f;
    

    //Animator parameters - Enemy
    public static float baseSpeedForEnemyAnimations = 3f;

    //Animator Parameters - Door
    public static int DoorOpen = Animator.StringToHash("open");

    //Animator Parameters - Books
    //public static int BookOpen = Animator.StringToHash("BookOpen");
    

    
    #endregion

    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion

    #region FIRING CONTROL
    public const float useAimAngleDistance = 3.5f; //if the target distance is less than this then the aim angle will be used (calculated from player), else the weapon aim angle will be used (calculated from the weapon shoot position).
    #endregion

    #region ASTAR PATHFINDING
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;
    #endregion

    #region ENEMY PARAMETERS
    public const int defaultEnemyHealth = 20;
    #endregion

    #region CONTACT DAMAGE PARAMETERS
    public const float contactDamageCollisionResetDelay = 0.5f;
    #endregion
}