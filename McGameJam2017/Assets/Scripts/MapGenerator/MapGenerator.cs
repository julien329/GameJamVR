using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [SerializeField]
    GameObject[]  tileVR = new GameObject[(int)TileType.TOTAL];

    [SerializeField]
    GameObject[] tileR = new GameObject[(int)TileReal.TOTAL];

    [SerializeField]
    GameObject realMapFloor;

    public void generateMap(Map map)
    {
#if UNITY_ANDROID
        generateMapVR(map);
#endif

#if UNITY_STANDALONE
        generateMapR(map);
#endif
    }

    void Start()
    {
        //test
        Map map = new Map();
        map.readMapFile("testRead");
        map.writeMapFile("testWrite");

        // Charger la map par defaut
        // Pour l'instant la seule map c<est-a-dire test.
        /*Map map = new Map();
        map.readMapFile("test");
        //map.readMapFile("mapCustom");
        generateMap(map);*/

    }

    void generateMapR(Map map)
    {
        //TODO
        GameObject parentMap = new GameObject("Map");
        parentMap.transform.position = new Vector3(0, 0, 0);

        //Instantiate la dalle du site de construction (16 par 32)
        GameObject realMapFloorClone = Instantiate(realMapFloor, realMapFloor.transform.position, new Quaternion(0, 0, 0, 0));
        realMapFloorClone.transform.parent = parentMap.transform;

        int dim1 = Floor.Dim1;
        int dim2 = Floor.Dim2;

        for (int j = 0; j < dim1; j++)
        {
            for (int k = 0; k < dim2; k++)
            {
                int tileType = map.FloorR.Tiles[j][k];
                // 0 = NORMAL, pour laquelle on ne fait rien.
                if (tileType != 0)
                {
                    GameObject clone = Instantiate(tileR[tileType], new Vector3(j * 2, 0, k * 2), new Quaternion(0, 0, 0, 0));
                    clone.transform.parent = parentMap.transform;
                }
            }
        }
    }

    void generateMapVR(Map map)
    {
        GameObject parentMap = new GameObject("Map");
        parentMap.transform.position = new Vector3(0, 0, 0);
        
        //Instantiate(tile[(int)(TileType.NORMAL)], new Vector3(0,0,0), new Quaternion(0, 0, 0, 0));
        // Parcourir les floorsVR pour generer les tiles
        int dim1 = Floor.Dim1;
        int dim2 = Floor.Dim2;
        int nbrLevel = map.FloorVR.Count;

        for (int i = 0; i < nbrLevel; i++)
        {
            for (int j = 0; j < dim1; j++)
            {
                for (int k = 0; k < dim2; k++)
                {
                    //levelDesign = levelDesign + (int)floorVR[i].Tiles[j][k] + ",";
                    int tileType = map.FloorVR[i].Tiles[j][k];
                    // 0 = HOLE, pour lequel on ne fait rien.
                    if (tileType != 0)
                    {
                        GameObject clone = Instantiate(tileVR[tileType], new Vector3(j*2, i*4, k*2), new Quaternion(0, 0, 0, 0));
                        clone.transform.parent = parentMap.transform;
                    }
                }
            }
        }
    }

    

}
