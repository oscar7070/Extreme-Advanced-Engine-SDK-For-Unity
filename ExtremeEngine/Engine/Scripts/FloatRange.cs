using System;

[Serializable]
public struct FloatRange
{

    public float MinAngle, MaxAngle;

    public FloatRange(float minAngle = -360, float maxAngle = 360)
    {
        MinAngle = minAngle;
        MaxAngle = maxAngle;
    }
}
