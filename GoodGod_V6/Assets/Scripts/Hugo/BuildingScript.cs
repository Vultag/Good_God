using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{

    public bool placed { get; private set; }
    public BoundsInt area;


    public bool CanBePlaced()
    {
        Vector3Int positionint = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areatemp = area;
        areatemp.position = positionint;

        if (BuildingSystem.current.CanTakeArea(areatemp))
            return true;

        return false;   

    }
    

    
}
