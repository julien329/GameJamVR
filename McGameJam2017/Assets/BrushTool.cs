using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : MonoBehaviour {

    public TileType currentBrush;

	// Use this for initialization
	void Start () {
        currentBrush = TileType.HOLE;
	}

    public void SetBrush(int type)
    {
        currentBrush = (TileType)type;
    }
}
