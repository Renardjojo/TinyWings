Shader "Skybox"
{
	Properties
	{
		_SkyColor("Sky Color", Color) = (0.37, 0.52, 0.73, 0)
		_SnowColor("Snow Color", Color) = (1.0, 1.0, 1.0, 0)
		_SnowRadius("Snow radius", float) = 0.01
		_SnowFrac("Snow Frac", float) = 100
		_SnowSpeed("Snow speed", float) = 1.0
		_SnowSmooth("Snow smooth", float) = 0.05
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Background" }
		ZWrite Off
		Fog{ Mode Off }
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 position : POSITION;
				float3 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 position : SV_POSITION;
				float3 uv : TEXCOORD0;
			};

			fixed3 _SkyColor;
			fixed3 _SnowColor;
			float _SnowRadius;
			float _SnowFrac;
			float _SnowSpeed;
			float _SnowSmooth;

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.position);
				o.uv = v.uv;
				return o;
			}

            float IsPointInsideDisk(float radius, float2 center,float2 pt, float transitionHalfWidth)
            {
                return smoothstep(radius - transitionHalfWidth, radius + transitionHalfWidth, length(center + pt));
            }

			fixed4 frag(v2f i) : COLOR
			{
			    float2 uv = normalize(i.uv - 0.5);
			    uv = frac(uv * _SnowFrac) -.5;
			    float2 center = float2(frac(_Time.y * _SnowSpeed) - 0.5, frac(_Time.y * _SnowSpeed) - 0.5);
			    float rst = IsPointInsideDisk(_SnowRadius, center, uv, _SnowSmooth);
                return fixed4(_SkyColor * rst + (1.0 - rst) * _SnowColor, 1.0);
			}
			ENDCG
		}
	}
}