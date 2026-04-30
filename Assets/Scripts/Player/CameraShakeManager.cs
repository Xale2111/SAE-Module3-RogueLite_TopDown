using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    [SerializeField] CinemachineBasicMultiChannelPerlin m_CinemachineNoise;
    [SerializeField] float m_ShakeDuration = 0.3f;
    [SerializeField] float m_ShakeMagnitudePlayerHit = 5f;
    [SerializeField] float m_ShakeMagnitudeEnemyhit = 3f;
    
    [SerializeField] AnimationCurve m_Curve;

    private float _timer;
    [SerializeField] private float amplitudeMultiplier = 0f;
    private void Start()
    {
        m_CinemachineNoise.FrequencyGain = 0;       
    }

    private void Update()
    {
        if (_timer <= m_ShakeDuration)
        {
            _timer += Time.deltaTime;
            Debug.Log("Curve : " + m_Curve.Evaluate(NormalizeAnimationDuration(_timer)));
            m_CinemachineNoise.AmplitudeGain = m_Curve.Evaluate(NormalizeAnimationDuration(_timer))*amplitudeMultiplier; 
        }
        else
        {
            m_CinemachineNoise.AmplitudeGain = 0;
        }
    }

    private float NormalizeAnimationDuration(float timer)
    {
        float normalizedDuration = timer / m_ShakeDuration;
        normalizedDuration = Mathf.Clamp01(normalizedDuration);
        
        return normalizedDuration;
    }

    public void ShakeCameraPlayerHit()
    {
        _timer = 0;
        m_CinemachineNoise.FrequencyGain = m_ShakeMagnitudePlayerHit;
    }

    public void ShakeCameraEnemyHit()
    {
        _timer = 0;
        m_CinemachineNoise.FrequencyGain = m_ShakeMagnitudeEnemyhit;
    }

}
