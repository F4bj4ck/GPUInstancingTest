Shader "Unlit/test 1"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PosTex("Positions", 2D) = "black" {}
        _NormalTex("Normals", 2D) = "black" {}
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
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint vertexID : SV_VertexID;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D_float _PosTex;
            sampler2D_float _NormalTex;
            float4 _MainTex_ST;
            float4 _PosTex_TexelSize; 
            float4 _PosTex_ST;
            float4 _NormalTex_TexelSize;
            float4 _NormalTex_ST;

            float3 ConvertValueFromRGB(float3 input)
            {
                float resultX = (input.x - 0.5) * 2;
                float resultY = (input.y - 0.5) * 2;
                float resultZ = (input.z - 0.5) * 2;

                float3 result = float3(resultX, resultY, resultZ);

                return result;
            } 

            v2f vert(appdata v)
            {
                v2f o;

                float vertexValue = v.vertexID * _PosTex_TexelSize.y;
                float animValue = frac(_Time.w * _PosTex_TexelSize.y * 255);

                float halfTexelCoordY = 0.5 * _PosTex_TexelSize.y;

                float4 texCoords = float4(animValue, vertexValue + halfTexelCoordY, 0, 0);

                float3 value = tex2Dlod(_PosTex, texCoords);

                float3 newValue = ConvertValueFromRGB(value);

                o.vertex = UnityObjectToClipPos(newValue);

                float vertexNormalValue = v.vertexID * _NormalTex_TexelSize.y;
                float animNormalValue = frac(_Time.w * _NormalTex_TexelSize.y * 255);

                float halfNormalTexelCoordY = 0.5 * _NormalTex_TexelSize.y;

                float4 texNormalCoords = float4(animNormalValue, vertexNormalValue + halfNormalTexelCoordY, 0, 0);

                float3 normalValue = tex2Dlod(_NormalTex, texNormalCoords);

                float3 newNormalValue = ConvertValueFromRGB(normalValue);

                o.normal = UnityObjectToWorldNormal(newNormalValue);

                o.uv = TRANSFORM_TEX(v.uv, _PosTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
             
            //col = frac(_Time.y * col);

                return col;
            }
            ENDCG
        }
    }
}
