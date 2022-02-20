using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class BuildingDemolishBtn : MonoBehaviour
    {
        [Header("Button (Drops).")]
        public Button _demolishBtn;

        [Header("Refs.")]
        public Base_Building demo_building;

        public void UIButton_DestroyBuilding()
        {
            ReturnPortionResources();
            DestroyTargetBuilding();
        }

        void ReturnPortionResources()
        {
            ResourceCost[] _costs = demo_building.GetBuildingSO().costArray;
            for (int i = 0; i < _costs.Length; i++)
            {
                _costs[i].ReturnPortionResources();
            }
        }

        void DestroyTargetBuilding()
        {
            Destroy(demo_building.gameObject);
        }
    }
}