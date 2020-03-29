Shader "Custom/Test" {
   Properties {
        _Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
        _MainTex ("Texture to blend", 2D) = "black" {}
        _Flash ("Flash", Range(0,1)) = 0
        _FlashColor ("Flash color", Color) = (1,1,1,1)
   }
   SubShader {
        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        Lighting Off

    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        struct v2f
        {
            float2 uv : TEXCOORD0;
            fixed4 diff : COLOR0;
            float4 vertex : SV_POSITION;
        };

        #include "UnityCG.cginc"
        #include "UnityLightingCommon.cginc"
        v2f vert(appdata_base v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            
            o.diff = nl * _LightColor0;
            return o;
        }
        uniform sampler2D _MainTex;
        uniform float _Flash;
        uniform float4 _FlashColor;

         float4 frag(v2f i) : COLOR {
            float4 t = tex2D(_MainTex, i.uv);
            if (_Flash != 0) {
               t.r = t.a;
               t.g = t.a;
               t.b = t.a;
               t = t * _FlashColor;
            }
            return t;
         }
         ENDCG
      }
   }
}
