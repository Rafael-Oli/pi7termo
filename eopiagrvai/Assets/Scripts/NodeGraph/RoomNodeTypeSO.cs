using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject{

    public string roomNodeTypeName;

    public bool displayInNodeGraphEditor = true;
    public bool isCorridor;
    //Detects if the corridor is North or South
    public bool isCorridorNS;
    //Detects if the corridor is East or West
    public bool isCorridorEW;
    public bool isEntrance;
    public bool isBossRoom;
    //Default None
    public bool isNone;

    //esse #if siginifica q o script so vai rodar no unity_editor
    #if UNITY_EDITOR
    private void onValidate() {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
    #endif
}
