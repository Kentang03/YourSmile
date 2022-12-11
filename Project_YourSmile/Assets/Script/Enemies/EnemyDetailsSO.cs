using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemies/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    #region Header BASE ENEMY DETAILS
    [Header("Enemy Details")]
    [Tooltip("Enemy name")] 
    #endregion
    public string enemyName;

    #region Tooltip
    [Tooltip("Enemy Bullet Sprite")] 
    #endregion
    public GameObject enemyPrefab;

    #region Tooltip
    [Tooltip("Distance to the player before enemy start chasing")]
    #endregion
    public float chaseDistance = 50f;

    #region Header ENEMY WEAPON SETTINGS
    [Space(10)]
    [Header("ENEMY WEAPON SETTINGS")]
    #endregion
    #region Tooltip
    [Tooltip("The weapon for the enmy - none if the enemy doesnt have a weapon")] 
    #endregion
    public WeaponDetailsSO enemyWeapon;

    #region Tooltip
    [Tooltip("The minimum time delay interval in seconds between burst of enemy shooting. This value should be greater than 0. A random value will be selected between the minimum value and the maximum value")]
    #endregion
    public float firingIntervalMin = 0.1f;

    #region Tooltip
    [Tooltip("The maximum time delay interval in seconds between burst of enemy shooting. This value should be greater than 0. A random value will be selected between the minimum value and the maximum value")]
    #endregion
    public float firingIntervalMax = 1f;

    #region Tooltip
    [Tooltip("The minimum firing duration that the enemy shoots for during a firing burst. This value should be greater than zero. A random value will be selected bet ween the minimum value and the maximum value.")] 
    #endregion
    public float firingDurationMin = 1f;

    #region Tooltip
    [Tooltip("The maximum firing duration that the enemy shoots for during a firing burst. This value should be greater than zero. A random value will be selected bet ween the minimum value and the maximum value.")] 
    #endregion
    public float firingDurationMax = 2f;

    #region Tooltip
    [Tooltip("Selec this if line of sight is required of the player before enemy fires. If line of sight isn't selected enemy will fire regardless of obstacles whenever the player is in 'range' ")] 
    #endregion
    public bool firingLineOfSightRequired;

    #region Header ENEMY HEALTH
    [Space(10)]
    [Header("ENEMY HEALTH")]
    #endregion
    #region Tooltip
    [Tooltip("The health of the enemy for each level")]
    #endregion
    public EnemyHealthDetails[] enemyHealthDetailsArray;

    #region Tooltip
    [Tooltip("Select if has immunity period immediately after being hit. if so specify the immunity time in seconds in the other fields")]
    #endregion
    public bool isImmunityAfterHit = false;

    #region Tooltip
    [Tooltip("Immunity time in seconds after being hit")]
    #endregion
    public float hitImmunityTime;
    

   

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);

        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin, nameof(firingIntervalMax), firingIntervalMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax), firingDurationMax, false);

        if(isImmunityAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }
    }
    
}