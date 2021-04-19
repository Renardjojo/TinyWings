Shader "Custom/PolynomeShape"
{
    Properties
    {
        _Dim ("Dim", Vector) = (0,0,0,0)
        _Color ("Color", Color) = (1,0,0,1)
        _A ("A", float) = 0
        _B ("B", float) = 0
        _C ("C", float) = 0
        _D ("D", float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Blend One One
                    
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

            float4 _Dim;
            fixed4 _Color;
            float _A;
            float _B;
            float _C;
            float _D;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float signWithBool(float boolean)
            {
                return boolean * 2 - 1;
            }
            
            float IsPtInsideFunction(float2 pt, float transitionHalfWidth = .005)
            {
                return smoothstep(pt.y - transitionHalfWidth, pt.y + transitionHalfWidth, ((_A * pt.x + _B) * pt.x + _C) * pt.x + _D);
            }
            
            float2 localToGlobalUVInRect(float2 uv)
            {
                float2 pt = uv;
                pt.x *= _Dim.z - _Dim.x;
                pt.y *= _Dim.w - _Dim.y;
                
                pt.x += _Dim.x;
                pt.y += _Dim.y;
                
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
