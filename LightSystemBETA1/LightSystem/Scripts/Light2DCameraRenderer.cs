using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, ImageEffectAllowedInSceneView, RequireComponent(typeof(Camera)), AddComponentMenu("Light2D/Light2D camera renderer")]
public class Light2DCameraRenderer : MonoBehaviour
{

    private Camera Camera => GetComponent<Camera>();
    public delegate void OnRenderImageDelegate(RenderTexture source, Light2DCameraRenderer light2DCameraRenderer, Camera camera, List<Light2DCollider> colliders, int maxDrawenLigths);
    public static OnRenderImageDelegate OnRenderImageCallbackForLight;
    private readonly List<BlitLightSettings> ToBlit = new();
    private readonly List<RenderTexture> RenderTexturesToDestoryFromMemoryLeak = new();
    private readonly List<Light2DCollider> Colliders = new();
    [HideInInspector] public int ActiveLightsCount = 0;
    [HideInInspector] public float GlobalLightIntensity = 1;
    [SerializeField] private Light2DSettings Light2DSettings;
    private readonly List<ToRender> ToRenderList = new();
    private CommandBuffer Buffer;

#if UNITY_EDITOR
    [HideInInspector] public bool DisableLightInSceneView = false;
#endif

    private void OnDestroy()
    {
        OnRenderImageCallbackForLight = null;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (source)
        {
#if UNITY_EDITOR
            if (!DisableLightInSceneView)
            {
#endif
                OnRenderImageCallbackForLight?.Invoke(source, this, Camera, Colliders, Light2DSettings.GetMaximumDrawenLights());
#if UNITY_EDITOR
            }
#endif
            if (ActiveLightsCount != 0)
            {
                RenderAllLights(source);
            }
            //oscar7070 note:
            //This script is from the web i don't found how to blit more then one time.
            //Thenks web!

            ToBlit.Sort((s1, s2) => s1.Layer.CompareTo(s2.Layer));

            RenderTexture tempSrc = RenderTexture.GetTemporary(source.width, source.height, source.depth, source.format);
            RenderTexture tempDst = RenderTexture.GetTemporary(source.width, source.height, source.depth, source.format);
            //^grab two temp textures that are the same as the source;
            Graphics.Blit(source, tempSrc); //blit the source into the tempSrc;
            for (int i = 0; i < ToBlit.Count; i++)
            { //for all the materials;
                if (i % 2.0f == 0.0f)
                { //if i is even blit from src to dst, if not then dst to src.
                    Graphics.Blit(tempSrc, tempDst, ToBlit[i].Material);
                }
                else
                {
                    Graphics.Blit(tempDst, tempSrc, ToBlit[i].Material);
                }
            }
            if (ToBlit.Count % 2.0f == 0.0f)
            { //if the total number of materials is even;
              //then we blit from the tempSrc;
                Graphics.Blit(tempSrc, destination); //final blit from tempSrc to dest;
            }
            else
            { //if not;
              //then we blit from the tempDst;
                Graphics.Blit(tempDst, destination); //final blit from tempDst to dest;
            }
            RenderTexture.ReleaseTemporary(tempSrc);
            RenderTexture.ReleaseTemporary(tempDst);
            ToRenderList.Clear();
            ToBlit.Clear();
            ActiveLightsCount = 0;
            GlobalLightIntensity = 1;
        }
        //To prevent memory leak!
        for (int i = 0; i < RenderTexturesToDestoryFromMemoryLeak.Count; i++)
        {
            RenderTexture.ReleaseTemporary(RenderTexturesToDestoryFromMemoryLeak[i]);
        }
        RenderTexturesToDestoryFromMemoryLeak.Clear();
        Colliders.Clear();
    }

