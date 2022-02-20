using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Build_Plain : Base_Building
    {
        [Header("Plain Refs.")]
        [ReadOnlyInspector] public BuildingSO _referBuildingType;

        #region Get.
        public override p_BuildingTypeEnum GetBuildingType()
        {
            return _referBuildingType.buildingType;
        }

        public override BuildingSO GetBuildingSO()
        {
            return _referBuildingType;
        }
        #endregion

        #region Setup.
        public override void Setup()
        {
            SetupRefs_Plain();

            SetupHealthSystem_Plain();

            BaseFunc_PlayBuildFinishedParticle();

            BaseFunc_SetupRepairButton();

            SubscribeEvent_OnHeal();

            SubscribeEvent_OnDamaged();

            SubscribeEvent_OnDied();
        }

        void SetupRefs_Plain()
        {
            _buildingManager = BuildingManager.singleton;
            _referBuildingType = _buildingManager._activeBuildingType;
        }
        
        void SetupHealthSystem_Plain()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem._temp_b_hp = _referBuildingType.b_hp;
            _healthSystem.Setup();
        }
        
        protected override void SubscribeEvent_OnHeal()
        {
            _healthSystem.OnHeal += HealthSystem_OnHeal;
        }

        protected override void SubscribeEvent_OnDamaged()
        {
            _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }

        protected override void SubscribeEvent_OnDied()
        {
            _healthSystem.OnDied += HealthSystem_OnDied;
        }
        #endregion

        #region Events.
        void HealthSystem_OnHeal(object sender, System.EventArgs e)
        {
            HideRepairButton();
        }

        void HealthSystem_OnDamaged(object sender, System.EventArgs e)
        {
            ShowRepairButton();
            PlayChromaticAberrationEffect();
            PlayBuildingDamagedSound();
        }

        void HealthSystem_OnDied(object sender, System.EventArgs e)
        {
            Instantiate_BuildingDestroyed_Particle();
            ShakeCamera();
            PlayChromaticAberrationEffect();
            DestroyGameObject();
            PlayBuildingDestroySound();
        }
        #endregion
    }
}