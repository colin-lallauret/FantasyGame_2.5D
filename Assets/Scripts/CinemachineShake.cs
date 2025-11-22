using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance;

    private CinemachineCamera cmCam;
    private CinemachineBasicMultiChannelPerlin noise;

    private float defaultAmplitude;
    private float defaultFrequency;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cmCam = GetComponent<CinemachineCamera>();

        // Récupère le module de noise (nouvelle API Cinemachine 3)
        noise = cmCam.GetCinemachineComponent(CinemachineCore.Stage.Noise) 
                as CinemachineBasicMultiChannelPerlin;

        if (noise == null)
        {
            Debug.LogError("⚠️ Aucun module CinemachineBasicMultiChannelPerlin trouvé. Ajoute 'Noise' à ta CinemachineCamera !");
            return;
        }

        defaultAmplitude = noise.AmplitudeGain;
        defaultFrequency = noise.FrequencyGain;

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

    public void TriggerShake(float amplitude, float frequency, float duration)
    {
        StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
    }

    private IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
    {
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        noise.AmplitudeGain = defaultAmplitude;
        noise.FrequencyGain = defaultFrequency;
    }
}
