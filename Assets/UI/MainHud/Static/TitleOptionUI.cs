using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class TitleOptionUI : MonoBehaviour
    {
        [Header("Groups (Drops).")]
        public CanvasGroup optionGroup;
        public Canvas optionCavnas;

        [Header("Option Tween.")]
        public LeanTweenType optionEaseType;
        public float optionFadeSpeed;

        [Header("Sound Vol Btns (Drops).")]
        public Button soundDescBtn;
        public Button soundInscBtn;

        [Header("Music Vol Btns (Drops).")]
        public Button musicDescBtn;
        public Button musicInscBtn;

        [Header("Text (Drops).")]
        public TMP_Text soundVolText;
        public TMP_Text musicVolText;

        [Header("MainMenuManager (Drops).")]
        public MainMenuManager mainMenuManager;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isOptionShowing;

        [Header("Refs.")]
        [ReadOnlyInspector] public SoundManager _soundManager;
        [ReadOnlyInspector] public MusicManager _musicManager;

        int screenTweenId;

        private void Start()
        {
            _soundManager = SoundManager.singleton;
            _musicManager = MusicManager.singleton;
        }

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

        #region On Option Open.
        void OnOptionOpen()
        {
            OnOptionOpen_SetCurrentText();
            DiableMainMenuBtnGroup();
        }

        void OnOptionOpen_SetCurrentText()
        {
            Refresh_SoundVol_Text();
            Refresh_MusicVol_Text();
        }

        void DiableMainMenuBtnGroup()
        {
            mainMenuManager.DisableBtnsGroupInteractable();
        }
        #endregion

        #region On Option Close.
        void OnOptionClose()
        {
            OnOptionClose_DisableCanvas();
            EnableMainMenuBtnGroup();
        }

        void OnOptionClose_DisableCanvas()
        {
            DisableCanvas();
        }

        void EnableMainMenuBtnGroup()
        {
            mainMenuManager.EnableBtnsGroupInteractable();
        }
        #endregion

        #region Show / Hide.
        public void ShowOption()
        {
            OnOptionOpen();

            EnableCanvas();
            screenTweenId = LeanTween.alphaCanvas(optionGroup, 1, optionFadeSpeed).id;
        }

        public void HideOption()
        {
            CancelScreenTween();

            screenTweenId = LeanTween.alphaCanvas(optionGroup, 0, optionFadeSpeed).setOnComplete(OnOptionClose).id;
        }

        void EnableCanvas()
        {
            optionCavnas.enabled = true;
        }

        void DisableCanvas()
        {
            optionCavnas.enabled = false;
        }

        void CancelScreenTween()
        {
            if (LeanTween.isTweening(screenTweenId))
                LeanTween.cancel(screenTweenId);
        }
        #endregion

        #region Buttons.
        public void UIButton_ToggleScreen()
        {
            isOptionShowing = !isOptionShowing;
            if (isOptionShowing)
            {
                ShowOption();
            }
            else
            {
                HideOption();
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
        #endregion
    }
}