Shader "Custom/UnlitGlow"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _GlowIntensity ("Glow Intensity", Range(0,5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _GlowIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, i.uv);
                fixed4 col = _Color * texcol;
                col.a *= _GlowIntensity;
                return col;
            }
            ENDCG
        }
    }
}
