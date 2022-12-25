// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:6,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32839,y:33206,varname:node_2865,prsc:2|emission-735-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6383,x:32471,y:33414,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_6383,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:735,x:32662,y:33267,varname:node_735,prsc:2|A-9717-OUT,B-6383-OUT;n:type:ShaderForge.SFN_Color,id:3515,x:31917,y:33428,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3515,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_RgbToHsv,id:5343,x:32092,y:33428,varname:node_5343,prsc:2|IN-3515-RGB;n:type:ShaderForge.SFN_HsvToRgb,id:207,x:32266,y:33428,varname:node_207,prsc:2|H-5343-HOUT,S-5343-SOUT,V-2848-OUT;n:type:ShaderForge.SFN_Vector1,id:2848,x:32092,y:33548,varname:node_2848,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:9717,x:32471,y:33267,varname:node_9717,prsc:2|A-8557-RGB,B-207-OUT;n:type:ShaderForge.SFN_Tex2d,id:8557,x:32266,y:33267,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_8557,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;proporder:6383-3515-8557;pass:END;sub:END;*/

Shader "Shader Forge/GlobalLightPostEffect" {
    Properties {
        _Intensity ("Intensity", Float ) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest Always
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define _GLOSSYENV 1
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Intensity)
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float4 node_5343_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_5343_p = lerp(float4(float4(_Color_var.rgb,0.0).zy, node_5343_k.wz), float4(float4(_Color_var.rgb,0.0).yz, node_5343_k.xy), step(float4(_Color_var.rgb,0.0).z, float4(_Color_var.rgb,0.0).y));
                float4 node_5343_q = lerp(float4(node_5343_p.xyw, float4(_Color_var.rgb,0.0).x), float4(float4(_Color_var.rgb,0.0).x, node_5343_p.yzx), step(node_5343_p.x, float4(_Color_var.rgb,0.0).x));
                float node_5343_d = node_5343_q.x - min(node_5343_q.w, node_5343_q.y);
                float node_5343_e = 1.0e-10;
                float3 node_5343 = float3(abs(node_5343_q.z + (node_5343_q.w - node_5343_q.y) / (6.0 * node_5343_d + node_5343_e)), node_5343_d / (node_5343_q.x + node_5343_e), node_5343_q.x);;
                float _Intensity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Intensity );
                float3 emissive = ((_MainTex_var.rgb*(lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac(node_5343.r+float3(0.0,-1.0/3.0,1.0/3.0)))-1),node_5343.g)*1.0))*_Intensity_var);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
