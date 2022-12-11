using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_",menuName = "Scriptable Objects/Player/PlayerDetails")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header PLAYER BASE DETAILS
    [Space(10)]
    [Header("PLAYER BASE DETAILS")]
    #endregion PLAYER BASE DETAILS
    #region Tooltip
    [Tooltip("Player character name")]
    #endregion Tooltip
    public string playerCharacterName;
    
    #region Tooltip
    [Tooltip("Prefabs gameobject for player")]
    #endregion Tooltip
    public GameObject playerPrefab;
    
    #region Tooltip
    [Tooltip("Player runtime animator controller")]
    #endregion Tooltip
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header HEALTH
    [Space(10)]
    [Header("HEALTH")]
    #endregion HEALTH
    #region Tooltip
    [Tooltip("Player starting health amount")]
    #endregion Tooltip
    public int playerHealthAmount;

    #region Header WEAPON
    [Space(10)]
    [Header("WEAPON")]
    #endregion
    #region Tooltip
    [Tooltip("Player initial starting weapon")]
    #endregion
    public WeaponDetailsSO startingWeapon;

    #region Tooltip
    [Tooltip("Populate with the list of starting weapons")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList;


    #region Header OTHER
    [Space(10)]
    [Header("OTHER")]
    #endregion OTHER
    #region Tooltip
    [Tooltip("Player icon sprite for minimap")]
    #endregion Tooltip
    public Sprite playerMiniMapIcon;

    #region Tooltip
    [Tooltip("Player hand sprite")]
    #endregion Tooltip
    public Sprite playerHandSprite;

    #region Header Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerCharacterName), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount),playerHealthAmount,false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMiniMapIcon),playerMiniMapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);
    }

#endif
    #endregion 
    

}
