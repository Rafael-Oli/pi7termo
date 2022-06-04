using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class RoomNodeGraphEditor : EditorWindow{

    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeSO currentRoomNode = null;
    private RoomNodeTypeListSO roomNodeTypeList;

    //node layout
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    //especifica como sera o nome da nova window
    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
    //cria a window
    private static void OpenWindow() {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    //cria o style dos node
    private void OnEnable() {
        
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line){
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if(roomNodeGraph != null){
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }
        return false;
    }


    private void OnGUI() {
        
        if(currentRoomNodeGraph != null){
            ProcessEvents(Event.current);

            DrawRoomNodes();
        }

        if(GUI.changed)
            Repaint();
    }

    private void ProcessEvents(Event currentEvent){

        if(currentRoomNode == null || currentRoomNode.isLeftClickDragging == false){

            currentRoomNode = isMouseOverRoomNode(currentEvent);
        }

        if(currentRoomNode == null){

            ProcessRoomNodeGraphEvents(currentEvent);
        } else {
            currentRoomNode.ProcessEvents(currentEvent);
        }
    } 

    private RoomNodeSO isMouseOverRoomNode(Event currentEvent){

        for(int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--){

            if(currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition)){

                return currentRoomNodeGraph.roomNodeList[i];

            }
        }
        return null;

    } 

    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            // Process Mouse Down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            // Process Mouse Up Events
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            // Process Mouse Drag Event
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);

                break;

            default:
                break;
        }
    }

    //processa o evento de click do mouse no node graph
    private void ProcessMouseDownEvent(Event currentEvent){
        if(currentEvent.button == 1){
            ShowContextMenu(currentEvent.mousePosition);
        }
    }

    //cria um menu para adicionar os nodes
    private void ShowContextMenu(Vector2 MousePosition){
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, MousePosition);

        menu.ShowAsContext();
    }

    //cria um room node
    private void CreateRoomNode(object mousePositionObject){
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    //overload
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType){
        Vector2 MousePosition = (Vector2)mousePositionObject;

        //cria o nodeSO asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        //adc o room node na lista
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        //adc valor aos room node
        roomNode.Initialise(new Rect(MousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();
    }

    private void DrawRoomNodes(){
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList){
            roomNode.Draw(roomNodeStyle);
        }

        GUI.changed = true;
    }
}

