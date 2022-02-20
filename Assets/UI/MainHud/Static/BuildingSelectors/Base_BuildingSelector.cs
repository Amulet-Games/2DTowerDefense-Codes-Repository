using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class Base_BuildingSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Outline Obj (Drops).")]
        public GameObject outlineObj;

        [Header("Refs")]
        [ReadOnlyInspector] public BuildingManager _buildingManager;

        #region UI Buttons.
        public abstract void OnSelected_Button();
        #endregion

        #region On / Off Current Selector.
        protected abstract void SetActiveBuildingType();

        public void OnCurSelector()
        {
            // Outline
            outlineObj.SetActive(true);

            // Active Building Type.
            SetActiveBuildingType();
        }

        public void OffCurSelector()
        {
            // Outline
            outlineObj.SetActive(false);
        }
        #endregion

        #region Pointer Events.
        protected abstract void ShowTipsByType();

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTipsByType();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buildingManager._tooltipHandler.Hide();
        }
        #endregion
    }
}