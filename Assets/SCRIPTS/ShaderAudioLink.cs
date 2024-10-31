using UnityEngine;

public class ShaderAudioLink : MonoBehaviour
{
    public Material reactiveMaterial;
    public AudioSpectrum audioSpectrum;

    private float bassAmplitude;
    private float midAmplitude;
    private float highAmplitude;

    void Update()
    {
        if (audioSpectrum == null || AudioSpectrum.spectrum == null)
            return;

        // Calculate amplitudes for each band
        bassAmplitude = CalculateAmplitude(0, 64);
        midAmplitude = CalculateAmplitude(65, 256);
        highAmplitude = CalculateAmplitude(257, 511);

        // Apply scaling and clamping
        bassAmplitude = Mathf.Clamp01(bassAmplitude * 10f);
        midAmplitude = Mathf.Clamp01(midAmplitude * 20f);
        highAmplitude = Mathf.Clamp01(highAmplitude * 50f);

        // Pass the amplitudes to the shader
        reactiveMaterial.SetFloat("_BassAmplitude", bassAmplitude);
        reactiveMaterial.SetFloat("_MidAmplitude", midAmplitude);
        reactiveMaterial.SetFloat("_HighAmplitude", highAmplitude);
    }

    float CalculateAmplitude(int minIndex, int maxIndex)
    {
        float sum = 0f;
        for (int i = minIndex; i <= maxIndex; i++)
        {
            sum += AudioSpectrum.spectrum[i];
        }
        return sum / (maxIndex - minIndex + 1);
    }
}

