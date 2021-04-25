Shader "ColorOnly"
{
	Properties
	{
		_Color("Color", Color) = (0.37, 0.52, 0.73, 0)
	}
		SubShader
	{
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
      
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
                    
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

			fixed3 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.position);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
                return fixed4(_Color, 1.0f);
			}
			ENDCG
		}
	}
}