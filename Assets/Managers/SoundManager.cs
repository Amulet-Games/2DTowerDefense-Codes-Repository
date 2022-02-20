using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Clips.")]
        public AudioClip buildingPlaced;
        public AudioClip buildingDamaged;
        public AudioClip buildingDestroyed;
        public AudioClip enemyDie;
        public AudioClip enemyHit;
        public AudioClip gameOver;
        
        [Header("Config.")]
        public float def_sound_vol = 0.8f;
        public float adjustRange = 0.1f;

        [Header("Status.")]
        [ReadOnlyInspector] public float cur_sound_vol;

        [Header("Refs.")]
        [ReadOnlyInspector] public AudioSource _soundSource;

        #region Callbacks.
        public static SoundManager singleton;
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
        #endregion

        #region Play Sounds.
        public void PlaySound_BuildingPlaced()
        {
            _soundSource.PlayOneShot(buildingPlaced, cur_sound_vol);
        }

        public void PlaySound_BuildingDamaged()
        {
            _soundSource.PlayOneShot(buildingDamaged, cur_sound_vol);
        }

        public void PlaySound_BuildingDestroyed()
        {
            _soundSource.PlayOneShot(buildingDestroyed, cur_sound_vol);
        }

        public void PlaySound_EnemyDie()
        {
            _soundSource.PlayOneShot(enemyDie, cur_sound_vol);
        }

        public void PlaySound_EnemyHit()
        {
            _soundSource.PlayOneShot(enemyHit, cur_sound_vol);
        }

        public void PlaySound_GameOver()
        {
            _soundSource.PlayOneShot(gameOver, cur_sound_vol);
        }
        #endregion

        #region Adjust Volume.
        public void IncreaseSoundVolume()
        {
            cur_sound_vol += adjustRange;
            cur_sound_vol = cur_sound_vol > 1 ? 1 : cur_sound_vol;
        }

        public void DecreaseSoundVolume()
        {
            cur_sound_vol -= adjustRange;
            cur_sound_vol = cur_sound_vol < 0 ? 0 : cur_sound_vol;
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupGetRefs();
            SetupCurrentVolume();
        }

        void SetupGetRefs()
        {
            _soundSource = GetComponent<AudioSource>();
        }

        void SetupCurrentVolume()
        {
            SessionManager _sessionManager = SessionManager.singleton;

            SetVolumeAsDefault();
            
            void SetVolumeAsDefault()
            {
                cur_sound_vol = def_sound_vol;
                _soundSource.volume = cur_sound_vol;
            }
        }
        #endregion
    }
}