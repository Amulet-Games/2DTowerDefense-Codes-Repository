using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SA
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Logo Before Config.")]
        public float orig_anch_width;
        public float orig_anch_height;
        public float orig_anch_YPos;

        [Header("Logo After Config.")]
        public float after_anch_width;
        public float after_anch_height;
        public float after_anch_YPos;

        [Header("Logo Tween (Drops).")]
        public RectTransform logoRect;
        public LeanTweenType logoSizeEaseType;
        public float logoSizeChangeSpeed;
        public LeanTweenType logoMoveEaseType;
        public float logoMoveupSpeed;

        [Header("Btns Group (Drops).")]
        public CanvasGroup btnsGroup;
        public Canvas btnsCanvas;

        [Header("Btns Group Tween.")]
        public LeanTweenType btnsEaseType;
        public float btnsFadeSpeed;

        [Header("Diff Mother Group (Drops).")]
        public CanvasGroup diffGroup;
        public Canvas diffCanvas;

        [Header("Diff Icon RectTransforms (Drops).")]
        public RectTransform easy_Diff_IconRect;
        public RectTransform normal_Diff_IconRect;
        public RectTransform hard_Diff_IconRect;
        public LeanTweenType diffIconEaseType;
        public float diffIconTweenWaitRate;
        public float diffIconSizeChangeSpeed;
        public float targetDiffIconSize;

        [Header("Diff Texts Group (Drops).")]
        public CanvasGroup diffTextGroup;
        public Canvas diffTextCanvas;
        public LeanTweenType diffTextEaseType;
        public float diffTextTweenWaitRate;
        public float diffTextFadeSpeed;

        [Header("Diff btns Drops")]
        public Button playEasyButton;
        public Button playNormButton;
        public Button playHardButton;
        public Button diff_quitGameButton;

        [Header("Refs.")]
        [ReadOnlyInspector] public SessionManager _sessionManager;
        
        #region Callbacks.
        private void Start()
        {
            Setup();
        }
        #endregion

        #region UI Buttons.
        public void UIButton_Start()
        {
            TweenLogoSize();
            HideBtnsGroup();
        }

        public void UIButton_Quit()
        {
            Application.Quit();
        }

        public void UIButton_PlayEasy()
        {
            _sessionManager.cur_gameScene_Index = (int)G_SceneTypeEnum.EasyGameScene;
            _sessionManager.LoadScene_PlayGame();
        }

        public void UIButton_PlayNormal()
        {
            _sessionManager.cur_gameScene_Index = (int)G_SceneTypeEnum.NormalGameScene;
            _sessionManager.LoadScene_PlayGame();
        }

        public void UIButton_PlayHard()
        {
            _sessionManager.cur_gameScene_Index = (int)G_SceneTypeEnum.HardGameScene;
            _sessionManager.LoadScene_PlayGame();
        }
        #endregion

        #region Change Logo Size / YPos.
        void TweenLogoSize()
        {
            LeanTween.value(logoRect.rect.width, after_anch_width, logoSizeChangeSpeed).setEase(logoSizeEaseType).setOnUpdate((value) => 
            {
                logoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
            });

            LeanTween.value(logoRect.rect.height, after_anch_height, logoSizeChangeSpeed).setEase(logoSizeEaseType).setOnUpdate((value) =>
            {
                logoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
            }).setOnComplete(OnCompleteTweenSize);

            void OnCompleteTweenSize()
            {
                TweenLogoYPos();

                /// Enable The Diff Mother Canvas immediately.
                Enable_DiffMother_Canvas();

                /// Wait a sec before Tweening Icon Size.
                Wait_TweenDiffIconSize();

                /// Wait a sec before Fade In Diff Texts.
                Wait_FadeIn_DiffText();
            }
        }

        void TweenLogoYPos()
        {
            logoRect.LeanMoveLocalY(after_anch_YPos, logoMoveupSpeed).setEase(logoMoveEaseType);
        }
        #endregion

        #region Hide / Show Btns Group.
        void HideBtnsGroup()
        {
            LeanTween.alphaCanvas(btnsGroup, 0, btnsFadeSpeed).setEase(btnsEaseType).setOnComplete(Disable_Btns_Canvas);
        }

        void ShowBtnsGroup()
        {
            LeanTween.alphaCanvas(btnsGroup, 1, btnsFadeSpeed).setEase(btnsEaseType);
        }

        void Disable_Btns_Canvas()
        {
            btnsCanvas.enabled = false;
        }

        void Enable_Btns_Canvas()
        {
            btnsCanvas.enabled = true;
        }
        #endregion

        #region Enable / Disable Diff Mother Group.
        void Disable_DiffMother_Canvas()
        {
            diffGroup.alpha = 0;
            diffCanvas.enabled = false;
        }

        void Enable_DiffMother_Canvas()
        {
            diffGroup.alpha = 1;
            diffCanvas.enabled = true;
        }
        #endregion

        #region Change Diff Icons Size.
        void Wait_TweenDiffIconSize()
        {
            LeanTween.value(0, 1, diffIconTweenWaitRate).setOnComplete(OnCompleteWait);
        
            void OnCompleteWait()
            {
                /// Tween Ease Difficulty Icon.
                Tween_DiffIcon_Size(easy_Diff_IconRect);
                Tween_DiffIcon_Size(normal_Diff_IconRect);
                Tween_DiffIcon_Size(hard_Diff_IconRect);
            }
        }

        void Tween_DiffIcon_Size(RectTransform _rectTransform)
        {
            LeanTween.value(0, targetDiffIconSize, diffIconSizeChangeSpeed).setEase(diffIconEaseType).setOnUpdate(
                (value) =>
                {
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);

                });
        }
        #endregion

        #region Fade In / Out Diff Text Group.
        void Wait_FadeIn_DiffText()
        {
            LeanTween.value(0, 1, diffTextTweenWaitRate).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                /// Fade In Diff Text Group.
                FadeIn_DiffText_Group();
            }
        }

        void FadeIn_DiffText_Group()
        {
            Enable_DiffText_Canvas();
            LeanTween.alphaCanvas(diffTextGroup, 1, diffTextFadeSpeed).setEase(diffTextEaseType).setOnComplete(OnCompleteTween);

            void OnCompleteTween()
            {
                Enable_DiffBtns_IsInteractable();
                Enable_DiffQuitGame_IsInteractable();
            }
        }

        void FadeOut_DiffText_Group()
        {
            LeanTween.alphaCanvas(diffTextGroup, 0, diffTextFadeSpeed).setEase(diffTextEaseType).setOnComplete(OnCompleteTween);

            void OnCompleteTween()
            {
                Diable_DiffText_Canvas();
            }
        }

        void Enable_DiffText_Canvas()
        {
            diffTextCanvas.enabled = true;
        }

        void Diable_DiffText_Canvas()
        {
            diffTextCanvas.enabled = false;
        }
        #endregion

        #region Disable Btns Group Raycasters.
        public void DisableBtnsGroupInteractable()
        {
            btnsGroup.blocksRaycasts = false;
        }

        public void EnableBtnsGroupInteractable()
        {
            btnsGroup.blocksRaycasts = true;
        }
        #endregion

        #region Enable / Disable Diff Interactables.
        void Enable_DiffBtns_IsInteractable()
        {
            playEasyButton.interactable = true;
            playNormButton.interactable = true;
            playHardButton.interactable = true;
        }

        void Disable_DiffBtns_IsInteractable()
        {
            playEasyButton.interactable = false;
            playNormButton.interactable = false;
            playHardButton.interactable = false;
        }

        void Enable_DiffQuitGame_IsInteractable()
        {
            diff_quitGameButton.interactable = true;
        }
        #endregion

        #region Setup.
        void Setup()
        {
            SetupRefs();
            SetupSoundManager();
            SetupMusicManager();
        }

        void SetupRefs()
        {
            _sessionManager = SessionManager.singleton;
        }

        void SetupSoundManager()
        {
            SoundManager.singleton.Setup();
        }

        void SetupMusicManager()
        {
            MusicManager.singleton.Setup();
        }
        #endregion
    }
}