﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapAreaScript : MonoBehaviour {
    GameObject [][] mapTiles;
    public BrushTool brushTool;

    int mapHeight = 16;
    int mapWidth = 32;

    int[,] floor1;
    int[,] floor2;
    int[,] floor3;
    public int currentFloor = 0;

    public GameObject basePanel;
    public Sprite[] textures;

	// Use this for initialization
	void Start () {

        //Instantiate map
        floor1 = new int[mapHeight, mapWidth];
        floor2 = new int[mapHeight, mapWidth];
        floor3 = new int[mapHeight, mapWidth];

        mapTiles = new GameObject[mapHeight][];
        for(int i = 0; i < mapHeight; ++i)
        {
            mapTiles[i] = new GameObject[mapWidth];
        }

        //Create the panels and set them to the tiles
		for(int i = 0; i < mapHeight; ++i)
        {
            for(int j=0; j < mapWidth; ++j)
            {
                //Instantiate the tiles
                floor1[i, j] = 0;
                floor2[i, j] = 0;
                floor3[i, j] = 0;

                var newPanel = Instantiate(basePanel);
                newPanel.transform.parent = this.transform;
                newPanel.GetComponent<MapTile>().x = j;
                newPanel.GetComponent<MapTile>().y = i;
                mapTiles[i][j] = newPanel;
            }            
        }
    }
	
	//Update map with new selected floor info
    public void SelectFloor(int floor)
    {
        currentFloor = floor;
        int[,] selectedFloor;
        switch(floor)
        {
            case 0:
                selectedFloor = floor1;
                break;
            case 1:
                selectedFloor = floor2;
                break;
            case 2:
                selectedFloor = floor3;
                break;
            default:
                selectedFloor = floor1;
                break;
        }

        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                mapTiles[i][j].GetComponent<Image>().sprite = textures[selectedFloor[i, j]];
                mapTiles[i][j].GetComponent<MapTile>().tileType = (TileType)selectedFloor[i, j];
            }
        }
    }

    public void UpdateTileValue(int x, int y, int floor, TileType type)
    {
        switch(floor)
        {
            case 0:
                floor1[y, x] = (int)type;
                break;
            case 1:
                floor2[y, x] = (int)type;
                break;
            case 2:
                floor3[y, x] = (int)type;
                break;
        }

        //If we are editing the current floor, make sure we update the visuals
        if(floor == currentFloor)
        {
            mapTiles[y][x].GetComponent<Image>().sprite = textures[(int)type];
            mapTiles[y][x].GetComponent<MapTile>().tileType = type;
        }
    }
}