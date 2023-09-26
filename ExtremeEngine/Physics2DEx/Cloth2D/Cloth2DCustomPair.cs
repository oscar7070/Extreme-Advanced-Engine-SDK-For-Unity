using UnityEngine;

public class Cloth2DCustomPair : MonoBehaviour
{

    public Cloth2D ClothToUse;
    //Can use only frozen points for this.
    public bool[] PairPoints;
    public bool[] FrozenPointsReadOnly;
    public enum PairModEnum
    {
        SelectNothing,
        SetToThisTransform,
        SetToTransform,
    }
    public PairModEnum PairMod;
    public Transform OtherTransform;

    /*private void OnDrawGizmos()
    {
        if (!Application.isPlaying & ClothToUse)
        {
            if (PairPoints.Length != ClothToUse.FrozenVertices.Length)
            {
                PairPoints = new bool[ClothToUse.FrozenVertices.Length];
            }
            FrozenPointsReadOnly = ClothToUse.FrozenVertices;
            SetFrozenVerticesCustomPair();
        }
    }

    private void Update()
    {
        SetFrozenVerticesCustomPair();
    }

    private void SetFrozenVerticesCustomPair()
    {
        for (int i = 0; i < ClothToUse.FrozenVerticesCustomPair.Length; i++)
        {
            if (PairPoints[i])
            {
                if (PairMod == PairModEnum.SetToThisTransform)
                {
                    ClothToUse.FrozenVerticesCustomPair[i] = transform.gameObject;
                }
                else if (PairMod == PairModEnum.SetToTransform)
                {
                    ClothToUse.FrozenVerticesCustomPair[i] = OtherTransform.gameObject;
                }
            }
        }
    }*/
}
