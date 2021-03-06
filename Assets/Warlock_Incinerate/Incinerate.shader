﻿Shader "Suntabu/Incinerate"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex("Mask",2D) = "white"{}
		_MaskTex2("Mask2",2D) = "white"{}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100 Cull off
		Blend SrcAlpha OneMinusSrcAlpha
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
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaskTex;
			float4 _MaskTex_ST;
			sampler2D _MaskTex2;
			float4 _MaskTex2_ST;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex) + _Time.xy;
				o.uv2 = TRANSFORM_TEX(v.uv, _MaskTex) - _Time.xy;
				o.uv3 = TRANSFORM_TEX(v.uv, _MaskTex2);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mask = tex2D(_MaskTex,i.uv2);
				fixed4 mask2 = tex2D(_MaskTex2,i.uv3);
				return fixed4(col.rgb,mask.a) *mask2.a * 1.3;
			}
			ENDCG
		}
	}
}
