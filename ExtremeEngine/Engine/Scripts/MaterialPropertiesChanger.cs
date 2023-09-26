using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertiesChanger : MonoBehaviour
{

    [Header("Extreme material property changer(is very useful for animations!)")]
    public PropertyConfig Properties;

    [Serializable]
    public class PropertyConfig
    {
        public string PropertyName;
        public MaterialPropertyType PropertyType;
        public Texture Texture;
        public Vector4 Vector;
        public Matrix4x4 Matrix;
        public float Float;
        public int Int;

        public PropertyConfig(string name, MaterialPropertyType type, Texture texture, Vector4 vector, Matrix4x4 matrix, float float32, int int32)
        {
            PropertyName = name;
            PropertyType = type;
            Texture = texture;
            Vector = vector;
            Matrix = matrix;
            Float = float32;
            Int = int32;
        }
    }
}
