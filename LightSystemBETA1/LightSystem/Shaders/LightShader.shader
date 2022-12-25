// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:True,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32992,y:32490,varname:node_3138,prsc:2|emission-6321-OUT,alpha-2277-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2726,x:32562,y:32685,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_2726,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_TexCoord,id:742,x:31959,y:33065,varname:node_742,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Length,id:1518,x:32364,y:33065,varname:node_1518,prsc:2|IN-359-OUT;n:type:ShaderForge.SFN_RemapRange,id:359,x:32141,y:33065,varname:node_359,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-742-UVOUT;n:type:ShaderForge.SFN_Slider,id:4656,x:32385,y:33252,ptovrint:False,ptlb:FallOff,ptin:_FallOff,varname:node_4656,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:2480,x:32780,y:32641,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_2480,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Divide,id:4672,x:32780,y:33065,varname:node_4672,prsc:2|A-9963-OUT,B-2416-OUT;n:type:ShaderForge.SFN_OneMinus,id:2416,x:32780,y:33197,varname:node_2416,prsc:2|IN-4656-OUT;n:type:ShaderForge.SFN_Clamp01,id:8183,x:32780,y:32932,varname:node_8183,prsc:2|IN-4672-OUT;n:type:ShaderForge.SFN_OneMinus,id:9963,x:32542,y:33065,varname:node_9963,prsc:2|IN-1518-OUT;n:type:ShaderForge.SFN_Tex2d,id:2858,x:32562,y:32490,ptovrint:False,ptlb:CameraSource,ptin:_CameraSource,varname:node_2858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6019-UVOUT;n:type:ShaderForge.SFN_ScreenPos,id:6019,x:32374,y:32490,varname:node_6019,prsc:2,sctp:2;n:type:ShaderForge.SFN_Multiply,id:6321,x:32780,y:32490,varname:node_6321,prsc:2|A-2858-RGB,B-2480-RGB,C-2726-OUT;n:type:ShaderForge.SFN_ComponentMask,id:4355,x:31954,y:33327,varname:node_4355,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4606-UVOUT;n:type:ShaderForge.SFN_ArcTan2,id:6634,x:32141,y:33327,varname:node_6634,prsc:2,attp:3|A-4355-G,B-4355-R;n:type:ShaderForge.SFN_Subtract,id:3993,x:32382,y:33358,varname:node_3993,prsc:2|A-6634-OUT,B-2336-OUT;n:type:ShaderForge.SFN_Slider,id:2336,x:31984,y:33500,ptovrint:False,ptlb:Apearness,ptin:_Apearness,varname:node_2336,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6150491,max:1;n:type:ShaderForge.SFN_Divide,id:2282,x:32563,y:33358,varname:node_2282,prsc:2|A-3993-OUT,B-6936-OUT;n:type:ShaderForge.SFN_Clamp01,id:2786,x:32780,y:33358,varname:node_2786,prsc:2|IN-2282-OUT;n:type:ShaderForge.SFN_Slider,id:7978,x:31984,y:33591,ptovrint:False,ptlb:Fade,ptin:_Fade,varname:node_7978,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Vector1,id:4726,x:32382,y:33646,varname:node_4726,prsc:2,v1:-1;n:type:ShaderForge.SFN_Multiply,id:624,x:32382,y:33500,varname:node_624,prsc:2|A-2336-OUT,B-7978-OUT;n:type:ShaderForge.SFN_Add,id:6936,x:32780,y:33500,varname:node_6936,prsc:2|A-6801-OUT,B-7978-OUT;n:type:ShaderForge.SFN_Multiply,id:6801,x:32563,y:33500,varname:node_6801,prsc:2|A-624-OUT,B-4726-OUT;n:type:ShaderForge.SFN_Multiply,id:1116,x:31565,y:33323,varname:node_1116,prsc:2|A-2420-OUT,B-3280-OUT;n:type:ShaderForge.SFN_Rotator,id:4606,x:31777,y:33327,varname:node_4606,prsc:2|UVIN-359-OUT,PIV-2053-OUT,ANG-1116-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3280,x:31401,y:33404,ptovrint:False,ptlb:Rotation,ptin:_Rotation,varname:node_3280,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Vector2,id:2053,x:31565,y:33235,varname:node_2053,prsc:2,v1:0,v2:0;n:type:ShaderForge.SFN_Tex2d,id:6440,x:32364,y:32805,ptovrint:False,ptlb:Cookie,ptin:_Cookie,varname:node_6440,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5481-UVOUT;n:type:ShaderForge.SFN_RgbToHsv,id:2957,x:32556,y:32805,varname:node_2957,prsc:2|IN-6440-RGB;n:type:ShaderForge.SFN_Vector1,id:2420,x:31401,y:33323,varname:node_2420,prsc:2,v1:0.01745329;n:type:ShaderForge.SFN_Rotator,id:5481,x:32162,y:32805,varname:node_5481,prsc:2|UVIN-742-UVOUT,ANG-1116-OUT;n:type:ShaderForge.SFN_Multiply,id:2277,x:32780,y:32805,varname:node_2277,prsc:2|A-2957-VOUT,B-2786-OUT,C-8183-OUT;proporder:2480-2726-4656-2858-2336-7978-3280-6440;pass:END;sub:END;*/

