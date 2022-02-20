using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace SA
{
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Config.")]
        public float secondsPerDay = 20f;

        [Header("Refs (Drops).")]
        public Gradient gradient;
        public Light2D light2d;

        [Header("Status.")]
        [ReadOnlyInspector] public float dayTime;
        [ReadOnlyInspector] public float dayTimeSpeed;

        private void Start()
        {
            dayTimeSpeed = 1 / secondsPerDay;
        }

        private void Update()
        {
            dayTime += Time.deltaTime * dayTimeSpeed;
            light2d.color = gradient.Evaluate(dayTime % 1f);
        }
    }
}