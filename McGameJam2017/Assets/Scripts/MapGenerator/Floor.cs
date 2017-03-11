using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {

    [SerializeField]
    static int dim1 = 16;
    [SerializeField]
    static int dim2 = 32;

    char[][] tiles;

    public Floor()
    {
        tiles = new char[dim1][];
        for (int i = 0; i < dim1; i++)
        {
            tiles[i] = new char[dim2];
        }
    } 

    public char[][] Tiles
    {
        get
        {
            return tiles;

        }
        set
        {
            tiles = value;
        }
    }
}
