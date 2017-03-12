using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Map {

    [SerializeField]
    bool debug = false;

    [SerializeField]
    int nbrLevelVR = 3;

    [SerializeField]
    int nbrLevelR = 1;

    List<Floor> floorVR;/* = new Floor[3] { new Floor(), new Floor(), new Floor() };*/ // Taille varie voir createMapWithCharArray()

    Floor floorR = new Floor();

    // Accesseurs et modificateurs
    public List<Floor> FloorVR
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

    public Floor FloorR
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

        string folderPath = @Application.dataPath + @"/LevelDesign/";
        //Debug.Log(Application.dataPath); //En mode editeur:  C:/Users/rache/Documents/REPO/McGameJam/McGameJam2017/McGameJam2017/Assets //En mode build : C:/Users/rache/Documents/REPO/McGameJam/build/test_Data

        if (findOrCreateDirectory(folderPath))
        {
            // Lecture du fichier s'il existe.
            string levelDesign = "";
            string filePath = folderPath + fileName + ".txt";
            if (readFile(filePath, out levelDesign))
            {
                // Remplir les floors!
                //char[] splitChar = { ' ' };

                string[] allTiles = levelDesign.Split(new string[] { "\n", "\r\n", "," }, StringSplitOptions.RemoveEmptyEntries);

                char[] allTilesChar = new char[allTiles.Length];

                for (int i = 0; i < allTiles.Length; i++)
                {
                    int nombre = -1;
                    bool parseReussi = Int32.TryParse(allTiles[i], out nombre);
                    if (parseReussi)
                    {
                        allTilesChar[i] = (char)nombre;
                    }
                    else
                    {
                        reussi = false;
                        return reussi;
                    }
                }

                reussi = createMapWithCharArray(allTilesChar);

                return reussi;
            }
            else
            {
                reussi = false;
            }
        }
        else
        {
            reussi = false;
        }

        return reussi;
    }

    /// <summary>
    /// But: Écrit un fichier texte à partir de la map.
    /// </summary>
    /// <param name="map"></param>
    public bool writeMapFile(string fileName)
    {
        bool reussi = true;
        // Verification: floorVR ne doit pas etre vide
        if (floorVR.Count != 0)
        {
            string folderPath = @Application.dataPath + @"/LevelDesign/";
            if (findOrCreateDirectory(folderPath))
            {

                /*for (int i = 0; i < floorVR.Count; i++) //TODO ajout verification que les floors soit bien remplies
                {

                }*/

                string filePath = folderPath + fileName + ".txt";

                // Code issu du site microsoft : https://msdn.microsoft.com/en-us/library/cc148994.aspx
                // Delete a file by using File class static method...
                if (System.IO.File.Exists(filePath))
                {
                    // Use a try block to catch IOExceptions, to
                    // handle the case of the file already being
                    // opened by another process.
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }
                // Fin de code issu du site microsoft : 


                string levelDesign = "";

                int dim1 = Floor.Dim1;
                int dim2 = Floor.Dim2;
                int nbrLevel = floorVR.Count;

                for (int i = 0; i < nbrLevel; i++)
                {
                    for (int j = 0; j < dim1; j++)
                    {
                        for (int k = 0; k < dim2; k++)
                        {
                            levelDesign = levelDesign + (int)floorVR[i].Tiles[j][k] + ",";
                        }

                        /*if (j != (dim1 - 1))
                        {*/
                        levelDesign = levelDesign + "\r\n";
                        /*}*/
                    }
                }

                System.IO.File.WriteAllText(filePath, levelDesign);
            }
            else
            {
                reussi = false;
            }

        }
        else
        {
            reussi = false;
        }

        return reussi;

    }

    bool findOrCreateDirectory(string path)
    {
        bool reussi = true;

        // Code issue du site de microsoft : https://msdn.microsoft.com/en-us/library/54a0at6s(v=vs.110).aspx
        try
        {
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                if (debug)
                {
                    Debug.Log("That path exists already.");
                }
                return reussi;
            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);

            if (debug)
            {
                Debug.Log("The directory " + di.FullName + " was created successfully at " + Directory.GetCreationTime(path));
            }

        }
        catch (Exception e)
        {
            if (debug)
            {
                Debug.Log("The process failed: " + e.ToString());
            }
            reussi = false;
        }
        finally { }
        // Fin du code issue du site de microsoft
        return reussi;
    }

    /// <summary>
    /// Lit tout le fichier et le met dans un string. Si la lecture echoue, le string sera vide.
    /// </summary>
    /// <param name="filePath">chemin vers le fichier avec extention.</param>
    /// <param name="text">texte issu du fichier</param>
    /// <returns>bool reussi</returns>
    bool readFile(string filePath, out string text)
    {
        text = "";
        bool reussi = true;
        // Le dossier existe, on peut lire le fichier
        try
        {
            text = System.IO.File.ReadAllText(filePath);//@"C:\Users\Public\TestFolder\WriteText.txt");
        }
        catch (Exception e)
        {
            if (debug)
            {
                Debug.Log("The process failed: " + e.ToString());
            }
            reussi = false;
        }
        finally { }
        return reussi;
    }

    bool createMapWithCharArray(char[] allFloors/*, char[] levelR*/)
    {
        bool reussi = true;

        int dim1 = Floor.Dim1;
        int dim2 = Floor.Dim2;

        if (allFloors.Length != (dim1 * dim2 * (nbrLevelVR + nbrLevelR)))
        {
            Debug.Log("ERREUR, le fichier de la map doit contenir tous les etages.");
            return false;
        }

        //int nbrLevel = allFloors.Length / (dim1 * dim2);

        // Reinitialisation de la liste.
        floorVR = new List<Floor>();

        //floorVR = new Floor[nbrLevel] { new Floor(), new Floor(), new Floor() };
        for (int i = 0; i < nbrLevelVR; i++)
        {
            floorVR.Add(new Floor());
        }

        // Remplir les floors VR
        for (int i = 0; i < nbrLevelVR; i++)
        {
            for (int j = 0; j < dim1; j++)
            {
                for (int k = 0; k < dim2; k++)
                {
                    floorVR[i].Tiles[j][k] = allFloors[i * (dim1 * dim2) + j * dim2 + k];
                }
            }
        }

        // Reinitialiser floorR
        floorR = new Floor();

        // Remplir le floor R
        for (int j = 0; j < dim1; j++)
        {
            for (int k = 0; k < dim2; k++)
            {
                floorR.Tiles[j][k] = allFloors[nbrLevelVR * (dim1 * dim2) + j * dim2 + k];
            }
        }

        return reussi;

    }

    #region fonctions pour l'editeur

    // Pour l'instant, il y a une seule map personnalisable: mapCustom.

    public void saveMapFromEditor(int[,] floor1, int[,] floor2, int[,] floor3)
    {
        // La structure des maps de l'editeur sont la suivante: 16 x 32, 16 ranges et 32 colonnes, il faut donc adapter au Map qui sont fait de floor 32 ranges par 16 colonnes.
        int[][,] editorFloors = new int[][,] { floor1, floor2, floor3 };
        // Conversion des arrays
        int dim1 = Floor.Dim1;
        int dim2 = Floor.Dim2;
        int nbrLevel = 3; // On fait 3 etages mais s'il y en a des vides.

        // Reinitialisation de la liste.
        floorVR = new List<Floor>();
        for (int i = 0; i < nbrLevel; i++)
        {
            floorVR.Add(new Floor());
        }

        // On parcours les Floor de la liste floorVR pour remplir avec les infos de l'editeur.
        for (int i = 0; i < nbrLevel; i++)
        {
            for (int j = 0; j < dim1; j++)
            {
                for (int k = 0; k < dim2; k++)
                {
                    floorVR[i].Tiles[j][k] = (char)editorFloors[i][k, dim1 - (j + 1)];
                }
            }
        }


        // write
        writeMapFile("mapCustom");
    }

    public int[][,] loadMapToEditor()
    {
        // charger la map custom // Remplir les floorVR
        readMapFile("mapCustom");

        // Conversion des arrays
        int dim1 = Floor.Dim1;
        int dim2 = Floor.Dim2;
        int nbrLevel = 3; // On fait 3 etages mais s'il y en a des vides.

        int[][,] floors = new int[][,] { new int[dim2, dim1], new int[dim2, dim1], new int[dim2, dim1] };

        // On parcours les Floor de la liste floorVR pour remplir avec les infos de l'editeur.
        for (int i = 0; i < nbrLevel; i++)
        {
            for (int j = 0; j < dim1; j++)
            {
                for (int k = 0; k < dim2; k++)
                {
                    //floorVR[i].Tiles[j][k] = (char)floors[i][k, dim1 - (j + 1)];
                    floors[i][k, dim1 - (j + 1)] = floorVR[i].Tiles[j][k];
                }
            }
        }

        //les etages vide (non existant) sont remplit de HOLE
        return floors;
    }

    #endregion
}
