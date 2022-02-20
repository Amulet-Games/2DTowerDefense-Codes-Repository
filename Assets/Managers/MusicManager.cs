using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SA
{
    public class MusicManager : MonoBehaviour
    {
        [Header("Audio Mixer.")]
        public AudioMixer mixerGroup_Master;
        public string masterVolumePara = "MixerGroup_Master";

        [Header("Audio Sources.")]
        public AudioSource mainMenu_MusicSource;
        public AudioSource gameLevel_MusicSource;

        [Header("Config.")]
        public float def_music_vol = 0f;
        public float max_music_vol = 20f;
        public float min_music_vol = -80f;
        public float adjustRange = 10f;

        [Header("Status.")]
        [ReadOnlyInspector, Range(-80, 20)] public float master_group_vol;
        
        #region Callbacks.
        public static MusicManager singleton;
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

        #region Adjust Volume.
        public void IncreaseMusicVolume()
        {
            master_group_vol += adjustRange;
            master_group_vol = master_group_vol > max_music_vol ? max_music_vol : master_group_vol;

            mixerGroup_Master.SetFloat(masterVolumePara, master_group_vol);
        }

        public void DecreaseMusicVolume()
        {
            master_group_vol -= adjustRange;
            master_group_vol = master_group_vol < min_music_vol ? min_music_vol : master_group_vol;

            mixerGroup_Master.SetFloat(masterVolumePara, master_group_vol);
        }
        #endregion

        #region On Scene Loaded.
        public void OnGameSceneLoaded()
        {
            ChangeAudio_GameLevel();
        }

        public void OnMainMenuSceneLoaded()
        {
            ChangeAudio_MainMenu();
        }
        #endregion

        #region Change Audio.
        void ChangeAudio_MainMenu()
        {
            mainMenu_MusicSource.enabled = true;
            gameLevel_MusicSource.enabled = false;
        }

        void ChangeAudio_GameLevel()
        {
            mainMenu_MusicSource.enabled = false;
            gameLevel_MusicSource.enabled = true;
        }
        #endregion

        #region Fade In Music.
        void FadeInMusicVolume()
        {
            LeanTween.value(min_music_vol, master_group_vol, 1f).setEaseOutCirc().setOnUpdate((value) =>
            {
                mixerGroup_Master.SetFloat(masterVolumePara, value);
            });
        }
        #endregion

        #region Map Current Volume Value to (0 - 100).
        public float GetCurrentVolume()
        {
            return (master_group_vol - min_music_vol) / (max_music_vol - min_music_vol) * 10;
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupCurrentVolume();
        }
        
        void SetupCurrentVolume()
        {
            SessionManager _sessionManager = SessionManager.singleton;

            SetVolumeAsDefault();

            void SetVolumeAsDefault()
            {
                master_group_vol = def_music_vol;
                FadeInMusicVolume();
            }
        }
        #endregion
    }
}
