using UnityEngine;
using System.Collections; // Include this if you're using Coroutines

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAudioVisualizer : MonoBehaviour
{
    public AudioSpectrum audioSpectrum;

    public enum FrequencyRange { Bass, Mid, High }
    public FrequencyRange frequencyRange;

    public float emissionMultiplier;
    public float smoothingSpeed = 5f;

    private int minFrequency;
    private int maxFrequency;

    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.TrailModule trailModule;
    private Material particleMaterial;

    public float smoothedAmplitude = 0f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
        trailModule = ps.trails; // Initialize the Trails module

        // Get the material from the Particle System Renderer
        ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
        particleMaterial = renderer.material;

        // Optional: Make the particles face the player
        Transform playerTransform = Camera.main.transform;
        transform.LookAt(playerTransform.position);

        SetFrequencyRange();
        SetEmissionMultiplier();

        // Enable the Trails module
        trailModule.enabled = true;

        // Ensure the trail inherits the particle color
        trailModule.colorOverTrail = new ParticleSystem.MinMaxGradient(Color.white);
        trailModule.colorOverLifetime = new ParticleSystem.MinMaxGradient(Color.white);
        trailModule.dieWithParticles = false;
    }

    void SetFrequencyRange()
    {
        switch (frequencyRange)
        {
            case FrequencyRange.Bass:
                minFrequency = 0;
                maxFrequency = 64;
                break;
            case FrequencyRange.Mid:
                minFrequency = 65;
                maxFrequency = 256;
                break;
            case FrequencyRange.High:
                minFrequency = 257;
                maxFrequency = 511;
                break;
        }
    }

    void SetEmissionMultiplier()
    {
        switch (frequencyRange)
        {
            case FrequencyRange.Bass:
                emissionMultiplier = 5000f;
                break;
            case FrequencyRange.Mid:
                emissionMultiplier = 15000f;
                break;
            case FrequencyRange.High:
                emissionMultiplier = 100000f;
                break;
        }
    }

    void Update()
    {
        if (audioSpectrum == null || AudioSpectrum.spectrum == null)
            return;

        float sum = 0f;
        for (int i = minFrequency; i <= maxFrequency; i++)
        {
            sum += AudioSpectrum.spectrum[i];
        }
        float average = sum / (maxFrequency - minFrequency + 1);

        // Apply scaling factor if needed
        average *= GetScalingFactor();

        // Smooth the amplitude
        smoothedAmplitude = Mathf.Lerp(smoothedAmplitude, average, Time.deltaTime * smoothingSpeed);

        // Adjust the emission rate
        emissionModule.rateOverTime = smoothedAmplitude * emissionMultiplier;

        // Adjust particle properties
        AdjustParticleProperties(smoothedAmplitude);

        // Pass the amplitude to the shader (if your shader uses _Amplitude)
        if (particleMaterial.HasProperty("_Amplitude"))
        {
            particleMaterial.SetFloat("_Amplitude", smoothedAmplitude);
        }
    }

    float GetScalingFactor()
    {
        switch (frequencyRange)
        {
            case FrequencyRange.Bass:
                return 1f;
            case FrequencyRange.Mid:
                return 2f;
            case FrequencyRange.High:
                return 5f;
            default:
                return 1f;
        }
    }

    void AdjustParticleProperties(float amplitude)
    {
        var mainModule = ps.main;
        float size;
        float trailLifetime;
        float trailWidth;

        // Define colors for each frequency range
        Color startColor;
        Color endColor;

        switch (frequencyRange)
        {
            case FrequencyRange.Bass:
                size = Mathf.Lerp(0.2f, 0.5f, amplitude * 10f);
                trailLifetime = Mathf.Lerp(1.0f, 2.0f, amplitude * 10f);
                trailWidth = Mathf.Lerp(1.0f, 1.8f, amplitude * 10f);
                startColor = Color.red;
                break;
            case FrequencyRange.Mid:
                size = Mathf.Lerp(0.1f, 0.4f, amplitude * 10f);
                trailLifetime = Mathf.Lerp(0.8f, 1.6f, amplitude * 10f);
                trailWidth = Mathf.Lerp(1.0f, 1.4f, amplitude * 10f);
                startColor = Color.green;
                break;
            case FrequencyRange.High:
                size = Mathf.Lerp(0.1f, 0.4f, amplitude * 10f);
                trailLifetime = Mathf.Lerp(0.5f, 0.1f, amplitude * 10f);
                trailWidth = Mathf.Lerp(0.5f, 0.1f, amplitude * 10f);
                startColor = Color.blue;
                break;
            default:
                size = 0.1f;
                trailLifetime = 0.1f;
                trailWidth = 0.05f;
                startColor = Color.white;
                break;
        }

        // Update particle size and color
        mainModule.startSize = size;
        mainModule.startColor = startColor;

        // Adjust trail lifetime
        trailModule.lifetime = trailLifetime;

        // Adjust trail width over trail
        AnimationCurve widthCurve = new AnimationCurve();
        widthCurve.AddKey(0.0f, trailWidth);
        widthCurve.AddKey(1.0f, 0.0f);
        trailModule.widthOverTrail = new ParticleSystem.MinMaxCurve(1.0f, widthCurve);

        // Adjust trail color
        Gradient trailGradient = new Gradient();
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        trailGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        trailModule.colorOverLifetime = new ParticleSystem.MinMaxGradient(trailGradient);
    }
}





