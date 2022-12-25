using System.Linq;
using UnityEngine;

public struct Bounds2D
{

    public readonly Vector2 Center;
    public readonly Vector2 Size;
    private readonly Vector2 Extents;
    public readonly Vector2 Min;
    public readonly Vector2 Max;

    public Bounds2D(Vector2 center, Vector2 size)
    {
        Center = center;
        Size = size;
        Extents = Size * .5f;
        Min = Center - Extents;
        Max = Center + Extents;
    }

    public bool Intersects(Bounds2D bounds)
    {
        return Min.x <= bounds.Max.x && Max.x >= bounds.Min.x && Min.y <= bounds.Max.y && Max.y >= bounds.Min.y;
    }

    public Bounds2D CalculateBound(Matrix4x4 center, Vector2[] points)
    {
        if (points.Length != 0)
        {
            Vector2[] tranPoints = new Vector2[points.Length];
            for (int i = 0; i < tranPoints.Length; i++)
            {
                tranPoints[i] = center.MultiplyPoint3x4(points[i]);
            }
            var minX = tranPoints.Min(p => p.x);
            var minY = tranPoints.Min(p => p.y);
            var maxX = tranPoints.Max(p => p.x);
            var maxY = tranPoints.Max(p => p.y);
            return new Bounds2D(new((minX + maxX) * .5f, (minY + maxY) * .5f), new(maxX - minX, maxY - minY));
        }
        else
        {
            return new Bounds2D(center.GetPosition(), Vector2.zero);
        }
    }

    public bool SeenByTheCamera(Camera camera)
    {
        return Intersects(GetCameraBounds(camera));
    }

    public static Bounds2D GetCameraBounds(Camera camera)
    {
        float verticalHeight = camera.orthographicSize * 2;
        float verticalWidth = verticalHeight * camera.aspect;
        return new Bounds2D(camera.transform.position, new(verticalWidth, verticalHeight));
    }
}
