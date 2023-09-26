using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMeshCreator
{

    public static Mesh GeneratePlane(Vector2Int resolution, Vector2 size)
    {
        Mesh mesh = new() { name = "Plane" };
        var vertices = new Vector3[(resolution.x + 1) * (resolution.y + 1)];
        var devideByToGetSize = size / resolution;
        for (int i = 0, y = 0; y <= resolution.y; y++)
        {
            for (int x = 0; x <= resolution.x; x++, i++)
            {
                vertices[i] = new Vector3(x, y) * devideByToGetSize;
            }
        }
        mesh.SetVertices(vertices);
        //Array.Reverse(mesh.vertices);

        var triangles = new int[resolution.x * resolution.y * 6];
        for (int ti = 0, vi = 0, y = 0; y < resolution.y; y++, vi++)
        {
            for (int x = 0; x < resolution.x; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + resolution.x + 1;
                triangles[ti + 5] = vi + resolution.x + 2;
            }
        }
        mesh.triangles = triangles;
        return mesh;
    }
}
