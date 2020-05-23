Shader "Ciconia Studio/Custom/B&W 2D Forest/Fog animated"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_CloudTex("Cloud Tex", 2D) = "white" {}
		_Tiling("Tiling", Float) = 1
		_Smooth("Smooth", Range( 0 , 8)) = 0
		_DistortionStrength("Distortion Strength", Range( 0 , 1)) = 0.05
		_Speed("Speed", Float) = 0.05
		_Intensity("Intensity", Float) = 1
		_Flickeringspeed("Flickering speed", Float) = 0
		_Min("Min", Float) = 0.5
		_Max("Max", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float _Intensity;
			uniform sampler2D _CloudTex;
			uniform float _Speed;
			uniform float _Tiling;
			uniform float _Smooth;
			uniform float _DistortionStrength;
			uniform float4 _AlphaTex_ST;
			uniform float _Max;
			uniform float _Min;
			uniform float _Flickeringspeed;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv65 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_cast_1 = (_Speed).xx;
				float2 temp_cast_2 = (_Tiling).xx;
				float2 uv59 = IN.texcoord.xy * temp_cast_2 + float2( 0,0 );
				float2 panner62 = ( uv59 + 1 * _Time.y * temp_cast_1);
				float lerpResult58 = lerp( 1 , 10 , _DistortionStrength);
				float4 lerpResult68 = lerp( float4( uv65, 0.0 , 0.0 ) , tex2Dlod( _CloudTex, float4( panner62, 0, _Smooth) ) , ( ( 1.0 - lerpResult58 ) / 10.0 ));
				float4 tex2DNode2 = tex2D( _MainTex, lerpResult68.rg );
				float2 uv_AlphaTex = IN.texcoord.xy * _AlphaTex_ST.xy + _AlphaTex_ST.zw;
				#ifdef _KEYWORD0_ON
				float staticSwitch9 = tex2D( _AlphaTex, uv_AlphaTex ).a;
				#else
				float staticSwitch9 = tex2DNode2.a;
				#endif
				float4 appendResult6 = (float4(( _Intensity * (tex2DNode2).rgb ) , staticSwitch9));
				float4 appendResult25 = (float4(( appendResult6 * IN.color * _Color ).xyz , tex2DNode2.a));
				float mulTime74 = _Time.y * 1;
				float lerpResult79 = lerp( _Max , _Min , sin( ( mulTime74 * _Flickeringspeed ) ));
				
				fixed4 c = ( appendResult25 * lerpResult79 );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
