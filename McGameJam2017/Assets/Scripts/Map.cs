using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    Floor[] floorVR = new Floor[3];

    Floor floorR = new Floor();


    // Accesseurs et modificateurs

    public Floor[] FloorVR
    {
    	get
    	{
    		return floorVR;
    	}
        set
    	{
    		floorVR = value;
    	}
    }
	
	public Floor LevelR
	{
		get
		{
			return floorR;
		}
		set
		{
            floorR = value;
		}
		
	}

    /// <summary>
    /// But: Remplir les floors de la map à partir d'un fichier texte.    
    /// </summary>
    /// <returns>bool reussi</returns>
    public bool readMapFile(string fileName)
    {
        bool reussi = true;

        return reussi;
    }
    
    /// <summary>
    /// But: Écrit un fichier texte à partir de la map.
    /// </summary>
    /// <param name="map"></param>
    public void writeMapFile(string fileName)
    {

        
    }
	
}
