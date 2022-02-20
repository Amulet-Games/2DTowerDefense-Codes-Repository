using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SA
{
    public class CinemachineShake : MonoBehaviour
    {
        [Header("Status.")]
        [ReadOnlyInspector] public float timer;
        [ReadOnlyInspector] public float timerMax;
        [ReadOnlyInspector] public float startingIntensity;

        [Header("Refs.")]
        [ReadOnlyInspector] CinemachineVirtualCamera virtualCamera;
        [ReadOnlyInspector] CinemachineBasicMultiChannelPerlin cinemachineMultiChannelPerlin;

        public static CinemachineShake singleton;
        private void Awake()
        {
            #region Init Singleton.
            if (singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
            }
            #endregion

            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            if (timer < timerMax)
            {
                timer += Time.deltaTime;
                float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
                cinemachineMultiChannelPerlin.m_AmplitudeGain = amplitude;
            }
        }

        public void ShakeCamera(float intensity, float timerMax)
        {
            this.timerMax = timerMax;
            timer = 0f;
            startingIntensity = intensity;
            cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
    }
}