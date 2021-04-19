Shader "Custom/LineDrawerShader"
{
    Properties
    {
        _Dim ("Dim", Vector) = (0,0,0,0)
        _Color ("Color", Color) = (1,0,0,1)
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
            //uniform int _Points_Length = 0;
            uniform float4 _Points [25]; // in float2 but contain 2 point with (x, y) = position

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float SignWithBool(float boolean)
            {
                return boolean * 2 - 1;
            }
           
            float2 LocalToGlobalUVInRect(float2 uv)
            {                
                return float2(uv.x * (_Dim.z - _Dim.x) + _Dim.x, uv.y * (_Dim.w - _Dim.y) + _Dim.y);
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
                float4 fragColorRst = {0.0, 0.0, 0.0, 0.0};
                fixed2 localUV = LocalToGlobalUVInRect(i.uv);
                
                for (int i = 0; i < 24; i += 3)
                {
                    fragColorRst = IsPtBellowLine(float2(_Points[i].x, _Points[i].y), float2(_Points[i].z, _Points[i].w), localUV) *
                                   IsPtBellowLine(float2(_Points[i].z, _Points[i].w), float2(_Points[i + 1].x, _Points[i + 1].y), localUV) * 
                                   IsPtBellowLine(float2(_Points[i + 1].x, _Points[i + 1].y), float2(_Points[i + 1].y, _Points[i + 1].z), localUV);
                }
                
                return _Color * fragColorRst;
            }
            ENDCG
        }
    }
}
