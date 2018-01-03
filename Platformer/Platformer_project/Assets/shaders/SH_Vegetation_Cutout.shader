// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Vegetation/Cutout" {
	Properties {
		_Color ("Color (RGB)", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		_Cutoff("Alpha cutoff (F)", Range(0,1)) = 0.5
		_Metallic("Metallic (RGBA)", 2D) = "white" {}
		_BumpMap("Bumpmap (RGB)", 2D) = "bump" {}

		_WindForce("Wind Strength (F)", Float) = 0.25
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
			LOD 200

		CGPROGRAM
		// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
		#pragma exclude_renderers gles
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		uniform float4 _Wind;
		uniform float4 _Obstacle;

		sampler2D _MainTex;
		sampler2D _Metallic;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float3 color : COLOR;
		};
		
		fixed4 _Color;
		float _WindForce;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v) {

			const float4 _waveXSize = float4(.015, .02, -.06, .05);
			const float4 _waveZSize = float4 (.06, .02, -.02, .01);
			const float4 waveSpeed = float4 (.3, .3, .1, .1);

			float4 _waveXmove = _waveXSize * waveSpeed * _WindForce * 25;
			float4 _waveZmove = _waveZSize * waveSpeed * _WindForce * 25;

			// We model the wind as basic waves...

			// Calculate the wind input to leaves from their vertex positions....
			// for now, we transform them into world-space x/z positions...
			// Later on, we should actually be able to do the whole calc's in post-projective space

			float3 worldPos = mul((float3x4)unity_ObjectToWorld, v.vertex);
			
			// This is the input to the sinusiodal warp

			float4 waves;
			waves = worldPos.x * _waveXSize;
			waves += worldPos.z * _waveZSize;

			// Add in time to model them over time
			waves += _Time[2] * waveSpeed;

			waves = sin(waves);
			float4 s = sin(waves);
			float4 c = cos(waves);

			float waveAmount = v.color.r;
			s *= waveAmount;

			// Faste winds move the grass more than slow winds
			float fade = dot(s, 1.3);

			float3 waveMove = float3 (0, 0, 0);
			waveMove.x = dot(s, _waveXmove);
			waveMove.z = dot(s, _waveZmove);

			v.vertex.xyz -= mul((float3x3)unity_WorldToObject, waveMove + _Wind * v.color.r * v.color.r).xyz ;
			
			// OBSTACLE AVOIDANCE CALC
			float3 bendDir = normalize(worldPos.xyz - _Obstacle.xyz);//direction of obstacle bend
			float distLimit = 2;// how far away does obstacle reach
			float distMulti = (distLimit - min(distLimit, distance(worldPos.xyz, _Obstacle.xyz))) / distLimit;//distance falloff
			//OBSTACLE AVOIDANCE END

			v.vertex.xz += bendDir.xz * distMulti * v.color.r * v.color.r * 1; //ADD OBSTACLE BENDING
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			// Metallic and smoothness come from slider variables
			o.Metallic = tex2D(_Metallic, IN.uv_MainTex).r;
			o.Smoothness = tex2D(_Metallic, IN.uv_MainTex).a;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Transparent/Cutout/Diffuse"
}
