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
    private void Start()
    {
        m_CinemachineNoise.FrequencyGain = 0;       
    }

    public void ShakeCameraPlayerHit()
    {
        StartCoroutine(ShakeCameraPlayerHitCoroutine());
    }

    public void ShakeCameraEnemyHit()
    {
        StartCoroutine(ShakeCameraEnemyHitCoroutine());
    }
    
    private IEnumerator ShakeCameraPlayerHitCoroutine()
    {
        m_CinemachineNoise.FrequencyGain = m_ShakeMagnitudePlayerHit;
        yield return new WaitForSeconds(m_ShakeDuration);
        m_CinemachineNoise.FrequencyGain = 0;       
    }
    
    private IEnumerator ShakeCameraEnemyHitCoroutine()
    {
        m_CinemachineNoise.FrequencyGain = m_ShakeMagnitudeEnemyhit;
        yield return new WaitForSeconds(m_ShakeDuration);
        m_CinemachineNoise.FrequencyGain = 0;       
    }
    
    

}
