using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    [SerializeField]
    static int dim1 = 16;
    [SerializeField]
    static int dim2 = 32;

    char[,] tiles = new char[dim1,dim2];
	
	public char[,] Tiles
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
