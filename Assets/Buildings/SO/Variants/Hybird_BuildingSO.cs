using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Data/Building Type/Hybird")]
    public class Hybird_BuildingSO : BuildingSO
    {
        [Header("Gen Resources.")]
        public p_GeneratorTypeEnum generatorType;
        public Sprite resourceSprite;

        [Header("Gen Rate.")]
        public float generateRate;
        public int maxGenRateAmount;

        [Header("Gen Radius.")]
        public float generateRadius;

        [Header("Search Rate.")]
        public float searchRadius = 10f;
        public float searchRate = 0.3f;

        [Header("Shoot Rate.")]
        public float maxShootRate = 0.27f;
        public float minShootRate = 0.21f;
        
        [Header("Projectile.")]
        public ArrowProjectile _arrowProj;

        public override void CreateBuilding_immediate()
        {
            _temp_baseBuildingRef = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Setup Building - Generator.
            Setup_HQ_Hybird();

            // Set as AI's base enemy.
            SetAsEnemyBaseTarget();
        }
        
        void Setup_HQ_Hybird()
        {
            _temp_baseBuildingRef.Setup();
        }

        void SetAsEnemyBaseTarget()
        {
            AISessionManager.singleton._HQ_BuildingTransform = _temp_baseBuildingRef.transform;
        }
    }
}