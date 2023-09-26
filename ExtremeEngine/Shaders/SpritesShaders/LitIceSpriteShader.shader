// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33779,y:32723,varname:node_1873,prsc:2|emission-5575-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32551,y:32729,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32996,y:32720,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-5376-RGB;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32551,y:32891,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:33042,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5376-A;n:type:ShaderForge.SFN_Tex2d,id:5567,x:32812,y:32868,ptovrint:False,ptlb:NormalTex,ptin:_NormalTex,varname:node_5567,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_RgbToHsv,id:5528,x:33214,y:32720,varname:node_5528,prsc:2|IN-1086-OUT;n:type:ShaderForge.SFN_HsvToRgb,id:5575,x:33445,y:32720,varname:node_5575,prsc:2|H-5528-HOUT,S-5528-SOUT,V-4927-OUT;n:type:ShaderForge.SFN_Multiply,id:4927,x:33214,y:32868,varname:node_4927,prsc:2|A-5528-VOUT,B-466-VOUT;n:type:ShaderForge.SFN_RgbToHsv,id:466,x:32996,y:32868,varname:node_466,prsc:2|IN-5567-RGB;n:type:ShaderForge.SFN_Noise,id:7931,x:32995,y:33195,varname:node_7931,prsc:2|XY-8455-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8455,x:32803,y:33195,varname:node_8455,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:6117,x:33533,y:33111,varname:node_6117,prsc:2|A-5575-OUT,B-3251-OUT;n:type:ShaderForge.SFN_RemapRange,id:3251,x:33182,y:33195,varname:node_3251,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7931-OUT;proporder:4805-5567;pass:END;sub:END;*/

Shader "Shader Forge/LitSpriteShader" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _NormalTex ("NormalTex", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            Stencil {
                Ref 128
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _NormalTex; uniform float4 _NormalTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_1086 = (_MainTex_var.rgb*i.vertexColor.rgb); // RGB
                float4 node_5528_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_5528_p = lerp(float4(float4(node_1086,0.0).zy, node_5528_k.wz), float4(float4(node_1086,0.0).yz, node_5528_k.xy), step(float4(node_1086,0.0).z, float4(node_1086,0.0).y));
                float4 node_5528_q = lerp(float4(node_5528_p.xyw, float4(node_1086,0.0).x), float4(float4(node_1086,0.0).x, node_5528_p.yzx), step(node_5528_p.x, float4(node_1086,0.0).x));
                float node_5528_d = node_5528_q.x - min(node_5528_q.w, node_5528_q.y);
                float node_5528_e = 1.0e-10;
                float3 node_5528 = float3(abs(node_5528_q.z + (node_5528_q.w - node_5528_q.y) / (6.0 * node_5528_d + node_5528_e)), node_5528_d / (node_5528_q.x + node_5528_e), node_5528_q.x);;
                float4 _NormalTex_var = tex2D(_NormalTex,TRANSFORM_TEX(i.uv0, _NormalTex));
                float4 node_466_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_466_p = lerp(float4(float4(_NormalTex_var.rgb,0.0).zy, node_466_k.wz), float4(float4(_NormalTex_var.rgb,0.0).yz, node_466_k.xy), step(float4(_NormalTex_var.rgb,0.0).z, float4(_NormalTex_var.rgb,0.0).y));
                float4 node_466_q = lerp(float4(node_466_p.xyw, float4(_NormalTex_var.rgb,0.0).x), float4(float4(_NormalTex_var.rgb,0.0).x, node_466_p.yzx), step(node_466_p.x, float4(_NormalTex_var.rgb,0.0).x));
                float node_466_d = node_466_q.x - min(node_466_q.w, node_466_q.y);
                float node_466_e = 1.0e-10;
                float3 node_466 = float3(abs(node_466_q.z + (node_466_q.w - node_466_q.y) / (6.0 * node_466_d + node_466_e)), node_466_d / (node_466_q.x + node_466_e), node_466_q.x);;
                float3 node_5575 = (lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac(node_5528.r+float3(0.0,-1.0/3.0,1.0/3.0)))-1),node_5528.g)*(node_5528.b*node_466.b));
                float3 emissive = node_5575;
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a*i.vertexColor.a));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
