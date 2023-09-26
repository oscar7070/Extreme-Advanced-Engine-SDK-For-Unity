using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects, ExecuteInEditMode]
#endif

[AddComponentMenu("Physics 2D/" + ExtremeEngineData.EngineName + "/Cloth2D")]
public class Cloth2D : MonoBehaviour
{

    [HideInInspector] public bool AutoQualityHideObj = false;
    private Mesh ClothMesh;
    [SerializeField, Range(1, 10)] private int Resolution = 1;
    private Vector2Int ResolutionInVector2 => new(Resolution, Resolution);
    [SerializeField] private float GravityScale = -1;
    [SerializeField, Min(0.0001f)] private float Mass = 1;
    [SerializeField] private Vector2 WeatherEffectMaxForceX = new(0, 1);
    [SerializeField] private Vector2 WeatherEffectMaxForceY = new(0, 0);
    [SerializeField] private Sprite Sprite;
    [SerializeField] private Color TextureColor = new(1, 1, 1, 1);
    [SerializeField] private Material Material;
    [SerializeField] private Vector2 TextureTiling = new(1, 1);
    [SerializeField] private Vector2 TextureOffset = new(0, 0);
    [SerializeField] private bool UseNormals = true;
    private Vector2 OldObjPos;
    public ClothGeometry Geometry = new();
    private Bounds2D Bounds;

    [Header("Advanced")]
    [SerializeField] private Mesh CustomMesh;
    //private PhysFireObjectsStorge ObjectsStorge;

    [Serializable]
    public struct ClothGeometry
    {

        [Serializable]
        public struct Vertex
        {

            public Vector2 position;
            public Vector2 Oldposition;
            public bool Frozen;
            //[Range(0, 1)] public float FrozeStrength;
            public PositionPropertiesEnum PositionProperties;
            public Transform PairTo;

            public Vertex(Vector2 pos, bool forzen = false, PositionPropertiesEnum posProperties = PositionPropertiesEnum.Nothing, Transform pairTo = null)
            {
                position = pos;
                Oldposition = position;
                Frozen = forzen;
                PositionProperties = posProperties;
                PairTo = pairTo;
            }

            public enum PositionPropertiesEnum
            {
                Nothing,
                SetToTransform,
            }
        }
        public List<Vertex> Vertices;
        public int[] Triangles;
        public Vector2[] UV;

        private readonly Vector3[] GetVerticesArray()
        {
            Vector3[] toReturn = new Vector3[Vertices.Count];
            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = Vertices[i].position;
            }
            return toReturn;
        }

        public void DeleteVertexAt(int vertex)
        {
            Vertices.RemoveAt(vertex);
            List<int> newTrias = new();
            for (int i = 0; i < Triangles.Length; i++)
            {
                newTrias.Add(Triangles[i]);
            }
            for (int i = 0; i < newTrias.Count; i++)
            {
                if (newTrias[i] == vertex)
                {
                    newTrias.RemoveAt(i);
                }
            }
            Triangles = newTrias.ToArray();
        }

        public void UpdateOldPosition()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var vertex = Vertices[i];
                vertex.Oldposition = vertex.position;
                Vertices[i] = vertex;
            }
        }

        public void ApplyGeometryToMesh(Mesh mesh)
        {
            mesh.SetVertices(GetVerticesArray());
            mesh.SetTriangles(Triangles, 0);
            mesh.uv = UV;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        public void GenerateUV()
        {
            UV = new Vector2[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                UV[i] = Vertices[i].position / 10;
            }
        }
    }

#if UNITY_EDITOR
    [MenuItem(ExtremeEngineData.EngineName + "/Physics2D/Cloth2D")]
    private static void SpawnEditor()
    {
        GameObject Obj = EditorObjSpawn.Invoke(new("Cloth2D Obj"));
        Obj.AddComponent<Cloth2D>();
    }
#endif

    private void Start()
    {
        Geometry.ApplyGeometryToMesh(CreateMeshIfNull());
        //ObjectsStorge = GameObject.FindWithTag("GameManager").GetComponent<PhysFireObjectsStorge>();
        //ObjectsStorge.AddFlagToStorge(this);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Geometry.ApplyGeometryToMesh(CreateMeshIfNull());
    }
