using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapons/Weapon Details" )]
public class WeaponDetailsSO : ScriptableObject
{
    #region Header WEAPON BASE DETAILS
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]
    #endregion Header WEAPON BASE DETAILS
    #region Tooltip
    [Tooltip("Weapon name")]
    #endregion Tooltip
    public string weaponName;

    #region Tooltip
    [Tooltip("Sprite for weapon")]
    #endregion Tooltip
    public Sprite weaponSprite;

    #region Header WEAPON CONFIGURATION
    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    #endregion Header WEAPON CONFIGURATION
    #region Tooltip
    [Tooltip("Weapon Shoot Position - the offset position for the end of the weapon from the sprite pivot point")]
    #endregion
    public Vector3 weaponShootPosition;

    #region Tooltip
    [Tooltip("Weapon Current Ammo")]
    #endregion
    public AmmoDetailsSO weaponCurrentAmmo;


    #region Header WEAPON OPERATING VALUES
    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]
    #endregion Header WEAPON OPERATING VALUES
    #region Tooltip
    [Tooltip("Select if the weapon Infinite ammo")]
    #endregion
    public bool hasInfiniteAmmo = false;

    #region Tooltip
    [Tooltip("Select if the weapon have Infinite Clip")]
    #endregion
    public bool hasInfiniteClipCapacity = false;

    #region Tooltip
    [Tooltip("Weapon ammo clip capacity - shot before reload")]
    #endregion
    public int weaponClipAmmoCapacity = 6;

    #region Tooltip
    [Tooltip("Weapon ammo capacity - maximum ammo can held for this weapon")]
    #endregion
    public int weaponAmmoCapacity = 100;

    #region Tooltip
    [Tooltip("Weapon fire rate - 0.2 means 5 shot per second")]
    #endregion
    public float weaponFireRate = 0.2f;

    #region Tooltip
    [Tooltip("Weapon Precharge Time - time ini seconds to hold fire button down before firing")]
    #endregion
    public float weaponPrechargeTime = 0f;

    #region Tooltip
    [Tooltip("Weapon reload time in second")]
    #endregion
    public float weaponReloadTime = 0f;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if(!hasInfiniteAmmo)
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        

        if(!hasInfiniteClipCapacity)
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        
    }
    

#endif
    #endregion

}
