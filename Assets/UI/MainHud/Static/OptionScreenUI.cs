using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class OptionScreenUI : MonoBehaviour
    {
        [Header("Groups (Drops).")]
        public CanvasGroup screenGroup;
        public Canvas screenCavnas;

        [Header("Screen Tween.")]
        public LeanTweenType screenEaseType;
        public float screenFadeSpeed;
        
        [Header("Sound Vol Btns (Drops).")]
        public Button soundDescBtn;
        public Button soundInscBtn;

        [Header("Music Vol Btns (Drops).")]
        public Button musicDescBtn;
        public Button musicInscBtn;

        [Header("Text (Drops).")]
        public TMP_Text soundVolText;
        public TMP_Text musicVolText;

        [Header("Toggle (Drops).")]
        public Toggle edgeScrollingToggle;

        [Header("Camera Handler (Drops).")]
        public CameraHandler _camHandler;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isScreenShowing;

        [Header("Refs.")]
        [ReadOnlyInspector] public SoundManager _soundManager;
        [ReadOnlyInspector] public MusicManager _musicManager;
        [ReadOnlyInspector] public SessionManager _sessionManager;

        int screenTweenId;

        private void Start()
        {
            _soundManager = SoundManager.singleton;
            _musicManager = MusicManager.singleton;
            _sessionManager = SessionManager.singleton;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Escape"))
            {
                UIButton_ToggleScreen();
            }
        }

        #region On Screen Open.
        void OnScreenOpen()
        {
            OnScreenOpen_SetCurrentText();
            OnScreenOpen_SetCurrentToggle();
        }

        void OnScreenOpen_SetCurrentText()
        {
            Refresh_SoundVol_Text();
            Refresh_MusicVol_Text();
        }

        void OnScreenOpen_SetCurrentToggle()
        {
            Refresh_EdgeScrolling_Toggle();
        }
        #endregion

        #region On Screen Close.
        void OnScreenClose()
        {
            OnScreenClose_DisableCanvas();
        }

        void OnScreenClose_DisableCanvas()
        {
            DisableCanvas();
        }
        #endregion

        #region Show / Hide.
        public void ShowScreen()
        {
            OnScreenOpen();

            EnableCanvas();
            screenTweenId = LeanTween.alphaCanvas(screenGroup, 1, screenFadeSpeed).setOnComplete(SetTimeScaleToZero).id;
        }

        public void HideScreen()
        {
            CancelScreenTween();
            SetTimeScaleToOne();

            screenTweenId = LeanTween.alphaCanvas(screenGroup, 0, screenFadeSpeed).setOnComplete(OnScreenClose).id;
        }

        void EnableCanvas()
        {
            screenCavnas.enabled = true;
        }

        void DisableCanvas()
        {
            screenCavnas.enabled = false;
        }
        
        void CancelScreenTween()
        {
            if (LeanTween.isTweening(screenTweenId))
                LeanTween.cancel(screenTweenId);
        }
        #endregion

        #region Time Scale.
        void SetTimeScaleToZero()
        {
            Time.timeScale = 0;
        }

        void SetTimeScaleToOne()
        {
            Time.timeScale = 1;
        }
        #endregion

        #region Refresh Text.
        void Refresh_SoundVol_Text()
        {
            soundVolText.text = (_soundManager.cur_sound_vol * 10).ToString("F0");
        }

        void Refresh_MusicVol_Text()
        {
            musicVolText.text = _musicManager.GetCurrentVolume().ToString("F0");
        }
        #endregion

        #region Buttons.
        public void UIButton_ToggleScreen()
        {
            isScreenShowing = !isScreenShowing;
            if (isScreenShowing)
            {
                ShowScreen();
            }
            else
            {
                HideScreen();
            }
        }

        public void UIButton_SoundDecrease()
        {
            _soundManager.DecreaseSoundVolume();

            Refresh_SoundVol_Text();
        }

        public void UIButton_SoundIncrease()
        {
            _soundManager.IncreaseSoundVolume();

            Refresh_SoundVol_Text();
        }

        public void UIButton_MusicDecrease()
        {
            _musicManager.DecreaseMusicVolume();

            Refresh_MusicVol_Text();
        }

        public void UIButton_MusicIncrease()
        {
            _musicManager.IncreaseMusicVolume();

            Refresh_MusicVol_Text();
        }

        public void UIButton_MainMenu()
        {
            SetTimeScaleToOne();
            SaveStateToSave();
            _sessionManager.LoadScene_ReturnMainMenu();
        }
        #endregion

        #region Toggles.
        public void UIToggle_SetEdgeScolling()
        {
            _camHandler.SetEdgeScrolling();
        }

        void Refresh_EdgeScrolling_Toggle()
        {
            if (_sessionManager._hasExitFromGame)
            {
                edgeScrollingToggle.isOn = _camHandler._isEdgeScrollingEnabled;
            }
        }
        #endregion
        
        #region Save / Load.
        public void SaveStateToSave()
        {
            SaveProfile _saveProfile = _sessionManager.saveProfile;
            _saveProfile._isEdgeScrolling = edgeScrollingToggle.isOn;

            _sessionManager._hasExitFromGame = true;
        }
        #endregion
    }
}