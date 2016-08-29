Shader "Jam/FlickerText"
{
	Properties
	{
		_MainTex ("Font Texture", 2D) = "white" {}
		_Color("Text Color", Color) = (1,1,1,1)
		_LineWidth("Scanline Width", Float) = 5
		_LineStrength("Scanline Strength", Range(0, 1)) = 0.5
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		Lighting Off Cull Off ZTest Always ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			float rand(float co)
			{
				return frac(sin(co * 12.9898) * 43758.5453);
			}

			struct appdata
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
				float4 screen_pos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float _LineWidth;
			float _LineStrength;
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;

				o.screen_pos = ComputeScreenPos(o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.color;
				col.a *= tex2D(_MainTex, i.uv).a;

				float scanline = saturate(sin(i.screen_pos.y * (3.14159 / (_LineWidth * 2)) ) * 2);

				col.a *= 1 - scanline * _LineStrength;

				float flicker = (rand(_Time.y) - 0.5) / 6 + 0.75;

				col.a *= flicker;
				
				return col;
			}
			ENDCG
		}
	}
}