#endif

    private Mesh CreateMeshIfNull()
    {
        ClothMesh = new Mesh { name = "Cloth2D Mesh" };
        return ClothMesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Bounds.Center, Bounds.Size);
        if (!Application.isPlaying)
        {
            //GenerateMesh(false);
        }
    }

    public void ApplyMesh(Mesh mesh)
    {
        CreateMeshIfNull();
        Geometry.Vertices.Clear();
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Geometry.Vertices.Add(new((Vector2)mesh.vertices[i]));
        }
        Geometry.Triangles = mesh.triangles;
        Geometry.UpdateOldPosition();
        Geometry.GenerateUV();
        Geometry.ApplyGeometryToMesh(ClothMesh);
        OldObjPos = transform.position;
        Geometry.UpdateOldPosition();
    }

    private void SetPositionProperties()
    {
        //if (PositionProperties == PositionPropertiesEnum.SetToTransform)
        {
            //transform.SetPositionAndRotation(CustomPositionTransform.position, CustomPositionTransform.rotation);
        }
    }

    public void AddForce(int elemantToAdd, Vector2 force)
    {
        Quaternion newRotation = new() { eulerAngles = transform.eulerAngles };
        ClothGeometry.Vertex vertex = Geometry.Vertices[elemantToAdd];
        if (!vertex.Frozen)
        {
            vertex.position += (Vector2)(Quaternion.Inverse(newRotation) * new Vector2(force.x, force.y) / Mass);
        }
        Geometry.Vertices[elemantToAdd] = vertex;
    }

    /*public void AddForceLiner(int elemantToAdd, float force, Vector2 startPoint, Vector2 endPoint)
    {
        if (!FrozenVertices[elemantToAdd])
        {
            Quaternion newRotation = new() { eulerAngles = transform.eulerAngles };
            float lineDistance = Vector2.Distance(startPoint, endPoint);
            Vector2 lineDir = (startPoint - endPoint) / lineDistance;
            float startPointDistance = Vector2.Distance(transform.position + CurrentMeshPositions[elemantToAdd], (Vector3)endPoint) / lineDistance;
            Debug.Log(startPointDistance);
            CurrentMeshPositions[elemantToAdd] += Quaternion.Inverse(newRotation) * ((Vector3)lineDir * (force / Mass));
        }
    }*/

    public void AddForceToAllPoints(Vector2 force)
    {
        Quaternion newRotation = new() { eulerAngles = transform.eulerAngles };
        for (int i = 0; i < Geometry.Vertices.Count; i++)
        {
            ClothGeometry.Vertex vertex = Geometry.Vertices[i];
            if (!vertex.Frozen)
            {
                vertex.position += (Vector2)(Quaternion.Inverse(newRotation) * new Vector2(force.x, force.y) / Mass);
            }
            Geometry.Vertices[i] = vertex;
        }
    }

    public void AddExplosionForce(int elemantToAdd, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F)
    {
        ClothGeometry.Vertex vertex = Geometry.Vertices[elemantToAdd];
        if (!vertex.Frozen)
        {
            Quaternion verticesRotation = new() { eulerAngles = transform.eulerAngles };
            Vector2 verticesAfterAddingRotation = verticesRotation * vertex.position;
            float distanceFromeGranade = Vector2.Distance((Vector3)verticesAfterAddingRotation + transform.position, explosionPosition);
            var explosionDir = verticesAfterAddingRotation + (Vector2)transform.position - explosionPosition;
            explosionDir.y += upwardsModifier;
            vertex.position += (explosionDir * (explosionForce / Mass * -((Mathf.Sqrt(Mathf.Pow(distanceFromeGranade, 1.5f) + Mathf.Pow(distanceFromeGranade, 1.5f)) / explosionRadius) - 1)));
        }
        Geometry.Vertices[elemantToAdd] = vertex;
    }

    private void Update()
    {
        Quaternion newRotation = new() { eulerAngles = transform.eulerAngles };
        Bounds = new((newRotation * new Vector3(ClothMesh.bounds.center.x, ClothMesh.bounds.center.y)) + transform.position, new Vector2(ClothMesh.bounds.size.x, ClothMesh.bounds.size.y));
        if (Camera.main != null && Bounds.SeenByTheCamera(Camera.main))
        {
            AutoQualityHideObj = false;
            Vector2[] oldVert = new Vector2[Geometry.Vertices.Count];
            for (int i = 0; i < Geometry.Vertices.Count; i++)
            {
                ClothGeometry.Vertex vertex = Geometry.Vertices[i];
                oldVert[i] = vertex.position;
                Geometry.Vertices[i] = vertex;
            }
            Geometry.ApplyGeometryToMesh(ClothMesh);
            for (int i = 0; i < Geometry.Vertices.Count; i++)
            {
                ClothGeometry.Vertex vertex = Geometry.Vertices[i];
                vertex.position = oldVert[i];
                Geometry.Vertices[i] = vertex;
            }
            Material mat = new(Material);
            if (Sprite)
                mat.mainTexture = Sprite.texture;
            if (mat.HasColor(mat.name))
                mat.color *= TextureColor;
            mat.mainTextureScale = TextureTiling * ResolutionInVector2;
            mat.mainTextureOffset = TextureOffset;
            RenderParams renderParams = new()
            {
                material = mat
            };
            Graphics.RenderMesh(renderParams, ClothMesh, 0, Matrix4x4.Translate(transform.position));
        }
        else
        {
            AutoQualityHideObj = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 objPosVelocity = ((Vector2)transform.position - OldObjPos) / Mass;
        OldObjPos = transform.position;
        Quaternion newRotation = new() { eulerAngles = transform.eulerAngles };
        for (int i = 0; i < Geometry.Vertices.Count; i++)
        {
            ClothGeometry.Vertex vertex = Geometry.Vertices[i];
            if (!vertex.Frozen)
            {
                Vector2 velocity = vertex.position - vertex.Oldposition;
                vertex.Oldposition = vertex.position;
                vertex.position += velocity;
                vertex.position -= objPosVelocity;
                vertex.position += (Vector2)(Quaternion.Inverse(newRotation) * new Vector2(Random.Range(WeatherEffectMaxForceX.x / 1000, WeatherEffectMaxForceX.y / 1000), Random.Range(WeatherEffectMaxForceY.x / 1000, WeatherEffectMaxForceY.y / 1000)));
                vertex.position += (Vector2)(Quaternion.Inverse(newRotation) * (Vector2.up * GravityScale) * Time.fixedDeltaTime);
            }
            Geometry.Vertices[i] = vertex;
        }
        //X
        for (int g = 0; g < ResolutionInVector2.y + 1; g++)
        {
            for (int iter = 0; iter < Physics2D.velocityIterations; iter++)
            {
                for (int i = (ResolutionInVector2.x + 1) * g; i < ResolutionInVector2.x + ((ResolutionInVector2.x + 1) * g); i++)
                {
                    ClothGeometry.Vertex vertex = Geometry.Vertices[i];
                    ClothGeometry.Vertex vertexSecond = Geometry.Vertices[i + 1];
                    Vector2 distance = vertex.position - new Vector2(transform.lossyScale.x / ResolutionInVector2.x, 0) - vertexSecond.position - new Vector2(transform.lossyScale.x / ResolutionInVector2.x, 0);
                    float distanceMagnitude = distance.magnitude;
                    float f = (distanceMagnitude - (1 - transform.lossyScale.x / 2)) / distanceMagnitude;

                    if (!vertex.Frozen)
                    {
                        vertex.position -= .5f * f * distance;
                        if (!vertexSecond.Frozen)
                        {
                            vertexSecond.position += .5f * f * distance;
                        }
                    }
                    else
                    {
                        vertexSecond.position += f * distance;
                    }
                    Geometry.Vertices[i] = vertex;
                    Geometry.Vertices[i + 1] = vertexSecond;
                }
            }
        }
        //Y
        for (int iter = 0; iter < Physics2D.velocityIterations; iter++)
        {
            for (int i = 0; i < Geometry.Vertices.Count - (ResolutionInVector2.x + 1); i++)
            {
                ClothGeometry.Vertex vertex = Geometry.Vertices[i];
                ClothGeometry.Vertex vertexSecond = Geometry.Vertices[i + ResolutionInVector2.x + 1];
                Vector2 distance = vertex.position - new Vector2(0, transform.lossyScale.y / ResolutionInVector2.y) - vertexSecond.position - new Vector2(0, transform.lossyScale.y / ResolutionInVector2.y);
                float distanceMagnitude = distance.magnitude;
                float f = (distanceMagnitude - (1 - transform.lossyScale.y / 2)) / distanceMagnitude;

                if (!vertex.Frozen)
                {
                    vertex.position -= .5f * f * distance;
                    if (!vertexSecond.Frozen)
                    {
                        vertexSecond.position += .5f * f * distance;
                    }
                }
                else
                {
                    vertexSecond.position += f * distance;
                }
                Geometry.Vertices[i] = vertex;
                Geometry.Vertices[i + ResolutionInVector2.x + 1] = vertexSecond;
            }
        }
        //Y
        /*for (int iter = 0; iter < Physics2D.velocityIterations; iter++)
        {
            for (int i = 0; i < Geometry.Triangles.Length - 3; i += 3)
            {
                for (int t = 0; t < 3; t++)
                {
                    if (Geometry.Triangles[i + t] != Geometry.Triangles[i + t + 1])
                    {
                        ClothGeometry.Vertex vertex = Geometry.Vertices[Geometry.Triangles[i + t]];
                        ClothGeometry.Vertex vertexSecond = Geometry.Vertices[Geometry.Triangles[i + t + 1]];
                        Vector2 distance = vertex.position - new Vector2(0, transform.lossyScale.y / ResolutionInVector2.y) - vertexSecond.position - new Vector2(0, transform.lossyScale.y / ResolutionInVector2.y);
                        float distanceMagnitude = distance.magnitude;
                        float f = (distanceMagnitude - (1 - transform.lossyScale.y / 2)) / distanceMagnitude;

                        if (!vertex.Frozen)
                        {
                            vertex.position -= .5f * f * distance;
                            if (!vertexSecond.Frozen)
                            {
                                vertexSecond.position += .5f * f * distance;
                            }
                        }
                        else
                        {
                            vertexSecond.position += f * distance;
                        }
                        Geometry.Vertices[Geometry.Triangles[i + t]] = vertex;
                        Geometry.Vertices[Geometry.Triangles[i + t + 1]] = vertexSecond;
                    }
                }
            }
        }*/
    }

    /*private void OnDestroy()
    {
        if (ObjectsStorge)
        {
            ObjectsStorge.RemoveFlagFromStorge(this);
        }
    }*/
}