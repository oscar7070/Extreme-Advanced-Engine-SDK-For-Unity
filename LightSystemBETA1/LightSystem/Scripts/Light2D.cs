using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects]
#endif

[ExecuteInEditMode, ImageEffectAllowedInSceneView, AddComponentMenu("Light2D/Light2D")]
public class Light2D : MonoBehaviour
{

    #region Inspector
    [Header("Main")]
    [ColorUsage(false)] public Color Color = Color.white;
    public bool UseColorTemperture = false;
    [Range(1000, 20000)] public float ColorTemperature = 6570;
    [Min(0)] public float Intensity = 1;
    [Min(0)] public float Size = 10;
    [Range(0, 1)] public float Apearness = 0;
    [Range(0, 1)] public float Fade = 0;
    [Range(0, 1)] public float FallOff = 0;
    [Header("Other")]
    public bool IsVolumetric = false;
    public Texture2D CookieTexture;
    public enum LightModeEnum
    {
        RealTime,
        BakedOnStart,
        NoShadows
    }
    public LightModeEnum LightMode = LightModeEnum.RealTime;
    #endregion

    private Material FinalImageWithLightMaterial;
    [HideInInspector] public Light2DSettings Light2DSettings;
    private float HalfSize => Size * .5f;
    private float SquareHalfEdgeSize => Size * .70710f;
    private Bounds2D LightBounds = new();
    private Light2DSceneData LightSceneData;

#if UNITY_EDITOR
    private bool IsDrawn = false;

    private readonly List<ShadowPoint> SPoints = new();
#endif

    #region Set on render image callback
    private void OnEnable()
    {
        Light2DCameraRenderer.OnRenderImageCallbackForLight += OnCameraRenderImage;
    }

    private void OnDisable()
    {
        Light2DCameraRenderer.OnRenderImageCallbackForLight -= OnCameraRenderImage;
    }

    private void OnDestroy()
    {
        Light2DCameraRenderer.OnRenderImageCallbackForLight -= OnCameraRenderImage;
    }
    #endregion

    private void Awake()
    {
        FinalImageWithLightMaterial = new Material(Light2DSettings.FinalImageWithLightMat.shader);
        FinalImageWithLightMaterial.CopyPropertiesFromMaterial(Light2DSettings.FinalImageWithLightMat);
        LightSceneData = FindObjectOfType<Light2DSceneData>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (IsDrawn)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            for (int i = 0; i < SPoints.Count; i++)
            {
                Handles.color = InverseColor(Color.Lerp(Color.green, Color.white, (float)i / SPoints.Count)) - new Color(0, 0, 0, .25f);
                Handles.DrawSolidDisc(SPoints[i].ClosePoint + position, Vector3.forward, .125f);
                Handles.DrawLine(position, SPoints[i].ClosePoint + position);
            }
            Gizmos.color = Color.yellow - new Color(0, 0, 0, 0);
            GizmoDrawWireArc(position, rotation * Vector2.right, 360 - (Apearness * 360), HalfSize);
            Gizmos.color = Color.yellow - new Color(0, 0, 0, .25f);
            GizmoDrawWireArc(position, rotation * Vector2.right, 360 - (Apearness * 360), HalfSize * FallOff);
            GizmoDrawWireArc(position, rotation * Vector2.right, 360 - (Apearness * 360) - (Fade * (1 - Apearness) * 360), HalfSize);
            Gizmos.DrawWireCube(LightBounds.Center, LightBounds.Size);
        }
    }

    private static void GizmoDrawWireArc(Vector2 position, Vector2 direction, float anglesRange, float radius, float maxSteps = 50)
    {
        var posA = position;
        var stepAngles = anglesRange / maxSteps;
        var angle = GetAngle(-direction) - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = position;
            posB += new Vector2(radius * MathF.Cos(rad), radius * MathF.Sin(rad));
            Gizmos.DrawLine(posA, posB);
            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, position);
    }

    private static Color InverseColor(Color color)
    {
        return new Color(1 - color.r, 1 - color.g, 1 - color.b);
    }
