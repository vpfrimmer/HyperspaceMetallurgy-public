// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Space Cloud"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_GradientTexture("Gradient Texture", 2D) = "white" {}
		_OffsetSpeed2("Offset Speed 2", Vector) = (0,0,0,0)
		_OffsetSpeed1("Offset Speed 1", Vector) = (0,0,0,0)
		[HDR]_GlowColor("Glow Color", Color) = (1,1,1,1)
		_Contrast("Contrast", Float) = 1
		_UVScale2("UV Scale 2", Float) = 1
		_UVScale1("UV Scale 1", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _GlowColor;
		uniform sampler2D _GradientTexture;
		uniform sampler2D _MainTexture;
		uniform float _UVScale1;
		uniform float2 _OffsetSpeed1;
		uniform float2 _OffsetSpeed2;
		uniform float _UVScale2;
		uniform float4 _MainTexture_ST;
		uniform float _Contrast;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner9 = ( _Time.y * ( _OffsetSpeed1 * float2( 0.01,0.01 ) ) + i.uv_texcoord);
			float4 tex2DNode1 = tex2D( _MainTexture, ( _UVScale1 * panner9 ) );
			float2 panner10 = ( _Time.y * ( _OffsetSpeed2 * float2( 0.01,0.01 ) ) + i.uv_texcoord);
			float4 tex2DNode3 = tex2D( _MainTexture, ( panner10 * _UVScale2 ) );
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 tex2DNode4 = tex2D( _MainTexture, uv_MainTexture );
			float2 temp_cast_0 = (( tex2DNode1.r * tex2DNode3.g * tex2DNode4.b * _Contrast )).xx;
			o.Emission = ( _GlowColor * tex2D( _GradientTexture, temp_cast_0 ) * _GlowColor.a ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
0;73;1231;629;2104.799;410.2797;1.76171;True;False
Node;AmplifyShaderEditor.Vector2Node;13;-1677.425,93.0414;Inherit;False;Property;_OffsetSpeed1;Offset Speed 1;4;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;14;-1669.625,226.9414;Inherit;False;Property;_OffsetSpeed2;Offset Speed 2;3;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;11;-1356.018,375.8946;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1382.499,229.2212;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.01,0.01;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1389.546,104.1397;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.01,0.01;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1382.018,-59.60543;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1037.203,394.8218;Inherit;False;Property;_UVScale2;UV Scale 2;7;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;10;-1067.417,235.4946;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;9;-1066.117,62.59444;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1044.25,-50.89088;Inherit;False;Property;_UVScale1;UV Scale 1;8;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-825.7976,12.53073;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-794.9816,-253.6146;Inherit;True;Property;_MainTexture;Main Texture;1;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-827.5591,227.4593;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-536.0432,189.1989;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-532.5,-19;Inherit;True;Property;_Tex0;Tex 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-536.0432,395.8989;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-158.1104,-89.64836;Inherit;False;Property;_Contrast;Contrast;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;14.74747,116.7755;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;256.9754,-163.8418;Inherit;False;Property;_GlowColor;Glow Color;5;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;315.137,130.1565;Inherit;True;Property;_GradientTexture;Gradient Texture;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-175.6013,339.088;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;626.7882,-8.90049;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;787.819,92.72305;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Space Cloud;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;14;0
WireConnection;19;0;13;0
WireConnection;10;0;12;0
WireConnection;10;2;21;0
WireConnection;10;1;11;0
WireConnection;9;0;12;0
WireConnection;9;2;19;0
WireConnection;9;1;11;0
WireConnection;22;0;23;0
WireConnection;22;1;9;0
WireConnection;25;0;10;0
WireConnection;25;1;24;0
WireConnection;3;0;2;0
WireConnection;3;1;25;0
WireConnection;1;0;2;0
WireConnection;1;1;22;0
WireConnection;4;0;2;0
WireConnection;7;0;1;1
WireConnection;7;1;3;2
WireConnection;7;2;4;3
WireConnection;7;3;18;0
WireConnection;6;1;7;0
WireConnection;8;0;1;1
WireConnection;8;1;3;2
WireConnection;8;2;4;3
WireConnection;16;0;15;0
WireConnection;16;1;6;0
WireConnection;16;2;15;4
WireConnection;0;2;16;0
ASEEND*/
//CHKSM=91672903B7E9BD7925AF218BBB19F86FD4B04C98