using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Tilemap))] //Custom inspector test shenanigans for the tile map
public class TileMapInspector : Editor {

 //   float testFloat = 0.0f;       //test float for custom inspector slider

    public override void OnInspectorGUI() //GUI for the inspector stuff
    {
        DrawDefaultInspector();     //Can either take the base inspector script or draw default inspector
        //EditorGUILayout.BeginVertical();                              //allocates vertical space for slider    
        //testFloat = EditorGUILayout.Slider(testFloat, 0f, 10f);       //adds a slider that changes test float from 0 - 100
        //EditorGUILayout.EndVertical();                                //ends that space
        if(GUILayout.Button("Generate TileMap"))        //button to generate tilemap
        {
            Tilemap _tileMapScript = (Tilemap)target;   //Button needs a target, so you cast the target as the script :v cheat hehe
            _tileMapScript.CreateMesh();        //then you call the function!
        }
    }
}
