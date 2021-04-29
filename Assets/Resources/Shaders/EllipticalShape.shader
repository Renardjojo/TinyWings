Shader "Unlit/EllipticalShape"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _ColorEdge ("ColorEdge", Color) = (1,0,0,1)
        _Edge ("Edge", float) = 1.0
        _Smoothness ("Smoothness", float) = 0.05
        _Height("Height", float) = 0
        _Width("Width", float) = 0
        _XMin("XMin", float) = 0
        _XMax("XMax", float) = 0
        _YMin("YMin", float) = 0
        _YMax("YMax", float) = 0
        _IsDesc("IsDesc", int) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _ColorEdge;
            fixed4 _Color;
            float _Edge;
            float _Smoothness;
            float _Height;
            float _Width;
            float _XMin;
            float _XMax;
            float _YMin;
            float _YMax;
            int _IsDesc;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float computeElliptic(float x)
            {
                float vOffSet = 0.0f;
                float sqr3 = sqrt(3);

                if (_IsDesc == 1)
                {
                    vOffSet = _YMax - _YMin;

                    if (x < (_XMax - _XMin) / 2.0f + _XMin)
                         return _YMin + vOffSet * sqrt(1 - pow(sqr3 * (x - _XMin) / (_XMax - _XMin), 2));
                    else
                       return _YMax - vOffSet * sqrt(1 - pow(sqr3 * (x - _XMax) / (_XMax - _XMin), 2));
                }
                else
                {
                    if (x < (_XMax - _XMin) / 2.0f + _XMin)
                    {
                        vOffSet = _YMin - _YMax;
                        return _YMax + vOffSet * sqrt(1 - pow(sqr3 * (x - _XMin) / (_XMax - _XMin), 2));
                    }
                    else
                    {
                        vOffSet = _YMax - _YMin;
                        return _YMin + vOffSet * sqrt(1 - pow(sqr3 * (x - _XMax) / (_XMax - _XMin), 2));
                    }
                }
            }

            float2 localToGlobalUVInRect(fixed2 uv)
            {
                return float2(uv.x * _Width, uv.y * _Height);
            }

            fixed4 ComputeCurveColor(fixed2 pt)
            {
                float y = computeElliptic(pt.x);
                
                float isPtInsideFunctionEdgeValue = smoothstep(pt.y - _Smoothness, pt.y + _Smoothness, y) * smoothstep(y - _Smoothness * 20.0, y + _Smoothness * 20.0, pt.y + _Edge);
                float isPtInsideFunctionValue = smoothstep(pt.y - _Smoothness, pt.y + _Smoothness, y);
                return _Color * isPtInsideFunctionValue * (1.0 - isPtInsideFunctionEdgeValue) + isPtInsideFunctionEdgeValue * _ColorEdge;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return ComputeCurveColor(localToGlobalUVInRect(i.uv));
            }
            
            ENDCG
        }
    }
}
