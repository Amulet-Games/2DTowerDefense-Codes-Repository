using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class BuildingRepairBtn : MonoBehaviour
    {
        [Header("Button (Drops).")]
        public Button _repairBtn;

        [Header("Refs.")]
        [ReadOnlyInspector] public HealthSystem _buildingHealthSystem;

        #region Repair Building.
        public void UIButton_RepairBuilding()
        {
            CanAffordToRepair();
        }

        void CanAffordToRepair()
        {
            int repairCost = (_buildingHealthSystem._temp_b_hp - _buildingHealthSystem._cur_hp) / 2;

            ResourceManager _resourceManager = ResourceManager.singleton;
            if (_resourceManager.cur_goldAmt >= repairCost)
            {
                // Enough Gold to Repair.
                DepleteResourcesToRepair();
                HealBuilding();
            }
            else
            {
                // Not enough Gold to Repair.
                TooltipHandler.singleton.Show(StrBuilderClass.Build_InsufficientRepairCost(repairCost - _resourceManager.cur_goldAmt), true);
            }

            void DepleteResourcesToRepair()
            {
                _resourceManager.RemoveResource_Gold(repairCost);
            }

            void HealBuilding()
            {
                _buildingHealthSystem.HealFull();
            }
        }
        #endregion

        #region Show / Hide.
        public void ShowButton()
        {
            gameObject.SetActive(true);
        }

        public void HideButton()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Setup.
        public void Setup(HealthSystem buildingHealthSystem)
        {
            _buildingHealthSystem = buildingHealthSystem;
        }
        #endregion
    }
}