using UnityEngine;

public class ReactiveSphereController : MonoBehaviour
{
    // Public references and settings
    public Material sphereMaterial; // Reference to the sphere's material
    public AudioSpectrum audioSpectrum; // Reference to your AudioSpectrum script

    public enum FrequencyRange { Bass, Mid, High }
    public FrequencyRange frequencyRange;

    public float rotationSpeed = 10f; // Rotation speed
    public float maxVortexStrength = 0.25f; // Maximum vortex strength for UV distortion
    public float colorShiftSpeed = 2f; // Speed of color shifting along the gradient texture
    public float smoothingSpeed = 5f; // Smoothing speed for amplitude

    private int minFrequency; // Min frequency band index
    private int maxFrequency; // Max frequency band index

    private float smoothedAmplitude; // Smoothed amplitude for stable visuals
    private float initialOffset; // Initial texture offset for the gradient shift

    void Start()
    {
        initialOffset = 0f; // Initialize the initial offset for texture color shifting
        SetFrequencyRange(); // Set the frequency range indices
    }

    void Update()
    {
        // Calculate the smoothed amplitude based on the specified frequency range
        CalculateSmoothedAmplitude();

        // 1. Rotate the sphere
        RotateSphere();

        // 2. Apply Vortex Effect
        ApplyVortexEffect();

        // 3. Shift color along the gradient texture based on audio amplitude
        ShiftColor();
    }

    void SetFrequencyRange()
    {
        // Define the frequency range for Bass, Mid, and High frequencies
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

    // Calculate the smoothed amplitude based on the audio spectrum data within the specified frequency range
    void CalculateSmoothedAmplitude()
    {
        float sum = 0f;
        for (int i = minFrequency; i <= maxFrequency; i++)
        {
            sum += AudioSpectrum.spectrum[i];
        }
        float averageAmplitude = sum / (maxFrequency - minFrequency + 1);

        // Smooth the amplitude value for better visuals
        smoothedAmplitude = Mathf.Lerp(smoothedAmplitude, averageAmplitude, Time.deltaTime * smoothingSpeed);
    }

    // Rotate the sphere around the Y-axis
    void RotateSphere()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    // Apply Vortex Effect by adjusting the UV distortion based on smoothed amplitude
    void ApplyVortexEffect()
    {
        if (sphereMaterial.HasProperty("_VortexStrength"))
        {
            // Calculate vortex strength based on smoothed amplitude
            float vortexStrength = smoothedAmplitude * maxVortexStrength;

            // Set the vortex strength in the shader
            sphereMaterial.SetFloat("_VortexStrength", vortexStrength);
        }
    }

    // Shift color along the gradient texture based on smoothed amplitude
    void ShiftColor()
    {
        if (sphereMaterial.HasProperty("_MainTex"))
        {
            // Use smoothed amplitude to shift color along the gradient texture
            float currentOffset = initialOffset + smoothedAmplitude * colorShiftSpeed;

            // Offset texture horizontally (assuming the gradient is mapped horizontally)
            sphereMaterial.SetTextureOffset("_MainTex", new Vector2(currentOffset, currentOffset));
        }
    }
}
