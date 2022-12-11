using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private GUIStyle roomNodeSelectedStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;

    private Vector2 graphOffset;
    private Vector2 graphDrag;

    private RoomNodeSO currentRoomNode = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    
    //Node Layout values;
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    //Connecting line values
    private const float connectingLineWidth = 3f;
    private const float connectingLineArrowSize = 6f;

    //Grid Spacing
    private const float gridLarge = 100f;
    private const float gridSmall = 25f;
    
    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]

    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    private void OnEnable()
    {
        //Subscribe to the inspector selection changed event
        Selection.selectionChanged += InspectorSelectionChanged;

        //Define node layout style
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);

        //Define selected node style
        roomNodeSelectedStyle = new GUIStyle();
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        roomNodeSelectedStyle.normal.textColor = Color.white;
        roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeSelectedStyle.border = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);


        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }
    
    private void OnDisable()
    {
        //Unsubscribe to the inspector selection changed event
        Selection.selectionChanged -= InspectorSelectionChanged;
    }

    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if(roomNodeGraph != null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }
        return false;
    }

    private void OnGUI() 
    {
        
        //if scriptable object of type RoomNodeGraphSO has been selected then process
        if(currentRoomNodeGraph != null)
        {
            //Draw Grid
            DrawBackgroundGrid(gridSmall, 0.2f, Color.gray);
            DrawBackgroundGrid(gridLarge, 0.3f, Color.gray);

            //Draw line if being dragged
            DrawDraggedLine();
            //Process event
            ProcessEvents(Event.current);
            
            //Draw Connections Between Room Nodes
            DrawRoomConnections();

            //Draw Room Nodes
            DrawRoomNodes();
        }

        if(GUI.changed)
            Repaint();
            
    }

    private void DrawBackgroundGrid(float gridSize, float gridOpacity, Color gridColor)
    {
        int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
        int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);

        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        graphOffset += graphDrag * 0.5f;

        Vector3 gridOffset = new Vector3(graphOffset.x % gridSize, graphOffset.y % gridSize, 0);

        for(int i = 0; i < verticalLineCount; i++)
        {
            Handles.DrawLine(new Vector3(gridSize * i, -gridSize,0) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);

        }

        for(int j = 0; j < horizontalLineCount; j++)
        {
            Handles.DrawLine(new Vector3(-gridSize, gridSize * j,0) + gridOffset, new Vector3(position.width + gridSize, gridSize * j, 0f) + gridOffset);
            
        }

        Handles.color = Color.white;
         
    }

    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero)
        {
            //Draw line from node to line position
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, 
            currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.white, null, connectingLineWidth);

        }
    }

    private void ProcessEvents(Event currentEvent)
    {
        //Reset graph drag
        graphDrag = Vector2.zero;

        //Get Room node that mouse is over if its null or not currently being dragged
        if(currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        {
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }

        //if mouse isnt over a room node
        if(currentRoomNode == null ||currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
        }

        else
        {
            currentRoomNode.ProcessEvents(currentEvent);
        }
        
       
    }

    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for(int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)
        {
            if(currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
            {
                return currentRoomNodeGraph.roomNodeList[i];
            }
        }

        return null;
    }

    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            //Process Mouse Down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            
            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //Process right click mouse down on graph event (show context menu)
        if(currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }

        //Process left mouse down on graph event
        else if(currentEvent.button == 0)
        {
            ClearLineDrag();
            ClearAllSelectedRoomNodes();
        }
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNodes);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Delete Selected Room Node Links"), false, DeleteSelectedRoomNodeLinks);
        menu.AddItem(new GUIContent("Delete Selected Room Nodes"), false, DeleteSelectedRoomNodes);

        menu.ShowAsContext();
    }

    private void CreateRoomNode(object mousePositionObject)
    {
        //if Current node graph empty then add entrance room node first
        if(currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            CreateRoomNode(new Vector2(200f,200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        }

        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        //create room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        //add room node to current room node graph room node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        //set room node values
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        //add room node to room node graph scriptable object asset database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets(); 

        //Refresh graph node dictionary
        currentRoomNodeGraph.OnValidate();
    }

    private void DeleteSelectedRoomNodes()
    {
        Queue<RoomNodeSO> roomNodeDeletionQueue = new Queue<RoomNodeSO>();

        //Iterate through all nodes
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected && !roomNode.roomNodeType.isEntrance)
            {
                roomNodeDeletionQueue.Enqueue(roomNode);

                //iterate through child room node ids
                foreach(string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    //Retrieve child room node
                    RoomNodeSO childRoomNode = currentRoomNodeGraph.GetRoomNode(childRoomNodeID);

                    if(childRoomNode != null)
                    {
                        //Remove parentID from child room node
                        childRoomNode.RemoveParentRoomNodeIDFromRoomNode(roomNode.id);
                    }
                }

                //Iterate thrugh parent room node ids
                foreach(string parentRoomNodeID in roomNode.parentRoomNodeIDList)
                {
                    //Retrieve parent room node
                    RoomNodeSO parentRoomNode = currentRoomNodeGraph.GetRoomNode(parentRoomNodeID);

                    if(parentRoomNode != null)
                    {
                        //Remove ChildID from parent node
                        parentRoomNode.RemoveChildRoomNodeIDFromRoomNode(roomNode.id);
                    }
                }
            }
        }
        
        //Delete queued room nodes
        while(roomNodeDeletionQueue.Count > 0)
        {
            //Get room node from queue
            RoomNodeSO roomNodeToDelete = roomNodeDeletionQueue.Dequeue();

            //Remove node from dictionary
            currentRoomNodeGraph.roomNodeDictionary.Remove(roomNodeToDelete.id);

            //Remove node from list
            currentRoomNodeGraph.roomNodeList.Remove(roomNodeToDelete);

            //Remove node from asset database
            DestroyImmediate(roomNodeToDelete, true);

            //Save asset database
            AssetDatabase.SaveAssets();
        }
    }

    private void DeleteSelectedRoomNodeLinks()
    {
        //Iterate through all room nodes
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            for(int i = roomNode.childRoomNodeIDList.Count - 1; i >= 0; i--)
            {
                //Get child room node
                RoomNodeSO childRoomNode = currentRoomNodeGraph.GetRoomNode(roomNode.childRoomNodeIDList[i]);

                //If the child room node is selected
                if(childRoomNode != null && childRoomNode.isSelected)
                {
                    //remove childID from parent room node
                    roomNode.RemoveChildRoomNodeIDFromRoomNode(childRoomNode.id);

                    //remove parentID from child room node
                    childRoomNode.RemoveParentRoomNodeIDFromRoomNode(roomNode.id);
                    
                }
            }
        }
    }

    private void ClearAllSelectedRoomNodes()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if(roomNode.isSelected)
            {
                roomNode.isSelected = false;

                GUI.changed = true;
            }
        }
    }

    private void SelectAllRoomNodes()
    {
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = true;
        }
        GUI.changed = true;
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if(currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            //check if over a room node
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);
            if(roomNode != null)
            {
                //if so set it as a child of the parent room node if it can be added
                if(currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    //Set parent ID in child room node
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }
            ClearLineDrag();
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }

        else if(currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent.delta);
        }
    }

    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        if(currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    private void ProcessLeftMouseDragEvent(Vector2 dragDelta)
    {
        graphDrag = dragDelta;

        for(int i = 0; i < currentRoomNodeGraph.roomNodeList.Count; i++)
        {
            currentRoomNodeGraph.roomNodeList[i].DragNode(dragDelta);
        }

        GUI.changed = true;
    }


    private void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePosition += delta;
    }

    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }

    private void DrawRoomConnections()
    {
        //Loop through all room nodes
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if(roomNode.childRoomNodeIDList.Count > 0)
            {
                //Loop thruogh child room nodes
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    //get child room node from dictionary
                    if(currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
                    {
                        DrawConectionLine(roomNode,currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);

                        GUI.changed = true;
                    }
                }
            }
        }
    }

    private void DrawConectionLine(RoomNodeSO parentRoomNode, RoomNodeSO childRoomNode)
    {
        //get line start and end position
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition = childRoomNode.rect.center;

        //Draw Arrow
        //calculate midway point
        Vector2 midPosition = (startPosition + endPosition) / 2f;

        //Vector from start to end position of line
        Vector2  direction = endPosition - startPosition;

        //Calculate normalised perpendicular position from the mid point
        Vector2 arrowTailPoint1 = midPosition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowTailPoint2 = midPosition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        
        //Calculate mid point offset position for arrow head
        Vector2 arrowHeadPoint= midPosition + direction.normalized * connectingLineArrowSize;
        
        //Draw Arrow
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, connectingLineWidth);
        

        //Draw line
        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, connectingLineWidth);

        GUI.changed = true;
    }

    private void DrawRoomNodes()
    {
        //Loop through all room nodes and draw them
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if(roomNode.isSelected)
            {
                roomNode.Draw(roomNodeSelectedStyle);
            }

            else
            {
                roomNode.Draw(roomNodeStyle);
            }
            
        }

        GUI.changed = true;
    }

    private void InspectorSelectionChanged()
    {
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;

        if(roomNodeGraph != null)
        {
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }

}
 