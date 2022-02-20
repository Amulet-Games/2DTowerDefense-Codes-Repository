using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Arrow_BuildingSelector : Base_BuildingSelector
    {
        #region UI Buttons.
        public override void OnSelected_Button()
        {
            _buildingManager.SetActiveBuildingType_Arrow();
        }
        #endregion
        
        #region On / Off Current Selector.
        protected override void SetActiveBuildingType()
        {
        }
        #endregion
        
        #region Pointer Events.
        protected override void ShowTipsByType()
        {
            _buildingManager._tooltipHandler.Show("Cursor", false);
        }
        #endregion
    }
}