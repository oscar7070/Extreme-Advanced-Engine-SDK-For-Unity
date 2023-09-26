#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class GUIExtension
{

    public static void DrawGrid(Vector2 min, Vector2 max, float spacing, Color gridColor, float thickness = 0)
    {
        Vector2 start = new(Mathf.Ceil(min.x / spacing) * spacing, Mathf.Ceil(min.y / spacing) * spacing);
        Vector2 end = new(Mathf.Floor(max.x / spacing) * spacing, Mathf.Floor(max.y / spacing) * spacing);

        int widthLines = Mathf.CeilToInt((end.x - start.x) / spacing);
        int heightLines = Mathf.CeilToInt((end.y - start.y) / spacing);

        Handles.BeginGUI();
        Handles.color = gridColor;
        for (int x = 0; x <= widthLines; x++)
        {
            Handles.DrawLine(
                new Vector3(start.x + x * spacing, min.y),
                new Vector3(start.x + x * spacing, max.y),
                thickness
            );
        }
        for (int y = 0; y <= heightLines; y++)
        {
            Handles.DrawLine(
                new Vector3(min.x, start.y + y * spacing),
                new Vector3(max.x, start.y + y * spacing),
                thickness
            );
        }
        Handles.EndGUI();
    }
}
#endif