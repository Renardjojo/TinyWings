Shader "Unlit/EllipticalShape"
{
    Properties
    {
        _Dim("Dim", Vector) = (0,0,0,0)
        _Color("Color", Color) = (1,0,0,1)
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

            float4 _Dim;
            fixed4 _Color;
            float _XMin;
            float _XMax;
            float _YMin;
            float _YMax;
            int _IsDesc;
            float sqr3 = sqrt(3);

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
                vOffSet = _YMax - _YMin;

                return _YMin + vOffSet * sqrt(1 - (sqr3 * (x - _XMin) / (_XMax - _XMin)) * (sqr3 * (x - _XMin) / (_XMax - _XMin)));

               /* if (_IsDesc == 1)
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
                }*/
            }

            float IsPointInsideFuntion(float2 pt, float transitionHalfWidth = .005)
            {
                return smoothstep(pt.y - transitionHalfWidth, pt.y + transitionHalfWidth, computeElliptic(pt.x));
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
                return _Color * IsPointInsideFuntion(localToGlobalUVInRect(i.uv));
            }
            ENDCG
        }
    }
}
