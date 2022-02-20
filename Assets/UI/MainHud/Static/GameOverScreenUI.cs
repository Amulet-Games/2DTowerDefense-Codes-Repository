using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class GameOverScreenUI : MonoBehaviour
    {
        [Header("Groups (Drops).")]
        public CanvasGroup screenGroup;
        public Canvas screenCavnas;

        [Header("Screen Tween.")]
        public LeanTweenType screenEaseType;
        public float screenFadeSpeed;

        [Header("Buttons (Drops).")]
        public Button retryBtn;
        public Button mainMenuBtn;

        [Header("Text (Drops).")]
        public TMP_Text resultText;

        int screenTweenId;

        public static GameOverScreenUI singleton;
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
        
        #region Show / Hide.
        public void ShowScreen()
        {
            SetGameOverResultText();

            EnableCanvas();
            screenTweenId = LeanTween.alphaCanvas(screenGroup, 1, screenFadeSpeed).id;
        }

        public void HideScreen()
        {
            CancelScreenTween();

            screenTweenId = LeanTween.alphaCanvas(screenGroup, 0, screenFadeSpeed).setOnComplete(DisableCanvas).id;
        }

        void EnableCanvas()
        {
            screenCavnas.enabled = true;
        }

        void DisableCanvas()
        {
            screenCavnas.enabled = true;
        }

        void CancelScreenTween()
        {
            if (LeanTween.isTweening(screenTweenId))
                LeanTween.cancel(screenTweenId);
        }
        #endregion

        #region Set Text.
        void SetGameOverResultText()
        {
            resultText.text = StrBuilderClass.Build_GameOverResultMsg();
        }
        #endregion

        #region Buttons.
        public void UIButton_RetryGame()
        {
            SessionManager.singleton.LoadScene_RetryGame();
        }

        public void UIButton_MainMenu()
        {
            SessionManager.singleton.LoadScene_ReturnMainMenu();
        }
        #endregion
    }
}