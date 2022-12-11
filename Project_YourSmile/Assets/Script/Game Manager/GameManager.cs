using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region  Header DUNGEON LEVELS
    [Space(10)]
    [Header("DUNGEON LEVELS")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with the dungeon level scriptable objects")]
    #endregion
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("Populate with the starting dungeon level for testing, first level = 0")]
    #endregion
    [SerializeField] private int currentDungeonLevelListIndex = 0;
    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;

    protected override void Awake()
    {
        base.Awake();

        //Set player details - saved in current player SO from main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

         InstantiatePlayer();

    }

    private void InstantiatePlayer()
    {
        //Instantiate player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        //Initialize player
        player = playerGameObject.GetComponent<Player>();

        player.Initialize(playerDetails);
    }

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
        SetCurrentRoom(roomChangedEventArgs.room);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();

        ////TEST ONLY
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    gameState = GameState.gameStarted;
        //}
    }

    private void HandleGameState()
    {
        //Handle game state
        switch(gameState)
        {
            case GameState.gameStarted:
                //Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);
                
                gameState = GameState.playingLevel;
                break;
        }
    }

    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;

        ////Debug
        //Debug.Log(room.prefab.name.ToString());
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        //Build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if(!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified room and node graphs");
        }

        //Call Static event that room has changed
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        //Set player roughly mid-room
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0);

        //Get nearest spawn point in room nearest to player
        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);

    }

#endif
    #endregion



}