    private void RenderAllLights(RenderTexture source)
    {
        RenderTextureDescriptor renderTextureDescriptor = GenerateRTD();
        Buffer = new() { name = "LightSystem2D cmd" };
        List<Light2DCollider> colliders = new(Colliders.Where(c => !c.SelfShadows));
        List<Light2DCollider> slefColliders = new(Colliders.Where(c => c.SelfShadows));

        RenderTexture silhouetteTempRT = RenderTexture.GetTemporary(renderTextureDescriptor);

        Buffer.SetRenderTarget(silhouetteTempRT);
        Buffer.ClearRenderTarget(false, true, Color.clear);
        DrawSprites(colliders);
        DestroyTextureToPreventMemoryLeak(silhouetteTempRT);

        RenderTexture selfSilhouetteTempRT = RenderTexture.GetTemporary(renderTextureDescriptor);

        Buffer.SetRenderTarget(selfSilhouetteTempRT);
        Buffer.ClearRenderTarget(false, true, Color.clear);
        DrawSprites(slefColliders);
        DestroyTextureToPreventMemoryLeak(selfSilhouetteTempRT);

        WaitenToDrawLightTextures[] lightTextures = new WaitenToDrawLightTextures[ToRenderList.Count];

        Material lightMaterial = Light2DSettings.LightMat;
        Material shadowMaterial = Light2DSettings.ShadowMat;

        //test function
        //Vector4[] lightTextures = new Vector4[ lightTextures.Length];

        for (int i = 0; i < lightTextures.Length; i++)
        {
            lightTextures[i] = DrawLight(ToRenderList[i], lightMaterial, shadowMaterial, renderTextureDescriptor, source);
            //lightTextures[i] = (Vector4)lightTextures[i].Light;
        }

        //Shader.SetGlobalBuffer("_LSLightsBuffer", new ComputeBuffer(0,0));
        Shader.SetGlobalVector("_LSLightPos", (Vector2)Camera.WorldToScreenPoint(ToRenderList[0].Matrix.GetPosition()));
        Shader.SetGlobalInt("NormalMapsEnabled", (int)Light2DSettings.GetNormalMapsEnabled());
        Shader.SetGlobalFloat("_LSLightSize", ToRenderList[0].Light.Size);
        Shader.SetGlobalMatrix("_LSLightsMatrix", ToRenderList[0].Matrix);
        Shader.SetGlobalTexture("_LSLightsTex", lightTextures[0].Light);

        Graphics.ExecuteCommandBuffer(Buffer);
        Buffer.Dispose();

        Vector2 ScrSize = new(Camera.pixelWidth, Camera.pixelHeight);
        Vector2 half = Vector2.one * .5f;

        for (int i = 0; i < lightTextures.Length; i++)
        {
            ToRenderList[i].FinalImageMaterial.SetTexture("_MainTex", source);
            ToRenderList[i].FinalImageMaterial.SetTexture("_NotSelf", silhouetteTempRT);
            ToRenderList[i].FinalImageMaterial.SetTexture("_Self", selfSilhouetteTempRT);
            ToRenderList[i].FinalImageMaterial.SetTexture("_LightTex", lightTextures[i].Light);
            ToRenderList[i].FinalImageMaterial.SetTexture("_ShadowTex", lightTextures[i].Shadow);
            ToRenderList[i].FinalImageMaterial.SetVector("_Position", ((Vector2)Camera.WorldToScreenPoint(ToRenderList[i].Matrix.GetPosition()) / ScrSize) - half);
            BlitLight(new BlitLightSettings(ToRenderList[i].FinalImageMaterial, 1));
            for (int ir = 0; ir < ToRenderList[i].ShadowRenderInfo.Count; ir++)
            {
                AutoDestroy.Execute(ToRenderList[i].ShadowRenderInfo[ir].Mesh);
            }
            AutoDestroy.Execute(ToRenderList[i].LightMesh);
        }
    }

    private WaitenToDrawLightTextures DrawLight(ToRender render, Material lightMaterial, Material shadowMaterial, RenderTextureDescriptor renderTextureDescriptor, RenderTexture source)
    {
        RenderTexture lightTempRT = RenderTexture.GetTemporary(renderTextureDescriptor);

        Buffer.SetRenderTarget(lightTempRT);
        Buffer.ClearRenderTarget(false, true, Color.clear);

        render.LightMaterialPropertyBlock.SetTexture("_CameraSource", source);
        Buffer.DrawMesh(render.LightMesh, render.Matrix, lightMaterial, 0, -1, render.LightMaterialPropertyBlock);

        RenderTexture shadowTempRT = RenderTexture.GetTemporary(renderTextureDescriptor);

        Buffer.SetRenderTarget(shadowTempRT);
        Buffer.ClearRenderTarget(false, true, Color.clear);

        //fix of transparent shadow visibility behind other shadow.
        render.ShadowRenderInfo.Sort((s1, s2) => s1.Collider.Alpha.CompareTo(s2.Collider.Alpha));

        for (int i = 0; i < render.ShadowRenderInfo.Count; i++)
        {
            MaterialPropertyBlock shadowPropertyBlock = new();
            shadowPropertyBlock.SetFloat("_Alpha", render.ShadowRenderInfo[i].Collider.Alpha);
            Buffer.DrawMesh(render.ShadowRenderInfo[i].Mesh, render.Matrix, shadowMaterial, 0, -1, shadowPropertyBlock);
        }

        DestroyTextureToPreventMemoryLeak(lightTempRT);
        DestroyTextureToPreventMemoryLeak(shadowTempRT);

        return new WaitenToDrawLightTextures(lightTempRT, shadowTempRT);
    }

