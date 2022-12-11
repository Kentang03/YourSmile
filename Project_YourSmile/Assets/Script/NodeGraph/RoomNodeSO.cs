using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    #region Editor Code
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;


    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = System.Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        //Load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    public void Draw(GUIStyle nodeStyle)
    {
        //Draw Node Box Using Begin Area
        GUILayout.BeginArea(rect, nodeStyle);

        //Start Region to Detect Popup Selection Changes
        EditorGUI.BeginChangeCheck();

        //if the room node has a parent or is of type entrance then display a label else display pop up
        if(parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            //Display a label that cant be changed
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }

        else
        {
            //Display a popup using the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

            roomNodeType = roomNodeTypeList.list[selection];
        }

        if(EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();
    }

    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if(roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }

        return roomArray;
    }

    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
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
        //left click down
        if(currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }

        //right click down
        if(currentEvent.button == 1)
        {
            ProcessRightClickDownEvent(currentEvent);
        }

    }

    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;

        //toggle node selection
        if(isSelected == true)
        {
            isSelected = false;
        }
        else
        {
            isSelected = true;
        }

    }

    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }

    private void ProcessLeftClickUpEvent()
    {
        if(isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }

    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;

        DragNode(currentEvent.delta);
        GUI.changed = true;
    }

    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }

    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        if(IsChildRoomValid(childID))
        {
            childRoomNodeIDList.Add(childID);
            return true;
        }

        return false;        
    }

    public bool IsChildRoomValid(string childID)
    {
        bool isConnectedBossNodeAlready = false;
        //Check if there is aldready a connected boss room in the node graph
        foreach(RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            if(roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
            {
                isConnectedBossNodeAlready = true;
            }
        }

        //if the child node has a type of boss room and ther is already a connected boss room node then return false
        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeAlready)
            return false;
        
        //if the child note has atype of none then return false
        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false;

        //if the node already has a child with this child ID return false
        if (childRoomNodeIDList.Contains(childID))
            return false;

        //if this node ID and the child ID are the same return false
        if(id == childID)
            return false;

        //if the child node already has a parent return false
        if(roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;

        //if child corridor and this node is a corridor return false
        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;
        
        //if child is not a corridor and this node is not a corridor return false
        if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;

        //if adding corridor check that this node has < the max permitted child corridor
        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count >= GameSettings.maxChildCorridors)
            return false;

        //if the child room is an entrance return false - the entrance must always be the top level parent node
        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;

        //If adding a room to a corridor check that this corridor node doesnt already have a room added
        if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
            return false;

        return true;       

    }


    public bool AddParentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }

    public bool RemoveChildRoomNodeIDFromRoomNode(string childID)
    {
        if(childRoomNodeIDList.Contains(childID))
        {
            childRoomNodeIDList.Remove(childID);
            return true;
        }
        return false;
    }

    public bool RemoveParentRoomNodeIDFromRoomNode(string parentID)
    {
        if(parentRoomNodeIDList.Contains(parentID))
        {
            parentRoomNodeIDList.Remove(parentID);
            return true;
        }
        return false;
    }



#endif

    #endregion Editor 

}