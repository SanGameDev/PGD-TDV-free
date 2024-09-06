using UnityEditor;
using UnityEngine;

public class CustomRuleTileWindow : EditorWindow
{
    [MenuItem("Window/Custom Rule Tile")]
    public static void ShowWindow()
    {
        GetWindow<CustomRuleTileWindow>("Custom Rule Tile");
    }

    private void OnGUI() 
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        int gridSize = 20;
        int gridWidth = Mathf.CeilToInt(position.width / gridSize);
        int gridHeight = Mathf.CeilToInt(position.height / gridSize);

        Handles.color = Color.gray;
        for (int i = 0; i < gridWidth; i++)
        {
            Handles.DrawLine(new Vector3(i * gridSize, 0, 0), new Vector3(i * gridSize, position.height, 0));
        }

        for (int j = 0; j < gridHeight; j++)
        {
            Handles.DrawLine(new Vector3(0, j * gridSize, 0), new Vector3(position.width, j * gridSize, 0));
        }
    }
}
