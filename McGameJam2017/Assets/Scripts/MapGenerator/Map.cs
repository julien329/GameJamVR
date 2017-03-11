﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Map {

    [SerializeField]
    bool debug = false;

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
                char[] splitChar = { ' ' };

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

                createMapWithCharArray(allTilesChar);

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
                Debug.Log("The directory was created successfully at " + Directory.GetCreationTime(path));
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

    void createMapWithCharArray(char[] levelVR/*, char[] levelR*/)
    {
        int dim1 = Floor.Dim1;
        int dim2 = Floor.Dim2;
        int nbrLevel = levelVR.Length / (dim1 * dim2);

        // Reinitialisation de la liste.
        floorVR = new List<Floor>();

        //floorVR = new Floor[nbrLevel] { new Floor(), new Floor(), new Floor() };
        for (int i = 0; i < nbrLevel; i++)
        {
            floorVR.Add( new Floor());
        }

        for (int i = 0; i < nbrLevel; i++)
        {
            for (int j = 0; j < dim1; j++)
            {
                for (int k = 0; k < dim2; k++)
                {
                    floorVR[i].Tiles[j][k] = levelVR[i*(dim1*dim2) + j * dim2 + k];
                }
            }
        }
        Debug.Log("test");

    }

}
