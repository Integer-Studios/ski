﻿Shader "Custom/Toon"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _LightColor("Light Color", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _CelShadingLevels ("Cel Shading Levels", Range(0,15)) = 5
        _MaxColor ("Highlight Color", Range(0,1)) = 0.9
        _MinColor ("Shadow Color", Range(0,1)) = 0.1
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline width", Range (0.0, 0.03)) = .003
        [Toggle(USELIGHTCOLOR)] _UseLightColor ("Use Environment Light Color", Float) = 0
    }


    SubShader
    {
        Tags {"LightMode"="ForwardBase""RenderType"="Opaque"}
        LOD 200
        Pass
        {
            Name "BASE"
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma shader_feature USELIGHTCOLOR
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float4 _LightColor;
            float _CelShadingLevels;
            float _OutlineWidth;
            float _MinColor;
            float _MaxColor;
            float _UseLightColor;

            struct v2fBase {
                float2 uv : TEXCOORD0;
                half3 worldNormal : TEXCOORD1;
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(2)
                LIGHTING_COORDS(3,4)
            };

            v2fBase vert (appdata_base v) {
                v2fBase o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o,o.pos);

                return o;
            }

            fixed4 frag (v2fBase i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                half NdotL = dot(i.worldNormal, _WorldSpaceLightPos0.xyz);
                NdotL = (NdotL + 1.0)/2.0;
                half cel = floor(NdotL * _CelShadingLevels) / (_CelShadingLevels - 0.5);
                //if (cel == 0.0) cel = _MinColor;
                //if (cel >= 1.0) cel = _MaxColor;
                if (cel == 0.0) cel = 0.0;
                if (cel >= 1.0) cel = 1.0;
                float scale = _MaxColor - _MinColor;
                cel *= scale;
                cel += _MinColor;
                col *= cel * _Color;
                col += _LightColor;
                #ifdef USELIGHTCOLOR
                col *= _LightColor0.rgba * _LightColor0.rgba;
                #endif
                float attenuation = LIGHT_ATTENUATION(i);
                col *= attenuation;
                UNITY_APPLY_FOG(i.fogCoord, col); 
                return col;
            }
            ENDCG
        }

    }
    Fallback "Diffuse"
}