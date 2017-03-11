using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapAreaScript : MonoBehaviour {
    public GameObject basePanel;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < 16*32; ++i)
        {
            var newPanel = Instantiate(basePanel);
            newPanel.transform.parent = this.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
