using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Data/Building Type/Generator")]
    public class Generator_BuildingSO : BuildingSO
    {
        [Header("Gen Resources.")]
        public p_GeneratorTypeEnum generatorType;
        public Sprite resourceSprite;

        [Header("Gen Rate.")]
        public float generateRate;
        public int maxGenRateAmount;

        [Header("Gen Radius.")]
        public float generateRadius;
        
        public override void CreateBuilding_AfterConstruction(Vector3 _constructPosition)
        {
            _temp_baseBuildingRef = Instantiate(prefab, _constructPosition, Quaternion.identity);

            // Setup Building - Generator.
            Setup_ActiveType_Generator();
        }

        void Setup_ActiveType_Generator()
        {
            _temp_baseBuildingRef.Setup();
        }
    }
}