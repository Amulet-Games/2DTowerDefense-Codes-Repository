using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Tower_BuildingSelector : Base_BuildingSelector
    {
        [Header("Generator Type (Drops).")]
        public Protector_BuildingSO referBuildingType;

        #region UI Buttons.
        public override void OnSelected_Button()
        {
            _buildingManager.SetActiveBuildingType_Tower();
        }
        #endregion

        #region On / Off Current Selector.
        protected override void SetActiveBuildingType()
        {
            // Active Building Type.
            _buildingManager._activeBuildingType = referBuildingType;
            _buildingManager._activeProtectorType = referBuildingType;
        }
        #endregion

        #region Pointer Events.
        protected override void ShowTipsByType()
        {
            _buildingManager._tooltipHandler.Show(StrBuilderClass.Build_SelectorTips_WithCost(referBuildingType), false);
        }
        #endregion
    }
}