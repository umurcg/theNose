
#if (UNITY_EDITOR) 

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

//This script apply importer settings for buildings. folder is main path of buildings and fileExtension is extension of building files.

public class BuildingImporter : AssetPostprocessor
{

    static string folder = "Assets/Models(blend)/Yapılar/";
    static string fileExtension = ".blend";

    // Use this for initialization
    void Start () {
	
	}

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

        //Loop in every moved asset and if directory is under of folder then goes into if condition
        for (int i = 0; i < movedAssets.Length; i++)
        {

            if (movedAssets[i].Length > folder.Length && System.String.Compare(movedAssets[i].Substring(0, folder.Length), folder) == 0)
            {

                //Check is file has extension

                if (movedAssets[i].Length > fileExtension.Length && movedAssets[i].Substring(movedAssets[i].Length - fileExtension.Length, fileExtension.Length) == fileExtension)
                {

                    Debug.Log("File is " + movedAssets[i]);

                    ModelImporter importer = null;

                    try
                    {
                        //Get modelImporter
                        importer = (ModelImporter)AssetImporter.GetAtPath(movedAssets[i]);
                    }
                    catch (System.InvalidCastException e)
                    {
                        Debug.Log(movedAssets[i] + " doesnt have model importer");
                        return;
                    }

                    Debug.Log("compressed");

                    if (!importer) Debug.Log("Importer is null");

                    importer.meshCompression = ModelImporterMeshCompression.High;
                    importer.importNormals = ModelImporterNormals.None;

                }






            }
        }
    }

   



    //    void OnPostprocessModel(GameObject obj)
    //{

    //    Debug.Log("postprocessing");

    //}

    // Update is called once per frame
    void Update () {
	
	}
}



#endif