using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BuildingSO : ScriptableObject
    {
        [Header("Basic Info.")]
        public p_BuildingTypeEnum buildingType;
        public string build_Name;
        public Sprite build_Sprite;

        [Header("Hp.")]
        public int b_hp;

        [Header("Costs.")]
        public ResourceCost[] costArray;

        [Header("Construct.")]
        public float minConstructionRadius;
        public float constructionTimerMax;

        [Header("Prefab")]
        public Base_Building prefab;

        protected Base_Building _temp_baseBuildingRef;

        public virtual void CreateBuilding_immediate()
        {
        }

        public virtual void CreateBuilding_AfterConstruction(Vector3 _constructPosition)
        {
        }
        
        public void DepleteResources()
        {
            for (int i = 0; i < costArray.Length; i++)
            {
                costArray[i].DepleteResources();
            }
        }
    }
}