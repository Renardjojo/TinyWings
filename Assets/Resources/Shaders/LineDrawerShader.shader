Shader "Custom/LineDrawerShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _Segment ("Segment", Vector) = (0,0,0,0)
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

            fixed4 _Color;
            float4 _Segment;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
           
            float2 LocalToGlobalUVInRect(float2 uv)
            {                
                return float2(uv.x * (_Segment.z - _Segment.x) + _Segment.x, uv.y * (_Segment.w - _Segment.y) + _Segment.y);
            }
            
            float GetSignedDistanceToLine(float2 linePt, float2 lineUnitDir, float2 pt)
            {
                return dot(float2(-lineUnitDir.y, lineUnitDir.x), pt - linePt);
            }

            float GetSignedDistanceToLine2(float2 linePt1, float2 linePt2, float2 pt)
            {
                float2 lineUnitDir = normalize(linePt1 - linePt2);
                return dot(float2(-lineUnitDir.y, lineUnitDir.x), pt - linePt1);
            }
            
           float IsPtBellowLine(float2 pt1, float2 pt2, float2 localFragPos, float transitionHalfWidth = 0.001)
           {
                return smoothstep(-transitionHalfWidth, transitionHalfWidth, GetSignedDistanceToLine2(pt1, pt2, localFragPos));
           }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color * IsPtBellowLine(float2(_Segment.x, _Segment.y), float2(_Segment.z, _Segment.w), LocalToGlobalUVInRect(i.uv));
            }
            ENDCG
        }
    }
}
