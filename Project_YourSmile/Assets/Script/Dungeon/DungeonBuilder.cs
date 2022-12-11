using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DungeonBuilder : SingletonMonobehaviour<DungeonBuilder>
{
    public Dictionary<string, Room> dungeonBuilderRoomDictionary = new Dictionary<string, Room>();
    private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
    private List<RoomTemplateSO> roomTemplateList = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    private bool dungeonBuildSuccessful;

     private void OnEnable()
    {
        // Set dimmed material to off
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 0f);
    }

    private void OnDisable()
    {
        // Set dimmed material to fully visible
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 1f);
    }

    protected override void Awake()
    {
        base.Awake();

        //Load the room node type list
        LoadRoomNodeTypeList();

    }

    //Load the room node type list
    private void LoadRoomNodeTypeList()
    {
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomTemplateList = currentDungeonLevel.roomTemplateList;

        //Load the scriptable objects room templates into the dictionary
        LoadRoomTemplatesIntoDictionary();

        dungeonBuildSuccessful = false;
        int dungeonBuildAttempts = 0;

        while(!dungeonBuildSuccessful && dungeonBuildAttempts < GameSettings.maxDungeonBuildAttempts)
        {
            dungeonBuildAttempts++;

            //Select a random node graph from the list
            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);

            int dungeonRebuildAttemptsForNodeGraph = 0;
            dungeonBuildSuccessful = false;

            //Loop until dungeon successfully built or more than max attempts for node graphs
            while(!dungeonBuildSuccessful && dungeonRebuildAttemptsForNodeGraph <= GameSettings.maxDungeonRebuildAttemptsForRoomGraph)
            {
                //Clear dungeon room gameobjects and dungeon room dictionary
                ClearDungeon();

                dungeonRebuildAttemptsForNodeGraph++;

                //Attempts to build a random dungeon for the selected room node graph
                dungeonBuildSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);

            }

            if(dungeonBuildSuccessful)
            {
                //Instantiate Room GameObject
                InstantiateRoomGameObjects();
            }

        }
        
        return dungeonBuildSuccessful;
    }

    private void LoadRoomTemplatesIntoDictionary()
    {
        //Clear room template dictionary
        roomTemplateDictionary.Clear();

        //Load room template list into dictionary
        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if(!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log("Duplicate Room Template Key In " + roomTemplateList);
            }
        }
    }

    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        //Create Open Room Node Queue
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        //Add Entrance node to room node queue from room node graph
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(x => x.isEntrance));

        if(entranceNode != null)
        {
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            Debug.Log("No Entrance Node");
            return false;
        }

        //Start with no room overlaps
        bool noRoomOverlaps = true;

        //Process open room ndoe queue
        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);

        //if all the room nodes have been processed and there hasnt been a room overlap the return true
        if(openRoomNodeQueue.Count == 0 && noRoomOverlaps)
        {
            return true;
        }
        else 
        {
            return false;
        }


    }

    private IEnumerable<Doorway> GetUnconnectedAvailableDoorways(List<Doorway> roomDoorwayList)
    {
        //Loop through doorway list
        foreach(Doorway doorway in roomDoorwayList)
        {
            if(!doorway.isConnected && !doorway.isUnavailable)
            {
                yield return doorway;
            }
        }
    }

    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph, Queue<RoomNodeSO> openRoomNodeQueue, bool noRoomOverlaps)
    {
        //While room nodes in open room node queue & no room overlaps detected.
        while(openRoomNodeQueue.Count > 0 && noRoomOverlaps == true)
        {
            //Get next room node from open room node queue
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();

            //Add child Nodes to queue from room node graph (with links to this parent Room)
            foreach(RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }

            //If the room is the entrance mark as positioned and add to room dictionary
            if(roomNode.roomNodeType.isEntrance)
            {
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);

                Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);

                room.isPositioned = true;

                //Add room to room dictionary
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }

            //else if the room type isnt an entrance
            else
            {
                //Else get parent room for node
                Room parentRoom = dungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];

                //See if room can be placedf without overlaps
                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }
        }

        return noRoomOverlaps;

    }

    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
    {
        //Initialise and assume overlap until proven
        bool roomOverlaps = true;

        //Do while room overlaps - try to place against all available doorways of the parent untul
        //the room is successfully placed w/o overlap
        while(roomOverlaps)
        {
            //Select random unconnected availab le doorway for paren
            List<Doorway> unconnectedAvailableParentDoorways = GetUnconnectedAvailableDoorways(parentRoom.doorWayList).ToList();

            if(unconnectedAvailableParentDoorways.Count == 0)
            {
                //if no more doorways to try then overlap failure
                return false; //room overlaps
            }

            Doorway doorwayParent = unconnectedAvailableParentDoorways[UnityEngine.Random.Range(0, unconnectedAvailableParentDoorways.Count)];

            //Get a random room template for room node that is consistent with the parent door orientation
            RoomTemplateSO roomTemplate = GetRandomTemplateForRoomConsistentWithParent(roomNode, doorwayParent);

            //Create a room
            Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);

            //Place the room - return true if the room doesnt overlap
            if(PlaceTheRoom(parentRoom, doorwayParent, room))
            {
                //If room doesnt overlap then set to false to exit while loop
                roomOverlaps = false;

                //Mark room as positioned
                room.isPositioned = true;

                //Add room to dictionary
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }

        return true; //no room overlaps

    }

    //Get random room template for room node taking into account the parent doorway orientation
    private RoomTemplateSO GetRandomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode, Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;

        //if room node is a corridor then select a random correct corridor room template based on
        //parent doorway orientation
        if(roomNode.roomNodeType.isCorridor)
        {
            switch(doorwayParent.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNS));
                    break;
                case Orientation.east:
                case Orientation.west:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;

                case Orientation.none:
                    break;

                default:
                    break;
            }
        }

        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }

        return roomTemplate;
    }

    //Place the room - return true if the room doesnt overlap, otherwise false
    private bool PlaceTheRoom(Room parentRoom, Doorway doorwayParent, Room room)
    {
        //Get current room doorway position
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.doorWayList);

        //Return if no doorway in room opposite to parent doorway
        if(doorway == null)
        {
            doorwayParent.isUnavailable = true;
            return false;
        }

        //Calculate 'world' grid parent doorway position
        Vector2Int parentDoorwayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;

        Vector2Int adjustment = Vector2Int.zero;

        //Calculate  adjustment position offset based on room doorway position that we are trying to connect (e.g. if this doorway is west then we need to add (1.0) to the east parent doorway)
        switch (doorway.orientation)
        {
            case Orientation.north:
                adjustment = new Vector2Int(0, -1);
                break;
            
            case Orientation.east:
                adjustment = new Vector2Int(-1, 0);
                break;
            
            case Orientation.south:
                adjustment = new Vector2Int(0, 1);
                break;
            
            case Orientation.west:
                adjustment = new Vector2Int(1, 0);
                break;

            case Orientation.none:
                break;

            default:
                break;
        }

        //Calculate room lower bounds and upper bounds based on positioning to align with parent doorway
        room.lowerBounds = parentDoorwayPosition + adjustment + room.templateLowerBounds - doorway.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        Room overlappingRoom = CheckForRoomOverlap(room);

        if(overlappingRoom == null)
        {
            //mark doorways as connected & unavailable
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;

            doorway.isConnected = true;
            doorway.isUnavailable = true;

            //return true to show  rooms have been connected
            return true;
        }

        else
        {
            //just mark the parent doorway as unavailable so we dont try and connect it again
            doorwayParent.isUnavailable = true;
            
            return false;
        }
    }

    

    private Doorway GetOppositeDoorway(Doorway parentDoorway, List<Doorway> doorwayList)
    {
        foreach(Doorway doorwayToCheck in doorwayList)
        {
            if(parentDoorway.orientation == Orientation.east && doorwayToCheck.orientation == Orientation.west)
            {
                return doorwayToCheck;
            }
            else if(parentDoorway.orientation == Orientation.west && doorwayToCheck.orientation == Orientation.east)
            {
                return doorwayToCheck;
            }
            else if(parentDoorway.orientation == Orientation.north && doorwayToCheck.orientation == Orientation.south)
            {
                return doorwayToCheck;
            }
            else if(parentDoorway.orientation == Orientation.south && doorwayToCheck.orientation == Orientation.north)
            {
                return doorwayToCheck;
            }
            
        }

        return null;

    }

    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach(KeyValuePair<string, Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            //skip if same room as room to test or room hasnt been positioned
            if(room.id == roomToTest.id || !room.isPositioned)
                continue;

            if(IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }

        return null;
    }

    //Check if 2 room overlap each other - return true if overlap, otherwise false
    private bool IsOverLappingRoom(Room room1, Room room2)
    {
        bool IsOverlappingX = IsOverlappingInterval(room1.lowerBounds.x, room1.upperBounds.x, room2.lowerBounds.x, room2.upperBounds.x);

        bool IsOverlappingY = IsOverlappingInterval(room1.lowerBounds.y, room1.upperBounds.y, room2.lowerBounds.y, room2.upperBounds.y);
        
        if(IsOverlappingX && IsOverlappingY)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    //Check if interval 1 overlap interval 2 - this method is used by the IsOverlapping Method
    private bool IsOverlappingInterval(int imin1, int imax1, int imin2, int imax2)
    {
        if(Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Get a random room template for room node taking into account the parent doorway orientation
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();

        //Loop through room template list
        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            //Add matching room templates
            if(roomTemplate.roomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }

        //Return null if list is zero
        if(matchingRoomTemplateList.Count == 0)
        {
            return null;
        }

        //Select random room template from list and return
        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];

    }

    private Room CreateRoomFromRoomTemplate(RoomTemplateSO roomTemplate,RoomNodeSO roomNode)
    {
        //Initialise room from template
        Room room = new Room();

        room.templateID = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.roomNodeType = roomTemplate.roomNodeType;
        room.lowerBounds = roomTemplate.lowerBounds;
        room.upperBounds = roomTemplate.upperBounds;
        room.spawnPositionArray = roomTemplate.spawnPositionArray;
        room.enemiesByLevelList = roomTemplate.enemiesByLevelList;
        room.roomLevelEnemySpawnParametersList = roomTemplate.roomEnemySpawnParametersList;
        room.templateLowerBounds = roomTemplate.lowerBounds;
        room.templateUpperBounds = roomTemplate.upperBounds;

        room.childRoomIDList = CopyStringList(roomNode.childRoomNodeIDList);
        room.doorWayList = CopyDoorwayList(roomTemplate.doorwayList);

        //Set parent ID for room
        if(roomNode.parentRoomNodeIDList.Count == 0)
        {
            room.parentRoomID = "";
            room.isPreviouslyVisited = true;

            //Set entrance in game manager
            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.parentRoomID = roomNode.parentRoomNodeIDList[0];
        }

        //if there are no enemies to spawn then default the room to be clear of enemies
        if (room.GetNumberOfEnemiesToSpawn(GameManager.Instance.GetCurrentDungeonLevel()) == 0)
        {
            room.isClearedOfEnemies = true;
        }
        
        return room;
    }
    private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if(roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("No room node graphs in list");
            return null;
        }
    }

    private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwayList)
    {
        List<Doorway> newDoorwayList = new List<Doorway>();

        foreach(Doorway doorway in oldDoorwayList)
        {
            Doorway newDoorway = new Doorway();

            newDoorway.position = doorway.position;
            newDoorway.orientation = doorway.orientation;
            newDoorway.doorPrefab = doorway.doorPrefab;
            newDoorway.isConnected  = doorway.isConnected;
            newDoorway.isUnavailable = doorway.isUnavailable;
            newDoorway.doorwayStartCopyPosition = doorway.doorwayStartCopyPosition;
            newDoorway.doorwayCopyTileWidth = doorway.doorwayCopyTileWidth;
            newDoorway.doorwayCopyTileHeight = doorway.doorwayCopyTileHeight;

            newDoorwayList.Add(newDoorway);
        }

        return newDoorwayList;
        
    }

    private List<string> CopyStringList(List<string> oldStringList)
    {
        List<string> newStringList = new List<string>();

        foreach(string stringValue in oldStringList)
        {
            newStringList.Add(stringValue);
        }

        return newStringList;
    }

    private void InstantiateRoomGameObjects()
    {
        foreach(KeyValuePair<string, Room> keyvaluepair in dungeonBuilderRoomDictionary)
        {
            Room room = keyvaluepair.Value;

            //Calculate room position (remember the room instantiation position needs to be adjusted by the room template lower bounds)
            Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);

            //Instantiate room
            GameObject roomGameObject = Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);

            //Get Instantiated room component from instantiated prefab
            InstantiatedRoom instantiatedRoom = roomGameObject.GetComponentInChildren<InstantiatedRoom>();

            instantiatedRoom.room = room;

            //Initialise the Instantiated Room
            instantiatedRoom.Initialise(roomGameObject);

            //Save gameobject reference
            room.instantiatedRoom = instantiatedRoom;
        }
    }

    //Get a room template by room template ID, return null if ID doesnt exist
    public RoomTemplateSO GetRoomTemplate(string roomTemplateID)
    {
        if(roomTemplateDictionary.TryGetValue(roomTemplateID, out RoomTemplateSO roomTemplate))
        {
            return roomTemplate;
        }
            
        else
        {
            return null;
        }
    }

    //Get room by roomID, if NO room exist with that ID return null
    public Room GetRoomByRoomID(string roomID)
    {
        if(dungeonBuilderRoomDictionary.TryGetValue(roomID, out Room room))
        {
            return room;
        }
            
        else
        {
            return null;
        }
            
    }

    //Clear Dungeon room gameobjects and dungeon room dictionary
    private void ClearDungeon()
    {
        //Destroy instantiated dungeon gameobjects and clear dungeon manager room dictionary
        if(dungeonBuilderRoomDictionary.Count > 0)
        {
            foreach(KeyValuePair<string, Room> keyvaluepair in dungeonBuilderRoomDictionary)
            {
                Room room = keyvaluepair.Value;

                if(room.instantiatedRoom != null)
                {
                    Destroy(room.instantiatedRoom.gameObject);
                }
            }

            dungeonBuilderRoomDictionary.Clear();
        }
    }
 
}
