Shader "Unlit/Test"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PosTex("Position Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members testValue)
#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _PosTex;
			float4 _MainTex_ST;
			float4 _PosTex_TexelSize;

			float3 ConvertValueFromRGB(float3 input)
			{
				float resultX = ((input.x - 0) / (1 - 0)) * (1 - -1) + -1;
				float resultY = ((input.y - 0) / (1 - 0)) * (1 - -1) + -1;
				float resultZ = ((input.z - 0) / (1 - 0)) * (1 - -1) + -1;

				float3 result = float3(resultX, resultY, resultZ);

				return result;
			}

			v2f vert(appdata v)
			{
				v2f o;

				float3 value = tex2Dlod(_PosTex, float4(0, v.vertexID / _PosTex_TexelSize.w, 0, 0));

				float3 newValue = ConvertValueFromRGB(value);

				o.vertex = UnityObjectToClipPos(newValue);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture

				fixed4 col = tex2D(_MainTex, i.uv);
			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
		}
		ENDCG
	}
	}
}
