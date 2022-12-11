using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/Movement/MovementDetails")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header MOVEMENT DETAILS
    [Space(10)]
    [Header("MOVEMENT DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("The minimum move speed. The GetMoveSpeed method calculate random value between min and max")]
    #endregion
    public float minMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("The minimum move speed. The GetMoveSpeed method calculate random value between min and max")]
    #endregion
    public float maxMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("If there is a dash movement - This is the dash speed")] 
    #endregion
    public float dashSpeed; //For Player
    
    #region Tooltip
    [Tooltip("If there is a dash movement - This is the dash distance")] 
    #endregion
    public float dashDistance; //For Player

    #region Tooltip
    [Tooltip("If there is a dash movement - Cooldown time in seconds between roll actions")] 
    #endregion
    public float dashCooldownTime; //For Player


    public float GetMoveSpeed()
    {
        if(minMoveSpeed == maxMoveSpeed) return minMoveSpeed;
        
        else return Random.Range(minMoveSpeed,maxMoveSpeed);
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);

        if(dashDistance != 0f || dashSpeed != 0 || dashCooldownTime != 0)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashDistance), dashDistance, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashSpeed), dashSpeed, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashCooldownTime), dashCooldownTime, false);
        }
    }

#endif
    #endregion
}
