using UnityEngine;
using System.Collections.Generic;

public class LightIntensityController : MonoBehaviour
{
    public AudioSpectrum audioSpectrum; // Reference to your AudioSpectrum script
    public List<Light> lights; // List of lights to control
    public float baseIntensity = 10f; // Initial base intensity of the lights
    public float intensityMultiplier = 5f; // Multiplier for high-frequency response
    public float smoothingSpeed = 5f; // Speed of intensity smoothing

    private float smoothedAmplitude = 0f; // Smoothed high-frequency amplitude

    void Start()
    {
        // Set the initial intensity for each light in the group
        foreach (Light light in lights)
        {
            light.intensity = baseIntensity;
        }
    }

    void Update()
    {
        // Get the high-frequency amplitude from AudioSpectrum and smooth it
        CalculateSmoothedHighFrequencyAmplitude();

        // Update the intensity of each light based on the smoothed amplitude
        UpdateLightIntensities();
    }

    // Calculates the smoothed high-frequency amplitude from AudioSpectrum
    void CalculateSmoothedHighFrequencyAmplitude()
    {
        float sum = 0f;
        int minFrequency = 257; // Start index for high frequencies
        int maxFrequency = 511; // End index for high frequencies

        for (int i = minFrequency; i <= maxFrequency; i++)
        {
            sum += AudioSpectrum.spectrum[i];
        }

        float averageAmplitude = sum / (maxFrequency - minFrequency + 1);

        // Smooth the amplitude using Lerp for stable visual effects
        smoothedAmplitude = Mathf.Lerp(smoothedAmplitude, averageAmplitude, Time.deltaTime * smoothingSpeed);
    }

    // Updates each light's intensity based on the smoothed high-frequency amplitude
    void UpdateLightIntensities()
    {
        foreach (Light light in lights)
        {
            light.intensity = baseIntensity + (smoothedAmplitude * intensityMultiplier);
        }
    }
}

