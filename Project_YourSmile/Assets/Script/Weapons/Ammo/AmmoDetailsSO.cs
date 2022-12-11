using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    #region Header BASIC AMMO DETAILS
    [Space(10)]
    [Header("BASIC AMMO DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("Name for the ammo")]
    #endregion
    public string ammoName;
    public bool isPlayerAmmo;
    
    
    #region Header AMMO SPRITE, PREFABS & MATERIALS
    [Space(10)]
    [Header("AMMO SPRITE, PREFABS & MATERIALS")]
    #endregion
    #region Tooltip
    [Tooltip("Sprite to be used for the ammo")]
    #endregion
    public Sprite ammoSprite;

    #region Tooltip
    [Tooltip("Populate with the prefabs to be used for the ammo. If multiple prefabs are specified then a random prefab from the array will be selected. The prefab can be an ammo pattern - as long as it conforms to the IFireable interface")]
    #endregion
    public GameObject[] ammoPrefabArray;

    #region Tooltip
    [Tooltip("The material to be used for the ammo")]
    #endregion
    public Material ammoMaterial;

    #region Tooltip
    [Tooltip("If the ammo should 'Charge' briefly before moving the set the time in seconds that the ammo is held charging after firing before release")]
    #endregion
    public float ammoChargeTime = 0.1f;

    #region Tooltip
    [Tooltip("If the ammo har a charge time then specify what material should be used to render the ammo while charging")]
    #endregion
    public Material ammoChargeMaterial;

    #region Header AMMO BASE PARAMETERS
    [Space(10)]
    [Header("AMMO BASE PARAMETERS")]
    #endregion
    #region Tooltip
    [Tooltip("The damage each ammo deals")]
    #endregion
    public int ammoDamage = 1;

    #region Tooltip
    [Tooltip("The minimum speed of the ammo - the speed will be random value between the min max")]
    #endregion
    public float ammoSpeedMin = 20f;
    
    #region Tooltip
    [Tooltip("The maximum speed of the ammo - the speed will be random value between the min max")]
    #endregion
    public float ammoSpeedMax = 20f;

    #region Tooltip
    [Tooltip("The range of the ammo (or ammo pattern) in unity units")]
    #endregion
    public float ammoRange = 20f;

    #region Tooltip
    [Tooltip("The rotation speed in degrees per second of the ammo pattern")]
    #endregion
    public float ammoRotationSpeed = 1f;
    
    #region Header AMMO SPREAD DETAILS
    [Space(10)]
    [Header("AMMO SPREAD DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("This is the minimum spread angle of the ammo, Higher spread means less accuracy. the spread will be random value between the min max ")]
    #endregion
    public float ammoSpreadMin = 0f;

    #region Tooltip
    [Tooltip("This is the maximum spread angle of the ammo, Higher spread means less accuracy. the spread will be random value between the min max ")]
    #endregion
    public float ammoSpreadMax = 0f;

    #region Header AMMO SPAWN DETAILS
    [Space(10)]
    [Header("AMMO SPAWN DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("this is minimum number of ammo that spawned per shot. A random number of ammo are spawned between the minimum and maximum ")]
    #endregion
    public int ammoSpawnAmountMin = 1;

    #region Tooltip
    [Tooltip("this is maximum number of ammo that spawned per shot. A random number of ammo are spawned between the minimum and maximum ")]
    #endregion
    public int ammoSpawnAmountMax = 1;

    #region Tooltip
    [Tooltip("this is minimum spawn interval time. The time interval in seconds between spawned ammo is a random value between the minimum and maximum values specified")]
    #endregion
    public int ammoSpawnIntervalMin = 1;

    #region Tooltip
    [Tooltip("this is maximum spawn interval time. The time interval in seconds between spawned ammo is a random value between the minimum and maximum values specified")]
    #endregion
    public int ammoSpawnIntervalMax = 1;

    #region Header AMMO TRAIL PARAMETERS
    [Space(10)]
    [Header("AMMO TRAIL PARAMETERS")]
    #endregion
    #region Tooltip
    [Tooltip("Selected if an ammo trail is required, otherwise deselec. if selected then the rest of the ammo trail values should be populated.")]
    #endregion
    public bool isAmmoTrail = false;
    
    #region Tooltip
    [Tooltip("Ammo trail lifetime in seconds. ")]
    #endregion
    public float ammoTrailTime = 3f;
    
    #region Tooltip
    [Tooltip("Ammo trail material ")]
    #endregion
    public Material ammoTrailMaterial;

    #region Tooltip
    [Tooltip("The starting width for the ammo trail.")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    
    #region Tooltip
    [Tooltip("The ending width for the ammo trail. ")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailEndWidth;
    
    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoSprite), ammoSprite);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoPrefabArray), ammoPrefabArray);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoMaterial), ammoMaterial);
        
        if(ammoChargeTime> 0)
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoChargeMaterial), ammoChargeMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoDamage), ammoDamage, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpeedMin), ammoSpeedMin, nameof(ammoSpeedMax), ammoSpeedMax, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoRange), ammoRange, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpreadMin), ammoSpreadMin, nameof(ammoSpreadMax), ammoSpreadMax, true);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpawnAmountMin), ammoSpawnAmountMin, nameof(ammoSpawnAmountMax), ammoSpawnAmountMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpawnIntervalMin), ammoSpawnIntervalMin, nameof(ammoSpawnIntervalMax), ammoSpawnIntervalMax, true);
        
        if(isAmmoTrail)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailTime), ammoTrailTime, false );
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailStartWidth), ammoTrailStartWidth, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailEndWidth), ammoTrailEndWidth, false);
        }
    }

#endif
    #endregion
}