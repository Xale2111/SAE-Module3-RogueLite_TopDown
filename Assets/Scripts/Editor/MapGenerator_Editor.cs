using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGenerator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;
        
        DrawDefaultInspector();
        
        EditorGUILayout.Space(20);
        EditorGUILayout.Separator();

        GUI.backgroundColor = new Color(0.2f,0.8f,0.2f);
        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateMap();
        }
        
        EditorGUILayout.Space(10);
        EditorGUILayout.Separator();
       GUI.backgroundColor = new Color(0.0f,0.8f,0.0f);
        if (GUILayout.Button("Draw next corridor"))
        {
            mapGenerator.DrawNextCorridor();
        }
       
        EditorGUILayout.Space(10);
        EditorGUILayout.Separator();
        GUI.backgroundColor = new Color(0.8f,0.2f,0.2f);
        if (GUILayout.Button("Clear All"))
        {
            mapGenerator.ClearAll();
        }
    }
}
