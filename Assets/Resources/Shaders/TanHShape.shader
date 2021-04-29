Shader "Custom/TanHShape"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _ColorEdge ("ColorEdge", Color) = (1,0,0,1)
        _Edge ("Edge", float) = 1.0
        _Smoothness ("Smoothness", float) = 0.05
        _Height ("Height", float) = 0
        _Width ("Width", float) = 0
        //_A ("A", float) = 0 // //A is same as K for rect on origine
        _B ("B", float) = 0
        _Kp ("Kp", float) = 0
        _K ("K", float) = 0
        _Alpha ("Alpha", float) = 0
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

            fixed4 _ColorEdge;
            fixed4 _Color;
            float _Edge;
            float _Smoothness;
            float _Height;
            float _Width;
            //float _A; //A is same as K for rect on origine
            float _B;
            float _Kp;
            float _K;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
                        
            float2 localToGlobalUVInRect(fixed2 uv)
            {
                return float2(uv.x * _Width, uv.y * _Height);
            }

            fixed4 ComputeCurveColor(fixed2 pt)
            {
                float exponential =  exp(_Alpha * _K * (pt.x - _B));
                float y = _K + _Kp * (1 - exponential) / (1 + exponential);
                
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
