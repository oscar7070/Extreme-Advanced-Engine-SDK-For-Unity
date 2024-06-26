Shader "Custom/MultiplayTextures" 
{

    Properties 
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _LightTex ("LightTex", 2D) = "white" {}
        _ShadowTex ("ShadowTex", 2D) = "white" {}
        _NotSelf ("NotSelf", 2D) = "white" {}
        _Self ("Self", 2D) = "white" {}
        _Normals ("Normals", 2D) = "white" {}
        //_Normals ("Normals", 2D) = "white" {}
        _RadialBlurAngle("RadialBlurAngle", float) = 2
        [ShowAsVector2] _Position("Position", vector) = (0, 0, 0, 0)

    }

    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        //LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _LightTex; uniform float4 _LightTex_ST;
            uniform sampler2D _ShadowTex; uniform float4 _ShadowTex_ST;
            uniform sampler2D _NotSelf; uniform float4 _NotSelf_ST;
            uniform sampler2D _Self; uniform float4 _Self_ST;
            uniform sampler2D _Normals; uniform float4 _Normals_ST;

            UNITY_INSTANCING_BUFFER_START( Props )
            UNITY_DEFINE_INSTANCED_PROP( float, _RadialBlurAngle)
            UNITY_DEFINE_INSTANCED_PROP( float2, _Position)
            UNITY_INSTANCING_BUFFER_END( Props )

            struct VertexInput 
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }

            fixed fastLerp(fixed a, fixed b, fixed w)
            {
                return a + w*(b-a);
            }

            float4 frag(VertexOutput i) : COLOR 
            {
                float4 mainTex = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 lightTex = tex2D(_LightTex,TRANSFORM_TEX(i.uv0, _LightTex));
                //float4 normalsTex = tex2D(_LightTex, TRANSFORM_TEX(i.uv0, _LightTex));
                //float4 _ShadowTex_var = tex2D(_ShadowTex,TRANSFORM_TEX(i.uv0, _ShadowTex));

                //SoftShadowsFilterStart
                const float Deg2Rad = 0.017453293;
                //const float Rad2Deg = 57.295777937;
                float2 coord = i.uv0;

                //min iters is 3
                const fixed samples = 32;
                const fixed angleInRad = _RadialBlurAngle * Deg2Rad;
                float finalShadows = tex2D(_ShadowTex, coord).a / samples;
                //Radial
                for (int f = 1; f < samples; f++)
                {
                        fixed rotationRadians = fastLerp(-angleInRad, angleInRad, (f / samples));
                        fixed s = sin(rotationRadians);
                        fixed c = cos(rotationRadians);
                        fixed2x2 rotationMatrix = fixed2x2(c, -s, s, c);
                 
                        coord -= .5 + _Position;
                        coord = mul(coord, rotationMatrix);
                        coord += .5 +_Position;

                        //half4 texel = tex2D(_ShadowTex,  mul (coord - _Position, rotationMatrix) + _Position) / iters;
                        half4 texel = tex2D(_ShadowTex, coord) / samples;
                        coord = i.uv0;
                        
                        //float4 texel = tex2D(_ShadowTex, mul (coord - (0.5+_Position), rotationMatrix ) + (0.5 +_Position)) / iters;
                        finalShadows += texel.a;
                }
                float4 notSelfTex = tex2D(_NotSelf,TRANSFORM_TEX(i.uv0, _NotSelf));
                float4 selfTex = tex2D(_Self,TRANSFORM_TEX(i.uv0, _Self));
                float4 normalsTex = tex2D(_Normals, TRANSFORM_TEX(i.uv0, _Normals));    
    fixed2 direction = (1.0 + i.pos / _ScreenParams) - _Position.xy;
    fixed3 lNormals = normalize(fixed3(direction.x, direction.y, 0));
    fixed normStr = dot(normalsTex, lNormals);
    fixed ref = max(.5f, reflect(-lNormals, normalsTex).z);
    //return normalsTex * lightTex;
    
    return fixed4((1.0 - (1.0 - mainTex.rgb) * (1.0 - (lightTex.rgb * (lightTex.a * saturate((1.0 - ((((finalShadows * lightTex.a) * (1.0 - notSelfTex.a)) + selfTex.a) / lightTex.a))))))).rgb, 1);
}
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
