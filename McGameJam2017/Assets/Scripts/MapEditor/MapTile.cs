using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour{
    public GameObject panel;
    public MapAreaScript mapArea;
    public BrushTool brushTool;

    public int x;
    public int y;
    //public TileType tileType;
    //Remplacer par un int pour rapidement adapter le script au deux types de floor (VR et REAL)
    public int tileEnumId;
    public int offset = (int)TileType.TOTAL; // le offset permettra de faire la difference entre les tiles VR et les tiles R
    // Donc, on a 0 a TileType.TOTAL-1 pour les tiles R et TileType.TOTAL a TileType.TOTAL+TileReal.TOTAL.

    void Awake()
    {
        panel = gameObject;
    }
    
    public void PassOver()
    {
        //If mouse is held, we apply the brush
        if(Input.GetMouseButton(0))
        {
            ApplyBrush();
            Debug.Log("Was pressed " + x +" "+ y );
        }
    }

    //Apply the brush effect to the tile
    void ApplyBrush()
    {
        SetTileType(brushTool.currentBrush);
    }

    //Will set the tile type
    void SetTileType(TileType type)
    {
        //Makes sure if we are deleting a teleport up and down to delete its corresponding portal
        if (tileEnumId == (int)TileType.TELEPORT_UP)
        {
            mapArea.UpdateTileValue(x, y, mapArea.currentFloor + 1, TileType.NORMAL);
        }
        else if (tileEnumId == (int)TileType.TELEPORT_DOWN)
        {
            mapArea.UpdateTileValue(x, y, mapArea.currentFloor - 1, TileType.NORMAL);
        }

        switch (type)
        {
            case TileType.TELEPORT_UP:
                if(mapArea.currentFloor < 2)
                {
                    mapArea.UpdateTileValue(x, y, mapArea.currentFloor, TileType.TELEPORT_UP);
                    mapArea.UpdateTileValue(x, y, mapArea.currentFloor + 1, TileType.TELEPORT_DOWN);
                }
                break;
            case TileType.TELEPORT_DOWN:
                if (mapArea.currentFloor > 0)
                {
                    mapArea.UpdateTileValue(x, y, mapArea.currentFloor, TileType.TELEPORT_DOWN);
                    mapArea.UpdateTileValue(x, y, mapArea.currentFloor - 1, TileType.TELEPORT_UP);
                }
                break;
            case TileType.TOTAL:
                break;
            default:                
                mapArea.UpdateTileValue(x, y, mapArea.currentFloor, type);
                break;
        }
    }	
}
