Shader "Custom/ReactiveSphereShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _BassAmplitude ("Bass Amplitude", Range(0,1)) = 0
        _MidAmplitude ("Mid Amplitude", Range(0,1)) = 0
        _HighAmplitude ("High Amplitude", Range(0,1)) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _GradientTex ("Gradient Texture", 2D) = "white" {}
        _VortexStrength ("Vortex Strength", Range(0,1)) = 0.1 // New vortex strength property
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Front
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _GradientTex;
        fixed4 _Color;
        half _BassAmplitude;
        half _MidAmplitude;
        half _HighAmplitude;
        half _VortexStrength; // Vortex strength variable

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos : WORLD_POS;
            INTERNAL_DATA // Required for built-in functions like WorldNormalVector
        };

        // Vertex shader function for vertex displacement and vortex effect
        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.uv_MainTex = v.texcoord;
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

            // Vertex displacement using bass amplitude
            half displacement = saturate(_BassAmplitude) * 0.5;
            v.vertex.xyz += v.normal * displacement;

            // Apply vortex effect by distorting UV coordinates
            half vortexIntensity = _VortexStrength * saturate(_HighAmplitude); // Vortex strength modulated by HighAmplitude
            float angle = length(v.vertex.xy) * vortexIntensity; // Angle of twist based on distance from center
            float sinAngle = sin(angle);
            float cosAngle = cos(angle);
            
            // Rotate UV coordinates to create the twist
            float2 uvCenter = float2(0.5, 0.5); // Center of the twist
            float2 uvOffset = v.texcoord - uvCenter;
            v.texcoord.xy = uvCenter + float2(
                uvOffset.x * cosAngle - uvOffset.y * sinAngle,
                uvOffset.x * sinAngle + uvOffset.y * cosAngle
            );

            o.uv_MainTex = v.texcoord;
        }

        // Surface shader function for color and emission
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

            // Calculate time
            half time = _Time.y * 0.1;

            // Generate noise pattern
            half noise = frac(sin(dot(IN.worldPos.xyz * 0.1 + time, half3(12.9898,78.233,45.164))) * 43758.5453);

            // Modulate noise with amplitudes
            half amplitudeSum = saturate(_BassAmplitude + _MidAmplitude + _HighAmplitude);
            half noiseModulation = noise * amplitudeSum * 0.3;

            // Base color
            fixed3 baseColor = _Color.rgb * tex.rgb;

            // Modulated color using multiplication
            fixed3 modulatedColor = baseColor * (1.0 + noiseModulation);
            modulatedColor = saturate(modulatedColor);

            // Fresnel effect: Manually calculate the view direction
            half3 normalDir = normalize(WorldNormalVector(IN, o.Normal));
            half3 viewDir = normalize(_WorldSpaceCameraPos - IN.worldPos);
            half fresnel = pow(1.0 - dot(viewDir, normalDir), 3.0);
            half fresnelEffect = fresnel * saturate(_HighAmplitude) * 0.4;

            // Gradient color
            half gradientPosition = amplitudeSum * 0.33;
            gradientPosition = saturate(gradientPosition);
            fixed4 gradientColor = tex2D(_GradientTex, half2(gradientPosition, 0.0));

            // Apply gradient color
            modulatedColor *= gradientColor.rgb;

            // Final color
            o.Albedo = modulatedColor;

            // Emission
            half emissionIntensity = amplitudeSum * 0.4;
            o.Emission = modulatedColor * emissionIntensity;
            o.Emission += fresnelEffect;

            // Clamp emission
            o.Emission = saturate(o.Emission);
        }
        ENDCG
    }
    FallBack "Diffuse"
}




