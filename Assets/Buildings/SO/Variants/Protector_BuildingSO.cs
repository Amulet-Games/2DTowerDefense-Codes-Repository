using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Data/Building Type/Protector")]
    public class Protector_BuildingSO : BuildingSO
    {
        [Header("Search Rate.")]
        public float searchRadius = 10f;
        public float searchRate = 0.3f;

        [Header("Shoot Rate.")]
        public float maxShootRate = 0.27f;
        public float minShootRate = 0.21f;
        
        [Header("Projectile.")]
        public ArrowProjectile _arrowProj;
        
        public override void CreateBuilding_AfterConstruction(Vector3 _constructPosition)
        {
            _temp_baseBuildingRef = Instantiate(prefab, _constructPosition, Quaternion.identity);

            // Setup Building - Generator.
            Setup_ActiveType_Protector();
        }

        void Setup_ActiveType_Protector()
        {
            _temp_baseBuildingRef.Setup();
        }
    }
}