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
        _VOffset("VOffset", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            float4 _Dim;
            fixed4 _Color;
            float _XMin;
            float _XMax;
            float _YMin;
            float _YMax;
            float _IsDesc;
            float _VOffset;
            float s = sqrt(3);

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv, _MainTex;
                return o;
            }

            float computeElliptic(float x)
            {
/*                if (x < = (XMax - XMin) / 2f + XMin)
                {
                    
                }
                else
                {
                    
                }*/
                return 0.0f;
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
