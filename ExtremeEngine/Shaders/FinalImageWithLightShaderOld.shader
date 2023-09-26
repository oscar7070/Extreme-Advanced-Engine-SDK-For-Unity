// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3641,x:34586,y:32661,varname:node_3641,prsc:2|emission-4879-OUT;n:type:ShaderForge.SFN_Tex2d,id:7107,x:33828,y:32633,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_7107,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8966,x:33427,y:32889,ptovrint:False,ptlb:LightTex,ptin:_LightTex,varname:node_8966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:388,x:33460,y:33262,ptovrint:False,ptlb:ShadowTex,ptin:_ShadowTex,varname:node_388,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8621,x:34061,y:32949,varname:node_8621,prsc:2|A-8966-A,B-571-OUT;n:type:ShaderForge.SFN_Divide,id:5318,x:33649,y:33081,varname:node_5318,prsc:2|A-4155-OUT,B-8966-A;n:type:ShaderForge.SFN_OneMinus,id:7390,x:33864,y:33081,varname:node_7390,prsc:2|IN-5318-OUT;n:type:ShaderForge.SFN_Multiply,id:9243,x:34061,y:32815,varname:node_9243,prsc:2|A-8966-RGB,B-8621-OUT;n:type:ShaderForge.SFN_Clamp01,id:571,x:34061,y:33081,varname:node_571,prsc:2|IN-7390-OUT;n:type:ShaderForge.SFN_Blend,id:4879,x:34061,y:32633,varname:node_4879,prsc:2,blmd:6,clmp:True|SRC-7107-RGB,DST-9243-OUT;n:type:ShaderForge.SFN_Tex2d,id:361,x:33460,y:33456,ptovrint:False,ptlb:NotSelf,ptin:_NotSelf,varname:node_361,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_OneMinus,id:4816,x:33667,y:33473,varname:node_4816,prsc:2|IN-361-A;n:type:ShaderForge.SFN_Multiply,id:735,x:33864,y:33296,varname:node_735,prsc:2|A-388-A,B-4816-OUT;n:type:ShaderForge.SFN_Tex2d,id:9011,x:33864,y:33456,ptovrint:False,ptlb:Self,ptin:_Self,varname:_NotSelf_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:4155,x:34071,y:33296,varname:node_4155,prsc:2|A-735-OUT,B-9011-A;proporder:7107-8966-388-361-9011;pass:END;sub:END;*/

Shader "Custom/MultiplayTextures" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _LightTex ("LightTex", 2D) = "white" {}
        _ShadowTex ("ShadowTex", 2D) = "white" {}
        _NotSelf ("NotSelf", 2D) = "white" {}
        _Self ("Self", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _LightTex; uniform float4 _LightTex_ST;
            uniform sampler2D _ShadowTex; uniform float4 _ShadowTex_ST;
            uniform sampler2D _NotSelf; uniform float4 _NotSelf_ST;
            uniform sampler2D _Self; uniform float4 _Self_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _LightTex_var = tex2D(_LightTex,TRANSFORM_TEX(i.uv0, _LightTex));
                float4 _ShadowTex_var = tex2D(_ShadowTex,TRANSFORM_TEX(i.uv0, _ShadowTex));
                float4 _NotSelf_var = tex2D(_NotSelf,TRANSFORM_TEX(i.uv0, _NotSelf));
                float4 _Self_var = tex2D(_Self,TRANSFORM_TEX(i.uv0, _Self));
                float3 emissive = saturate((1.0-(1.0-_MainTex_var.rgb)*(1.0-(_LightTex_var.rgb*(_LightTex_var.a*saturate((1.0 - (((_ShadowTex_var.a*(1.0 - _NotSelf_var.a))+_Self_var.a)/_LightTex_var.a))))))));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
