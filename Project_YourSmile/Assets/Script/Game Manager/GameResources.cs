using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;
    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    public RoomNodeTypeListSO roomNodeTypeList;
    public CurrentPlayerSO currentPlayer;

    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    #region  Tooltip
    [Tooltip("Dimmed Material")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("Sprite-lit-Default Material")]
    #endregion
    public Material litMaterial;

    #region Tooltip
    [Tooltip("Populate with the Variable Lit Shader")]
    #endregion
    public Material variableLitShader;

    #region Header SPECIAL TILEMAP TILES
    [Space(10)]
    [Header("SPEACIAL TILEMAP TILES")]
    #endregion
    #region Tooltip
    [Tooltip("Collision tiles that the enemies can navigate to")]
    #endregion
    public TileBase[] enemyUnwalkableCollisionTilesArray;
    
    #region Tooltip
    [Tooltip("Preferred path tile for enemy navigation")]
    #endregion
    public TileBase preferredEnemypathTile;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(roomNodeTypeList), roomNodeTypeList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(currentPlayer), currentPlayer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(litMaterial), litMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(dimmedMaterial), dimmedMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(variableLitShader), variableLitShader);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyUnwalkableCollisionTilesArray), enemyUnwalkableCollisionTilesArray);
        HelperUtilities.ValidateCheckNullValue(this, nameof(preferredEnemypathTile), preferredEnemypathTile);
    }

#endif
    #endregion
}


