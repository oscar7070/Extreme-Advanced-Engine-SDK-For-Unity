#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialPropertiesChanger)), CanEditMultipleObjects()]
public class MaterialPropertiesChangerGUI : Editor
{
    public override void OnInspectorGUI()
    {
        MaterialPropertiesChanger t = (MaterialPropertiesChanger)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("GetAllMaterialProperties"))
        {
            Material sharedMaterial = t.gameObject.GetComponent<Renderer>().sharedMaterial;
            Shader shader = sharedMaterial.shader;
            /*t.Properties.Clear();
            {
                var floats = sharedMaterial.GetPropertyNames(MaterialPropertyType.Float);
                for (int i = 0; i < floats.Length; i++)
                {
                    t.Properties.Add(new(floats[i], MaterialPropertyType.Float, null, Vector4.zero, Matrix4x4.zero, sharedMaterial.GetFloat(floats[i]), 0));
                }
            }

            {
                var ints = sharedMaterial.GetPropertyNames(MaterialPropertyType.Int);
                for (int i = 0; i < ints.Length; i++)
                {
                    t.Properties.Add(new(ints[i], MaterialPropertyType.Int, null, Vector4.zero, Matrix4x4.zero, 0, sharedMaterial.GetInt(ints[i])));
                }
            }

            {
                var vectors = sharedMaterial.GetPropertyNames(MaterialPropertyType.Vector);
                for (int i = 0; i < vectors.Length; i++)
                {
                    t.Properties.Add(new(vectors[i], MaterialPropertyType.Vector, null, sharedMaterial.GetVector(vectors[i]), Matrix4x4.zero, 0, 0));
                }
            }

            {
                var matrixes = sharedMaterial.GetPropertyNames(MaterialPropertyType.Matrix);
                for (int i = 0; i < matrixes.Length; i++)
                {
                    t.Properties.Add(new(matrixes[i], MaterialPropertyType.Matrix, null, Vector4.zero, sharedMaterial.GetMatrix(matrixes[i]), 0, 0));
                }
            }

            {
                var textures = sharedMaterial.GetPropertyNames(MaterialPropertyType.Texture);
                for (int i = 0; i < textures.Length; i++)
                {
                    t.Properties.Add(new(textures[i], MaterialPropertyType.Texture, sharedMaterial.GetTexture(textures[i]), Vector4.zero, Matrix4x4.zero, 0, 0));
                }
            }*/
        }
    }
}
#endif