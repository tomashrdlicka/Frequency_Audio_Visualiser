using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
    [Range(64, 8192)]
    public int spectrumSize = 512; // Default length, adjustable in the Inspector

    public static float[] spectrum;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure spectrumSize is within the valid range and is a power of two
        spectrumSize = Mathf.Clamp(spectrumSize, 64, 8192);
        spectrumSize = Mathf.ClosestPowerOfTwo(spectrumSize);

        // Initialize the spectrum array with the validated size
        spectrum = new float[spectrumSize];
    }

    void Update()
    {
        if (spectrum == null || spectrum.Length != spectrumSize)
        {
            // Reinitialize the array if size has changed or it was not set
            spectrum = new float[spectrumSize];
        }

        // Fill the spectrum array with audio data
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }
}

