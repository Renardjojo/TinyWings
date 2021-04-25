Shader "Custom/SinusoidShape"
{
    Properties
    {
        _Height ("Height", float) = 0
        _Width ("Width", float) = 0
        _Color ("Color", Color) = (1,0,0,1)
        _Amplitude ("Amplitude", float) = 0
        _VOffset ("VOffset", float) = 0
        _Pulsation ("Pulsation", float) = 0
        _Phase ("Phase", float) = 0
        _Pow ("Pow", float) = 0
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
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Height;
            float _Width;
            fixed4 _Color;
            fixed _Amplitude;
            fixed _VOffset;
            fixed _Pulsation;
            fixed _Phase;
            fixed _Pow;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float IsPtInsideFunction(fixed2 pt, fixed transitionHalfWidth = .005)
            {
                float powSin = 1.f;
                
                for (int i = 0; i < _Pow; ++i)
                {
                    powSin *= sin(_Pulsation * pt.x + _Phase);
                }
                
                return smoothstep(pt.y - transitionHalfWidth, pt.y + transitionHalfWidth, _VOffset + _Amplitude * powSin);
            }
            
            float2 localToGlobalUVInRect(fixed2 uv)
            {
                float2 pt = uv;
                pt.x *= _Width;
                pt.y *= _Height;
                
                return pt;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color * IsPtInsideFunction(localToGlobalUVInRect(i.uv));
            }
            ENDCG
        }
    }
}
