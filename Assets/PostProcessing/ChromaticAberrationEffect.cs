using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SA
{
    public class ChromaticAberrationEffect : MonoBehaviour
    {
        [Header("Config.")]
        public float decreaseSpeed = 1f;

        [Header("Refs.")]
        public Volume volume;

        public static ChromaticAberrationEffect singleton;
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
        }

        private void Update()
        {
            if (volume.weight > 0)
            {
                volume.weight -= Time.deltaTime * decreaseSpeed;
            }
        }

        public void SetWeight(float weight)
        {
            volume.weight = weight;
        }
    }
}