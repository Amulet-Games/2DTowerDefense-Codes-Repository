using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class MainHudManager : MonoBehaviour
    {
        #region General.
        #endregion

        #region Upper Right.
        [Header("Resources Text (Drops).")]
        public TMP_Text woodText;
        public TMP_Text stoneText;
        public TMP_Text goldText;
        #endregion

        #region Bottom Left.
        [Header("Buttons (Drops).")]
        public CanvasGroup selectorsGroup;
        public Arrow_BuildingSelector arrowSelector;
        public Wood_BuildingSelector woodSelector;
        public Stone_BuildingSelector stoneSelector;
        public Gold_BuildingSelector goldSelector;
        public Tower_BuildingSelector towerSelector;
        #endregion
        
        [Header("Refs.")]
        [ReadOnlyInspector] public BuildingManager _buildingManager;
        [ReadOnlyInspector] public ResourceManager _resourceManager;
        [ReadOnlyInspector] public Base_BuildingSelector _curSelector;

        public static MainHudManager singleton;
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
        
        #region Update Resources Text.
        public void UpdateWoodText()
        {
            woodText.text = _resourceManager.cur_woodAmt.ToString();
        }

        public void UpdateStoneText()
        {
            stoneText.text = _resourceManager.cur_stoneAmt.ToString();
        }

        public void UpdateGoldText()
        {
            goldText.text = _resourceManager.cur_goldAmt.ToString();
        }
        #endregion

        #region Select Current Selector.
        public void SetSelectImage_Arrow()
        {
            _curSelector.OffCurSelector();
            _curSelector = arrowSelector;
            _curSelector.OnCurSelector();
        }

        public void SetSelectImage_Wood()
        {
            _curSelector.OffCurSelector();
            _curSelector = woodSelector;
            _curSelector.OnCurSelector();
        }

        public void SetSelectImage_Stone()
        {
            _curSelector.OffCurSelector();
            _curSelector = stoneSelector;
            _curSelector.OnCurSelector();
        }

        public void SetSelectImage_Gold()
        {
            _curSelector.OffCurSelector();
            _curSelector = goldSelector;
            _curSelector.OnCurSelector();
        }

        public void SetSelectImage_Tower()
        {
            _curSelector.OffCurSelector();
            _curSelector = towerSelector;
            _curSelector.OnCurSelector();
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupRefs();
            SetupResourcesText();
            SetupSelectors();
            SetupSetCurSelector();
        }

        void SetupRefs()
        {
            _resourceManager = ResourceManager.singleton;
        }

        void SetupResourcesText()
        {
            woodText.text = _resourceManager.cur_woodAmt.ToString();
            stoneText.text = _resourceManager.cur_stoneAmt.ToString();
            goldText.text = _resourceManager.cur_goldAmt.ToString();
        }

        void SetupSelectors()
        {
            arrowSelector._buildingManager = _buildingManager;
            woodSelector._buildingManager = _buildingManager;
            stoneSelector._buildingManager = _buildingManager;
            goldSelector._buildingManager = _buildingManager;
            towerSelector._buildingManager = _buildingManager;
        }

        void SetupSetCurSelector()
        {
            _curSelector = arrowSelector;
        }
        #endregion

        #region On Died.
        public void OnDied()
        {
            selectorsGroup.blocksRaycasts = false;
        }
        #endregion
    }
}