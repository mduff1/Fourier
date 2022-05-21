Shader "Custom/Mandelbrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area("Area", vector) = (0, 0, 4, 4)
        _MaxIt("MaxIt", range(1,5000)) = 500
        _Angle("Angle", range(-3.1415, 3.1415)) = 0
        _Color("Color", range(0,1)) = 1.0
        _Repeat("Repeat", float) = 1
        _Radius("Radius", range(0.1,1000.0)) = 10.0
        _Speed("Speed", range(1.0,10.0)) = 0.1
        _FracSwitch("FracSwitch", range(0,2)) = 0
        
        //audio 
        _freqBand0("Freq Band 0", float) = 0
        _freqBand1("Freq Band 1", float) = 0
        _freqBand2("Freq Band 2", float) = 0
        _freqBand3("Freq Band 3", float) = 0
        _freqBand4("Freq Band 4", float) = 0
        _freqBand5("Freq Band 5", float) = 0
        _freqBand6("Freq Band 6", float) = 0
        _freqBand7("Freq Band 7", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _Speed;
            float4 _Area;
            float _Angle, _MaxIt, _Color, _Repeat, _Radius;
            float _freqBand0, _freqBand1, _freqBand2, _freqBand3, _freqBand4, _freqBand5, _freqBand6, _freqBand7;
            sampler2D _MainTex;

            float2 rot(float2 p, float2 pivot, float angle) {
                float s = sin(angle);
                float c = cos(angle);
                p -= pivot;
                p = float2(p.x*c-p.y*s, p.x*s+p.y*c);
                p += pivot;
                return p;
            }

            float calc(float2 z, float2 c, float r)
            {
                float zLast;
                for (float i = 0; i < _MaxIt; i++)
                {
                    zLast = z;
                    z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + c;

                    if (sqrt(dot(z, z)) > r)
                    {
                        return sqrt((i - log2(log2(dot(z, z))) + 5.0) / _MaxIt);
                    }
                }
                return 0.0f;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = _Area.xy + (i.uv - 0.5) * _Area.zw;
                float2 z;// = pos;
                float2 c = pos;//float2(cos(1.2 * _Time[3]*_Speed), sin(1.5 * _Time[3]*_Speed));
                c = rot(c, _Area.xy, _Angle);
                float m = calc(z, c, _Radius);



                float3 col;

                if (m == 0.0) {
                    col = float3(0, 0, 0);
                }
                else if (m > 0.0 && m < 0.2) {
                    //col = float3(cos((m) * 40.0 + 3.0) + _freqBand5/300, cos((m) * 30.0 + 2.8) + _freqBand5 / 300, cos((m) * 35.0 + 4.0) + _freqBand5 / 300);
                    col = tex2D(_MainTex, float2(m /*_Repeat * abs(sin(_Time[_Speed] + 0.1)) */ , _Color * _freqBand0));
                }
                else if (m > 0.2 && m < 0.3) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand1));
                }
                else if (m > 0.3 && m < 0.4) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand2));
                }
                else if (m > 0.4 && m < 0.5) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand3));
                }
                else if (m > 0.5 && m < 0.6) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand4));
                }
                else if (m > 0.6 && m < 0.7) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand5));
                }
                else if (m > 0.7 && m < 0.8) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand6));
                }
                else if (m >= 0.8) {
                    col = tex2D(_MainTex, float2(m/** _Repeat * abs(sin(_Time[_Speed] + 0.1))*/, _Color * _freqBand7));
                }
                return float4(col, 1.0f);
            }
            ENDCG
        }
    }
}
