Shader "Custom/SinusoidShape"
{
    Properties
    {
        _Dim ("Dim", Vector) = (0,0,0,0)
        _Color ("Color", Color) = (1,0,0,1)
        [Toggle] _isDesc("is descendant", Float) = 0
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
            bool _isDesc;

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
                float   xMin = _Dim.x, 
                        yMin = _Dim.y,
                        xMax = _Dim.z,
                        yMax = _Dim.w; 
                        
                float width = xMax - xMin, height = yMax - yMin;
                
                float amplitude = height / 2;
                float vOffSet = amplitude + yMin;
                float pulsation = UNITY_PI / width;
                float phase = signWithBool(_isDesc) * UNITY_PI / 2 - xMin * pulsation;
                        
                return smoothstep(pt.y - transitionHalfWidth, pt.y + transitionHalfWidth, vOffSet + amplitude * sin(pulsation * pt.x + phase));
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