#endif

    private void OnCameraRenderImage(RenderTexture source, Light2DCameraRenderer light2DCameraRenderer, Camera camera, List<Light2DCollider> Colliders, int maxDrawenLights)
    {
        if (light2DCameraRenderer.ActiveLightsCount < maxDrawenLights)
        {
            Vector2 position = transform.position;
            LightBounds = new(position, new(Size, Size));
            if (LightBounds.SeenByTheCamera(camera))
            {
                light2DCameraRenderer.ActiveLightsCount += 1;
                UpdateMesh(position, source, light2DCameraRenderer, Colliders);
#if UNITY_EDITOR
                IsDrawn = true;
                return;
#endif
            }
        }
#if UNITY_EDITOR
        IsDrawn = false;
#endif
    }

    private MaterialPropertyBlock GenerateMaterialPropertyBlock()
    {
        MaterialPropertyBlock PropertyBlock = new();
        if (UseColorTemperture)
        {
            PropertyBlock.SetColor("_Color", Mathf.CorrelatedColorTemperatureToRGB(ColorTemperature) * Color);
        }
        else
        {
            PropertyBlock.SetColor("_Color", Color);
        }
        Color color = PropertyBlock.GetColor("_Color");
        PropertyBlock.SetColor("_Color", new Color(color.r, color.g, color.b, 1));
        PropertyBlock.SetFloat("_Intensity", Intensity);
        PropertyBlock.SetFloat("_Apearness", Apearness);
        PropertyBlock.SetFloat("_Fade", Fade);
        PropertyBlock.SetFloat("_FallOff", FallOff);
        PropertyBlock.SetFloat("_Rotation", transform.eulerAngles.z);
        if (CookieTexture)
        {
            PropertyBlock.SetTexture("_Cookie", CookieTexture);
        }
        return PropertyBlock;
    }

    private readonly struct Point
    {
        public readonly Vector2 Position;
        public readonly float Angle;
        public readonly bool IsHit;

        public Point(Vector2 position, float angle, bool isHit)
        {
            Position = position;
            Angle = angle;
            IsHit = isHit;
        }
    }

    private readonly struct ShadowPoint
    {
        public readonly Vector3 ClosePoint;
        public readonly Vector3 FarPoint;
        public readonly float Angle;

        public ShadowPoint(Vector3 closePoint, Vector3 farPoint, float angle)
        {
            ClosePoint = closePoint;
            FarPoint = farPoint;
            Angle = angle;
        }
    }

    private readonly struct ColliderMoreData
    {
        public readonly Light2DCollider Light2DCollider;
        public readonly Vector2[] AllPointsInWorldSpace;

        public ColliderMoreData(Light2DCollider light2DCollider, Vector2[] allPointsInWorldSpace)
        {
            Light2DCollider = light2DCollider;
            AllPointsInWorldSpace = allPointsInWorldSpace;
        }
    }

    private void UpdateMesh(Vector2 position, RenderTexture source, Light2DCameraRenderer light2DCameraRenderer, List<Light2DCollider> colliders)
    {
#if UNITY_EDITOR
        SPoints.Clear();
#endif
        ColliderMoreData[] foundColliders = new ColliderMoreData[0];
        if (LightMode != LightModeEnum.NoShadows)
        {
            foundColliders = FindCollidersInsideBounds();
        }
        List<ShadowMeshInfo> shadowRenderGroupInfo = new();
        Vector2 angleFrom = transform.rotation * Vector2.right;
        float compareWithAngle = (360 - (Apearness * 360)) / 2;
        for (int i = 0; i < foundColliders.Length; i++)
        {
            List<ShadowPoint> shadowPoints = new();
            for (int i2 = 0; i2 < foundColliders[i].Light2DCollider.Points.Length; i2++)
            {
                Vector2 realPointPos = foundColliders[i].AllPointsInWorldSpace[i2];
                Vector2 angleDirection = realPointPos - position; //Normalize(realPointPos - position);
                if (Vector2.Distance(realPointPos, position) < SquareHalfEdgeSize & (Apearness == 0 || Angle(angleFrom, angleDirection) < compareWithAngle))
                {
                    CulcShadowPoint(shadowPoints, angleDirection, foundColliders[i], position);
                }
            }
            if (Apearness != 0)
            {
                CulcShadowPoint(shadowPoints, transform.TransformDirection(GetVectorFromAngle(compareWithAngle)), foundColliders[i], position);
                CulcShadowPoint(shadowPoints, transform.TransformDirection(GetVectorFromAngle(-compareWithAngle)), foundColliders[i], position);
            }
            if (shadowPoints.Count != 0)
            {
                shadowPoints.Sort((s1, s2) => s1.Angle.CompareTo(s2.Angle));
#if UNITY_EDITOR
                SPoints.AddRange(shadowPoints);
#endif
                colliders.Add(foundColliders[i].Light2DCollider);
                shadowRenderGroupInfo.Add(new(RenderShadowMesh(shadowPoints), foundColliders[i].Light2DCollider));
            }
        }
        Matrix4x4 matrix = new();
        matrix.SetTRS(position, Quaternion.identity, Vector3.one);
        light2DCameraRenderer.SetLightToRender(RenderLightMesh(), GenerateMaterialPropertyBlock(), FinalImageWithLightMaterial, shadowRenderGroupInfo, matrix, this);
    }

    private ColliderMoreData[] FindCollidersInsideBounds()
    {
        var light2DColliders = LightSceneData?.GetAllColliders();
        List<ColliderMoreData> ToReturn = new();
        if (light2DColliders != null)
        {
            for (int iColl = 0; iColl < light2DColliders.Count; iColl++)
            {
                if (light2DColliders[iColl].GetBounds2D().Intersects(LightBounds))
                {
                    ToReturn.Add(new ColliderMoreData(light2DColliders[iColl], light2DColliders[iColl].GetAllPointsInWorldSpace()));
                }
            }
        }
        return ToReturn.ToArray();
    }

    private static void CulcShadowPoint(List<ShadowPoint> shadowPoints, Vector2 direction, ColliderMoreData foundCollider, Vector2 position)
    {
        for (int i = 0; i < 2; i++)
        {
            float toAdd = .0001f * ((i * 2) - 1);
            Vector2 dir = new(direction.x * MathF.Cos(toAdd) - direction.y * MathF.Sin(toAdd), direction.x * MathF.Sin(toAdd) + direction.y * MathF.Cos(toAdd));
            Point point = ShadowRayCast(position, dir, foundCollider);
            if (point.IsHit)
            {
                Vector2 endShadowPoint = point.Position;
                endShadowPoint += 1000 * dir;
                //endShadowPoint += 1.25f * dir;
                shadowPoints.Add(new ShadowPoint(point.Position - position,endShadowPoint - position, point.Angle));
                return;
            }
        }
    }

    private Mesh RenderLightMesh()
    {
        Mesh mesh = new();
        #region Culculate vertices
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-HalfSize, -HalfSize, 0),
            new Vector3(HalfSize, -HalfSize, 0),
            new Vector3(-HalfSize, HalfSize, 0),
            new Vector3(HalfSize, HalfSize, 0)
        };
        mesh.SetVertices(vertices);
        #endregion

        #region Culculate triangles
        int[] tris = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        mesh.SetTriangles(tris, 0);
        #endregion

        #region Culculate UV's
        Vector2[] uVs = new Vector2[vertices.Length];
        for (int i = 0; i < uVs.Length; i++)
        {
            uVs[i] = ((Vector2)vertices[i] + new Vector2(HalfSize, HalfSize)) / Size;
        }
        mesh.SetUVs(0, uVs);
        //mesh.RecalculateBounds();
        #endregion
        return mesh;
    }

    private Mesh RenderShadowMesh(List<ShadowPoint> shadowPoints)
    {
        Mesh mesh = new();
        #region Culculate vertices
        Vector3[] vertices = new Vector3[(shadowPoints.Count * 4)];
        for (int i = 0; i < shadowPoints.Count; i++)
        {
            if (i == shadowPoints.Count - 1)
            {
                vertices[i * 4] = shadowPoints[0].ClosePoint;
                vertices[(i * 4) + 1] = shadowPoints[0].FarPoint;
                vertices[(i * 4) + 2] = shadowPoints[i].FarPoint;
                vertices[(i * 4) + 3] = shadowPoints[i].ClosePoint;
            }
            else
            {
                vertices[i * 4] = shadowPoints[i + 1].ClosePoint;
                vertices[(i * 4) + 1] = shadowPoints[i + 1].FarPoint;
                vertices[(i * 4) + 2] = shadowPoints[i].FarPoint;
                vertices[(i * 4) + 3] = shadowPoints[i].ClosePoint;
            }
        }
        mesh.SetVertices(vertices);
        #endregion
        #region Culculate Quads
        int[] quads = new int[vertices.Length];
        for (int i = 0; i < quads.Length - 3; i++)
        {
            quads[i] = i;
            quads[i + 1] = i + 1;
            quads[i + 2] = i + 2;
            quads[i + 3] = i + 3;
        }
        mesh.SetIndices(quads, MeshTopology.Quads, 0);
        #endregion
        #region Culculate UV's
        Vector2[] uVs = new Vector2[vertices.Length];
        for (int i = 0; i < uVs.Length; i++)
        {
            uVs[i] = ((Vector2)vertices[i] + new Vector2(HalfSize, HalfSize)) / Size;
        }
        mesh.SetUVs(0, uVs);
        #endregion
        //mesh.RecalculateBounds();
        return mesh;
    }

    /*private Mesh RenderShadowMesh(List<ShadowPoint> shadowPoints)
    {
        Mesh mesh = new();
        #region Culculate vertices
        Vector3[] vertices = new Vector3[(shadowPoints.Count * 4)];
        for (int i = 0; i < shadowPoints.Count; i++)
        {
            if (i == shadowPoints.Count - 1)
            {
                vertices[i * 4] = shadowPoints[0].ClosePoint;
                vertices[(i * 4) + 1] = shadowPoints[0].FarPoint;
                vertices[(i * 4) + 2] = shadowPoints[i].FarPoint;
                vertices[(i * 4) + 3] = shadowPoints[i].ClosePoint;
            }
            else
            {
                vertices[i * 4] = shadowPoints[i + 1].ClosePoint;
                vertices[(i * 4) + 1] = shadowPoints[i + 1].FarPoint;
                vertices[(i * 4) + 2] = shadowPoints[i].FarPoint;
                vertices[(i * 4) + 3] = shadowPoints[i].ClosePoint;
            }
        }
        mesh.SetVertices(vertices);
        #endregion
        #region Culculate Quads
        int[] quads = new int[vertices.Length];
        for (int i = 0; i < quads.Length - 3; i++)
        {
            quads[i] = i;
            quads[i + 1] = i + 1;
            quads[i + 2] = i + 2;
            quads[i + 3] = i + 3;
        }
        mesh.SetIndices(quads, MeshTopology.Quads, 0);
        #endregion
        #region Culculate UV's
        Vector2[] uVs = new Vector2[vertices.Length];
        for (int i = 0; i < uVs.Length; i++)
        {
            uVs[i] = ((Vector2)vertices[i] + new Vector2(HalfSize, HalfSize)) / Size;
        }
        mesh.SetUVs(0, uVs);
        #endregion
        //mesh.RecalculateBounds();
        return mesh;
    }*/

    public readonly struct ShadowMeshInfo
    {
        public readonly Mesh Mesh;
        public readonly Light2DCollider Collider;

        public ShadowMeshInfo(Mesh mesh, Light2DCollider collider)
        {
            Mesh = mesh;
            Collider = collider;
        }
    }

    private static Point ShadowRayCast(Vector2 origin, Vector2 direction, ColliderMoreData lightCollider)
    {
        List<Vector2> points = new();
        Vector2[] pointsInWorldSpace = lightCollider.AllPointsInWorldSpace;
        for (int i = 0; i < pointsInWorldSpace.Length; i++)
        {
            float x1 = pointsInWorldSpace[i].x;
            float y1 = pointsInWorldSpace[i].y;
            float x2;
            float y2;
            if (i != pointsInWorldSpace.Length - 1)
            {
                x2 = pointsInWorldSpace[i + 1].x;
                y2 = pointsInWorldSpace[i + 1].y;
            }
            else
            {
                x2 = pointsInWorldSpace[0].x;
                y2 = pointsInWorldSpace[0].y;
            }
            float thisX1 = origin.x;
            float thisX2 = origin.x + direction.x;
            float thisY1 = origin.y;
            float thisY2 = origin.y + direction.y;

            float den = (x1 - x2) * (thisY1 - thisY2) - (y1 - y2) * (thisX1 - thisX2);
            if (den == 0)
            {
                continue;
            }
            float t = ((x1 - thisX1) * (thisY1 - thisY2) - (y1 - thisY1) * (thisX1 - thisX2)) / den;
            float u = -((x1 - x2) * (y1 - thisY1) - (y1 - y2) * (x1 - thisX1)) / den;

            if (t > 0 & t < 1 & u > 0)
            {
                Vector2 pt = new()
                {
                    x = x1 + t * (x2 - x1),
                    y = y1 + t * (y2 - y1)
                };
                points.Add(pt);
            }
        }
        if (points.Count != 0)
        {
            Vector2 pointDistanceToCompare = points[0];
            if (!lightCollider.Light2DCollider.SelfShadows)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    if (Vector2.Distance(points[i], origin) > Vector2.Distance(pointDistanceToCompare, origin))
                    {
                        pointDistanceToCompare = points[i];
                    }
                }
            }
            else
            {
                for (int i = 1; i < points.Count; i++)
                {
                    if (Vector2.Distance(points[i], origin) < Vector2.Distance(pointDistanceToCompare, origin))
                    {
                        pointDistanceToCompare = points[i];
                    }
                }
            }
            return new Point(pointDistanceToCompare, GetAngle(direction), true);
        }
        else
        {
            return new Point(Vector2.zero, 0, false);
        }
    }

    /*private Point ShadowRayCastForLine(Vector2 origin, Vector2 direction, float maxSize, Light2DCollider lightCollider)
    {
        float x1 = lightCollider.transform.TransformPoint(lightCollider.Points[0]).x;
        float y1 = lightCollider.transform.TransformPoint(lightCollider.Points[0]).y;
        float x2 = lightCollider.transform.TransformPoint(lightCollider.Points[1]).x;
        float y2 = lightCollider.transform.TransformPoint(lightCollider.Points[1]).y;

        float thisX1 = origin.x;
        float thisX2 = origin.x + direction.x;
        float thisY1 = origin.y;
        float thisY2 = origin.y + direction.y;

        float den = (x1 - x2) * (thisY1 - thisY2) - (y1 - y2) * (thisX1 - thisX2);
        if (den == 0)
        {
            return IfRayNotHit(origin, direction, maxSize);
        }
        float t = ((x1 - thisX1) * (thisY1 - thisY2) - (y1 - thisY1) * (thisX1 - thisX2)) / den;
        float u = -((x1 - x2) * (y1 - thisY1) - (y1 - y2) * (x1 - thisX1)) / den;

        if (t > 0 & t < 1 & u > 0)
        {
            Vector2 pt = new()
            {
                x = x1 + t * (x2 - x1),
                y = y1 + t * (y2 - y1)
            };
            return new Point(pt, GetAngle(direction), true);
        }
        else
        {
            return IfRayNotHit(origin, direction, maxSize);
        }
    }*/

    private static float GetAngle(Vector2 direction)
    {
        return ((float)Math.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 180f;
    }

    private static Vector2 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private static float Angle(Vector2 from, Vector2 to)
    {
        float num = MathF.Sqrt((from.x * from.x + from.y * from.y) * (to.x * to.x + to.y * to.y));
        if (num < 1E-15f)
        {
            return 0f;
        }
        return MathF.Acos(Mathf.Clamp((from.x * to.x + from.y * to.y) / num, -1f, 1f)) * 57.29578f;
    }

    /*private static Vector2 Normalize(Vector2 value)
    {
        float num = (float)Math.Sqrt(value.x * value.x + value.y * value.y);
        if (num > 1E-05f)
        {
            return value / num;
        }
        return new Vector2(0f, 0f);
    }*/
}