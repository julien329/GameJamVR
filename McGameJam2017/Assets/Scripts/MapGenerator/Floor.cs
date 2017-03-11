using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {

    [SerializeField]
    static int dim1 = 32; // ligne
    [SerializeField]
    static int dim2 = 16; // colonne

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

    public static int Dim1
    {
        get
        {
            return dim1;

        }
    }

    public static int Dim2
    {
        get
        {
            return dim2;

        }
    }
}
