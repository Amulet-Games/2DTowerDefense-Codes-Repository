using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class TooltipHandler : MonoBehaviour
    {
        [Header("Rects (Drops).")]
        public RectTransform mainHudCanvasRect;
        public RectTransform handlerRect;
        public RectTransform tipsBackgroundRect;

        [Header("Texts (Drops).")]
        public TMP_Text tipsText;

        [Header("Timer Config.")]
        public float showRate;
        [ReadOnlyInspector] public float showTimer;
        [ReadOnlyInspector] public bool isShowTipsWithTimer;

        #region Privates.
        Vector2 anchoredPos;
        Rect tipsBG_rect_struct;
        Rect mainHud_rect_struct;
        #endregion

        #region Awake.
        public static TooltipHandler singleton;
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

            Hide();
        }
        #endregion

        #region Tick.
        public void Tick()
        {
            UpdateHandlerPosition();

            UpdateHandlerTimer();
        }

        void UpdateHandlerPosition()
        {
            anchoredPos = Input.mousePosition / mainHudCanvasRect.localScale.x;

            tipsBG_rect_struct = tipsBackgroundRect.rect;
            mainHud_rect_struct = mainHudCanvasRect.rect;

            #region Check Out of Bounds (Width).
            if (anchoredPos.x + tipsBG_rect_struct.width > mainHud_rect_struct.width)
            {
                anchoredPos.x = mainHud_rect_struct.width - tipsBG_rect_struct.width;
            }
            else if (anchoredPos.x < 0)
            {
                anchoredPos.x = 0;
            }
            #endregion

            #region Check Out of Bounds (Height).
            if (anchoredPos.y + tipsBG_rect_struct.height > mainHud_rect_struct.height)
            {
                anchoredPos.y = mainHud_rect_struct.height - tipsBG_rect_struct.height;
            }
            else if (anchoredPos.y < 0)
            {
                anchoredPos.y = 0;
            }
            #endregion

            handlerRect.anchoredPosition = anchoredPos;
        }

        void UpdateHandlerTimer()
        {
            if (isShowTipsWithTimer)
            {
                showTimer += Time.deltaTime;
                if (showTimer >= showRate)
                {
                    OffIsShowTipsWithTimer();
                    Hide();
                }
            }
        }
        #endregion

        #region Show Tips.
        public void Show(string tipsText, bool showWithTimer)
        {
            gameObject.SetActive(true);
            SetText(tipsText);

            if (showWithTimer)
            {
                OnIsShowTipsWithTimer();
            }
            else
            {
                OffIsShowTipsWithTimer();
            }
        }

        void SetText(string tooltipText)
        {
            tipsText.text = tooltipText;
            tipsText.ForceMeshUpdate();

            Vector2 textSize = tipsText.GetRenderedValues(false);
            Vector2 padding = new Vector2(20, 7.5f);
            tipsBackgroundRect.sizeDelta = textSize + padding;
        }
        #endregion

        #region On / Off IsShowTipsWithTimer.
        void OnIsShowTipsWithTimer()
        {
            isShowTipsWithTimer = true;
            showTimer = 0;
        }

        void OffIsShowTipsWithTimer()
        {
            isShowTipsWithTimer = false;
        }
        #endregion

        #region Hide Tips.
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}