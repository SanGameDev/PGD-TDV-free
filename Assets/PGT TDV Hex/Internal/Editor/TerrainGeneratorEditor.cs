using UnityEditor;
using UnityEngine;

namespace PGT_TDV_Hex
{
    [CustomEditor(typeof(TerrainGenerator))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TerrainGenerator terrainGenerator = (TerrainGenerator)target;

            GUILayout.Space(25);

            //make this two buttons in the same line

            GUILayout.Label("Terrain Generator Buttons", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Generate Terrain"))
            {
                //terrainGenerator.ClearTerrain();
                //terrainGenerator.GenerateTerrain();
            }

            if(GUILayout.Button("Clear Terrain"))
            {
                //terrainGenerator.ClearTerrain();
            }

            GUILayout.EndHorizontal();
        }
    }
}

