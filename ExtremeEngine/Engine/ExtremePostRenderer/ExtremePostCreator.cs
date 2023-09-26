using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView, RequireComponent(typeof(Camera)), AddComponentMenu(ExtremeEngineData.EngineNameShort + "/PostCreator")]
public class ExtremePostCreator : MonoBehaviour
{

    public Status EnabledStatus = Status.On;
    public enum Status { Off, On, RenderToAllCameras }
    public PostProperties Post;

    [Serializable]
    public struct PostProperties
    {

        public List<Render> BeforePost;
        public int RuntimePostPreset;
        public ExtremePostAsset[] PostPresets;
        public List<Render> AfterPost;

        [Serializable]
        public struct Render
        {

        }

        /*private PostProperties()
        {

        }*/
    }
}