Shader "Shader Forge/LightShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Intensity ("Intensity", Float ) = 1
        _FallOff ("FallOff", Range(0, 1)) = 0
        _CameraSource ("CameraSource", 2D) = "white" {}
        _Apearness ("Apearness", Range(0, 1)) = 0.6150491
        _Fade ("Fade", Range(0, 1)) = 1
        _Rotation ("Rotation", Float ) = 0
        _Cookie ("Cookie", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _CameraSource; uniform float4 _CameraSource_ST;
            uniform sampler2D _Cookie; uniform float4 _Cookie_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Intensity)
                UNITY_DEFINE_INSTANCED_PROP( float, _FallOff)
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _Apearness)
                UNITY_DEFINE_INSTANCED_PROP( float, _Fade)
                UNITY_DEFINE_INSTANCED_PROP( float, _Rotation)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
////// Lighting:
////// Emissive:
                float4 _CameraSource_var = tex2D(_CameraSource,TRANSFORM_TEX(sceneUVs.rg, _CameraSource));
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float _Intensity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Intensity );
                float3 emissive = (_CameraSource_var.rgb*_Color_var.rgb*_Intensity_var);
                float3 finalColor = emissive;
                float _Rotation_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Rotation );
                float node_1116 = (0.01745329*_Rotation_var);
                float node_5481_ang = node_1116;
                float node_5481_spd = 1.0;
                float node_5481_cos = cos(node_5481_spd*node_5481_ang);
                float node_5481_sin = sin(node_5481_spd*node_5481_ang);
                float2 node_5481_piv = float2(0.5,0.5);
                float2 node_5481 = (mul(i.uv0-node_5481_piv,float2x2( node_5481_cos, -node_5481_sin, node_5481_sin, node_5481_cos))+node_5481_piv);
                float4 _Cookie_var = tex2D(_Cookie,TRANSFORM_TEX(node_5481, _Cookie));
                float4 node_2957_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_2957_p = lerp(float4(float4(_Cookie_var.rgb,0.0).zy, node_2957_k.wz), float4(float4(_Cookie_var.rgb,0.0).yz, node_2957_k.xy), step(float4(_Cookie_var.rgb,0.0).z, float4(_Cookie_var.rgb,0.0).y));
                float4 node_2957_q = lerp(float4(node_2957_p.xyw, float4(_Cookie_var.rgb,0.0).x), float4(float4(_Cookie_var.rgb,0.0).x, node_2957_p.yzx), step(node_2957_p.x, float4(_Cookie_var.rgb,0.0).x));
                float node_2957_d = node_2957_q.x - min(node_2957_q.w, node_2957_q.y);
                float node_2957_e = 1.0e-10;
                float3 node_2957 = float3(abs(node_2957_q.z + (node_2957_q.w - node_2957_q.y) / (6.0 * node_2957_d + node_2957_e)), node_2957_d / (node_2957_q.x + node_2957_e), node_2957_q.x);;
                float node_4606_ang = node_1116;
                float node_4606_spd = 1.0;
                float node_4606_cos = cos(node_4606_spd*node_4606_ang);
                float node_4606_sin = sin(node_4606_spd*node_4606_ang);
                float2 node_4606_piv = float2(0,0);
                float2 node_359 = (i.uv0*2.0+-1.0);
                float2 node_4606 = (mul(node_359-node_4606_piv,float2x2( node_4606_cos, -node_4606_sin, node_4606_sin, node_4606_cos))+node_4606_piv);
                float2 node_4355 = node_4606.rg;
                float _Apearness_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Apearness );
                float _Fade_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Fade );
                float _FallOff_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FallOff );
                return fixed4(finalColor,(node_2957.b*saturate((((1-abs(atan2(node_4355.g,node_4355.r)/3.14159265359))-_Apearness_var)/(((_Apearness_var*_Fade_var)*(-1.0))+_Fade_var)))*saturate(((1.0 - length(node_359))/(1.0 - _FallOff_var)))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
