Shader "Unlit/UnlitSpec"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Glossiness", Range(1,100)) = 0.5
        _DiffuseThreshold ("DiffuseTrheshold", Range(.1,1)) = 0.3
        _Steps ("Steps", Range(1,12)) = 3
        _AmbientLight ("Ambient Light (swizzled)", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal :NORMAL;
                
            };

            fixed Posterize(fixed steps, fixed value)
            {
                return floor(value * steps) / steps;
            }

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2; 
            };

            fixed4 _Color;
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            fixed _Glossiness;
            fixed _DiffuseThreshold;
            fixed _SpecularThreshold;
            int _Steps;
            fixed4 _AmbientLight;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 normal = normalize(i.normal);
                float3 lightCol = _LightColor0.rgb;
                
                // specular highlight
                float3 fragToCam = _WorldSpaceCameraPos - i.worldPos;
                float3 viewDir = normalize(fragToCam);

                float3 viewReflection = reflect(-viewDir,  i.normal);
                fixed diffuseFallof = max(0, dot(lightDir, viewReflection));
                //diffuseFallof = step(_DiffuseThreshold, diffuseFallof);
                diffuseFallof = Posterize(_Steps, max(0,pow(diffuseFallof, _DiffuseThreshold)));
                float3 directDiffuse = diffuseFallof * lightCol;
                fixed specFalloff = max(0, dot(viewReflection, lightDir));
                //specFalloff = step(_SpecularThreshold, specFalloff);
                specFalloff = Posterize(_Steps, max(0,pow(specFalloff, _Glossiness)));
                

                float3 directSpec = specFalloff * lightCol;

                float3 diffuseLight = _AmbientLight.xxx + directDiffuse;
                float3 surfaceCol = diffuseLight * _Color.rgb + directSpec;
                
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(col * surfaceCol,0);
                
            }
            ENDCG
        }
    }
}
