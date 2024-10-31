Shader "Custom/HighReactiveParticleShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Float) = 0

        // Add gradient texture property
        _GradientTex ("Gradient Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade vertex:vert

        sampler2D _MainTex;
        sampler2D _GradientTex;
        fixed4 _Color;
        float _Amplitude;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float particleAge : TEXCOORD1; // Custom TEXCOORD channel for particle age
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.particleAge = v.texcoord1.x; // Assuming particle age is passed in texcoord1.x

            // Optional: Add vertex displacement based on amplitude
            float displacement = _Amplitude * 0.1;
            v.vertex.xyz += v.normal * displacement;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // Sample the gradient texture using the particle's normalized age
            float age = saturate(IN.particleAge); // Ensure age is between 0 and 1
            fixed4 gradientColor = tex2D(_GradientTex, float2(age, 0.5));

            // Apply the gradient color
            c.rgb = gradientColor.rgb;

            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // Optional: Add emission based on amplitude
            o.Emission = c.rgb * _Amplitude * 2.0;
        }
        ENDCG
    }
    FallBack "Particles/Alpha Blended"
}

