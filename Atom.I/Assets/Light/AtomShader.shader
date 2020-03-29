Shader "Custom/Simple Diffuse"
{
	Properties
	{
		_Littness("Litness", Range(0.1,1)) = 1
		_Color("Color", Color) = (255,0,0,0)
	}
		SubShader
		{
			Pass
			{
				Tags {"LightMode" = "ForwardBase"}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc" // for UnityObjectToWorldNormal
				#include "UnityLightingCommon.cginc" // for _LightColor0

				struct v2f
				{
					float2 uv : TEXCOORD0;
					fixed4 diff : COLOR0;
					float4 vertex : SV_POSITION;
				};

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

				float _Littness;
				float4 _Color;

				fixed4 frag(v2f i) : SV_Target
				{
					float4 col = _Color;
				
					col *= i.diff;

					if (_Littness != 0) {
               			col = col * _Littness;
            		}
					return col;
				}
			ENDCG
			}
		}
}