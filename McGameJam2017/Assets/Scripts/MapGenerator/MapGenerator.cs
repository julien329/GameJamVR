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

    [SerializeField]
    static string nameMapDefault = "defaultMap";

    [SerializeField]
    static string nameMapCustom = "customMap";

    [SerializeField]
    string nameParentGameObject = "Map";

    GameObject parentMap = null;

    [SerializeField]
    GameObject currentTileStart = null;

    [SerializeField]
    GameObject currentTileEnd = null;

    void Start()//Pour tester
    {
        //test
        /*Map map = new Map();
        map.readMapFile("testRead");
        map.writeMapFile("testWrite");*/

        // Charger la map par defaut
        // Pour l'instant la seule map c<est-a-dire test.
        /*Map map = new Map();
        map.readMapFile("test");
        //map.readMapFile("mapCustom");
        generateMap(map);*/

        //generateMap(nameMapCustom);

    }

    public void generateDefaultMap()
    {
        generateMap(nameMapDefault);
    }

    public void generateCustomMap()
    {
        generateMap(nameMapDefault);
    }

    public void generateMap(string mapName)
    {
        // S'il y a une map dans la scene la delete avant de charger la nouvelle map.
        if (parentMap)
        {
            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        Map map = new Map();
        if (map.readMapFile(mapName))
        {
#if UNITY_ANDROID
        generateMapVR(map);
#endif

#if UNITY_STANDALONE
            generateMapR(map);
#endif
        }
    }

    void generateMapR(Map map)
    {
        //TODO
        parentMap = new GameObject(nameParentGameObject);
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
        GameObject parentMap = new GameObject(nameParentGameObject);
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
                        GameObject clone = Instantiate(tileVR[tileType], new Vector3(j * 2, i * 4, k * 2), new Quaternion(0, 0, 0, 0));
                        clone.transform.parent = parentMap.transform;
                        if (tileType == (int)TileType.START)
                        {
                            currentTileStart = clone;
                        }
                        else if (tileType == (int)TileType.END)
                        {
                            currentTileEnd = clone;
                        }
                    }
                }
            }
        }
    }

    // Accesseurs et modificateurs
    public static string NameMapCustom
    {
        get
        {
            return nameMapCustom;
        }
    }

    public static string NameMapDefault
    {
        get
        {
            return nameMapDefault;
        }
    }
}
