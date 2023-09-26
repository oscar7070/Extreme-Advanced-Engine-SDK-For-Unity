using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CanEditMultipleObjects]
#endif

[ExecuteInEditMode, AddComponentMenu("Light2D/Light2D collider")]
public class Light2DCollider : MonoBehaviour
{

    public Vector2[] Points = new Vector2[0];
    private Vector2[] PointsInWorldSpace = new Vector2[0];
    private Mesh SpriteMesh;
    private SpriteRenderer SpriteRenderer;
    private int CachedSpriteID;
    private bool NeedToUpdatePointsInWorldSpace = false;
    public bool SelfShadows = false;
    [Range(0, 1)] public float Alpha = 1;
    private Bounds2D ColliderBounds = new();

#if UNITY_EDITOR

    [HideInInspector] public EditorParams Params;

    [Serializable]
    public class EditorParams
    {
        public int Selected = 0;
        public int SelectedPath = 0;
        public bool CopyOnChange = false;
    }
#endif
    #region Initialize
    private void OnEnable()
    {
        Light2DSceneData sceneData = FindObjectOfType<Light2DSceneData>();
        if (sceneData)
        {
            sceneData.AddCollider(this);
        }
    }

    private void OnDisable()
    {
        Light2DSceneData sceneData = FindObjectOfType<Light2DSceneData>();
        if (sceneData)
        {
            sceneData.RemoveCollider(this);
        }
    }

    private void OnDestroy()
    {
        Light2DSceneData sceneData = FindObjectOfType<Light2DSceneData>();
        if (sceneData)
        {
            sceneData.RemoveCollider(this);
        }
    }

    private Light2DSceneData TryCreateOrFind()
    {
        Light2DSceneData sceneData = FindObjectOfType<Light2DSceneData>();
        if (sceneData != null)
        {
            return sceneData;
        }
        else
        {
            GameObject newSystemData = new("Light2D scene data");
            return newSystemData.AddComponent<Light2DSceneData>();
        }
    }
    #endregion

    public struct SpriteParameters
    {
        public readonly Mesh Mesh;
        public readonly Material SharedMaterial;
        public readonly MaterialPropertyBlock MaterialPropertyBlock;
        public readonly Matrix4x4 Matrix;

        public SpriteParameters(Mesh mesh, Material sharedMaterial, MaterialPropertyBlock materialPropertyBlock, Matrix4x4 matrix)
        {
            Mesh = mesh;
            SharedMaterial = sharedMaterial;
            MaterialPropertyBlock = materialPropertyBlock;
            Matrix = matrix;
        }
    }

    private void Start()
    {
        SpriteMesh = new();
        OnPointsUpdate();
        TryCreateSpriteParamsForShadow(transform.localToWorldMatrix);
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            OnPointsUpdate();
        }
    }

    public Vector2[] GetAllPointsInWorldSpace()
    {
        if (NeedToUpdatePointsInWorldSpace)
        {
            PointsInWorldSpace = new Vector2[Points.Length];
            Matrix4x4 matrix = transform.localToWorldMatrix;
            for (int i = 0; i < PointsInWorldSpace.Length; i++)
            {
                PointsInWorldSpace[i] = matrix.MultiplyPoint3x4(Points[i]);
            }
            NeedToUpdatePointsInWorldSpace = false;
        }
        return PointsInWorldSpace;
    }

    public SpriteParameters TryGetSpriteParamsForShadow()
    {
        Matrix4x4 matrix = transform.localToWorldMatrix;
        if (SpriteRenderer && SpriteRenderer.sprite.GetInstanceID() == CachedSpriteID)
        {
            MaterialPropertyBlock materialPropertyBlock = new();
            if (SpriteRenderer.sprite.texture)
            {
                materialPropertyBlock.SetTexture("_MainTex", SpriteRenderer.sprite.texture);
            }
            materialPropertyBlock.SetColor("_Color", SpriteRenderer.color);
            return new SpriteParameters(SpriteMesh, SetMissingMaterialIfNull(new(Shader.Find("ExtremeEngine/MissingSpriteShader"))), materialPropertyBlock, matrix);
        }
        else
        {
            return TryCreateSpriteParamsForShadow(matrix);
        }
    }

    private SpriteParameters TryCreateSpriteParamsForShadow(Matrix4x4 matrix)
    {
        SpriteMesh.Clear();
        if (!SpriteRenderer)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (SpriteRenderer && SpriteRenderer.sprite)
        {
            CachedSpriteID = SpriteRenderer.sprite.GetInstanceID();
            SpriteMesh.SetVertices(Array.ConvertAll(SpriteRenderer.sprite.vertices, i => (Vector3)i));
            SpriteMesh.SetTriangles(Array.ConvertAll(SpriteRenderer.sprite.triangles, i => (int)i), 0);
            SpriteMesh.SetUVs(0, SpriteRenderer.sprite.uv);
            MaterialPropertyBlock materialPropertyBlock = new();
            if (SpriteRenderer.sprite.texture)
            {
                materialPropertyBlock.SetTexture("_MainTex", SpriteRenderer.sprite.texture);
            }
            materialPropertyBlock.SetColor("_Color", SpriteRenderer.color);
            return new SpriteParameters(SpriteMesh, SetMissingMaterialIfNull(new(Shader.Find("ExtremeEngine/MissingSpriteShader"))), materialPropertyBlock, matrix);
        }
        else
        {
            return new SpriteParameters(null, null, null, matrix);
        }
    }

    private static Material SetMissingMaterialIfNull(Material material)
    {
        if (material == null)
        {
            return new(Shader.Find("ExtremeEngine/MissingSpriteShader"));
        }
        return material;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < Points.Length; i++)
        {
            if (i == Points.Length - 1)
            {
                Gizmos.DrawLine(transform.TransformPoint(Points[i]), transform.TransformPoint(Points[0]));
            }
            else
            {
                Gizmos.DrawLine(transform.TransformPoint(Points[i]), transform.TransformPoint(Points[i + 1]));
            }
        }
        Gizmos.DrawWireCube(ColliderBounds.Center, ColliderBounds.Size);
    }

    public void CopyPointsFromPolygonCollider(PolygonCollider2D polygonCollider2D, int selectedPath)
    {
        Points = polygonCollider2D.GetPath(selectedPath);
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i] += polygonCollider2D.offset;
        }
        OnPointsUpdate();
    }

    private void OnPointsUpdate()
    {
        ColliderBounds = ColliderBounds.CalculateBound(transform.localToWorldMatrix, Points);
        NeedToUpdatePointsInWorldSpace = true;
    }

    public Bounds2D GetBounds2D()
    {
        return ColliderBounds;
    }
}