    private RenderTextureDescriptor GenerateRTD()
    {
        float resolution = Light2DSettings.GetLightResolution();
        return new((int)Mathf.Max(4, Camera.main.pixelWidth * resolution), (int)Mathf.Max(4, Camera.main.pixelHeight * resolution), RenderTextureFormat.DefaultHDR);
    }

    private void DrawSprites(List<Light2DCollider> usedColliders)
    {
        for (int i = 0; i < usedColliders.Count; i++)
        {
            Light2DCollider.SpriteParameters spriteParameters = usedColliders[i].TryGetSpriteParamsForShadow();
            if (spriteParameters.Mesh)
            {
                Buffer.DrawMesh(spriteParameters.Mesh, spriteParameters.Matrix, spriteParameters.SharedMaterial, 0, -1, spriteParameters.MaterialPropertyBlock);
            }
        }
    }

    private readonly struct WaitenToDrawLightTextures
    {
        public readonly RenderTexture Light;
        public readonly RenderTexture Shadow;

        public WaitenToDrawLightTextures(RenderTexture light, RenderTexture shadow)
        {
            Light = light;
            Shadow = shadow;
        }
    }

    public readonly struct BlitLightSettings
    {
        public readonly Material Material;
        public readonly int Layer;
        //Layer = 0; GlobalLight
        //Layer = 1; Shadows
        //Layer = 2; Light

        public BlitLightSettings(Material material, int layer = 0)
        {
            Material = material;
            Layer = layer;
        }
    }

    public void BlitLight(BlitLightSettings blitLightSettings, RenderTexture renderTexturesToDestoryFromMemoryLeak = null)
    {
        /*#if UNITY_EDITOR
                if (!LightInSceneView & (blitLightSettings.Layer != 0 || blitLightSettings.Layer != 1 || blitLightSettings.Layer != 2))
                {
        #endif*/
        ToBlit.Add(blitLightSettings);
        DestroyTextureToPreventMemoryLeak(renderTexturesToDestoryFromMemoryLeak);
        /*#if UNITY_EDITOR
                }
        #endif*/
    }

    private void DestroyTextureToPreventMemoryLeak(RenderTexture renderTexture)
    {
        RenderTexturesToDestoryFromMemoryLeak?.Add(renderTexture);
    }

    private struct ToRender
    {
        public readonly Mesh LightMesh;
        public MaterialPropertyBlock LightMaterialPropertyBlock;
        public Material FinalImageMaterial;
        public readonly List<Light2D.ShadowMeshInfo> ShadowRenderInfo;
        public readonly Matrix4x4 Matrix;
        public readonly Light2D Light;

        public ToRender(Mesh lightMesh, MaterialPropertyBlock lightMaterialPropertyBlock, Material finalImageMaterial, List<Light2D.ShadowMeshInfo> shadowRenderInfo, Matrix4x4 matrix, Light2D light)
        {
            LightMesh = lightMesh;
            LightMaterialPropertyBlock = lightMaterialPropertyBlock;
            FinalImageMaterial = finalImageMaterial;
            ShadowRenderInfo = shadowRenderInfo;
            Matrix = matrix;
            Light = light;
        }
    }

    public void SetLightToRender(Mesh lightMesh, MaterialPropertyBlock lightMaterialPropertyBlock, Material finalImageMaterial, List<Light2D.ShadowMeshInfo> shadowRenderInfo, Matrix4x4 matrix, Light2D light)
    {
        ToRenderList.Add(new(lightMesh, lightMaterialPropertyBlock, finalImageMaterial, shadowRenderInfo, matrix, light));
    }

#if UNITY_EDITOR
    public void SetLightEnebledInSceneView(bool enabled)
    {
        DisableLightInSceneView = enabled;
    }
#endif
}
