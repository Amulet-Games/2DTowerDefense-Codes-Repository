using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class Base_Building : MonoBehaviour
    {
        [Header("2D Collider (Drops).")]
        public BoxCollider2D buildingCol;

        [Header("Particles (Drops).")]
        public ParticleSystem buildFinishedParticle;

        [Header("Repair Button (Drops).")]
        public BuildingRepairBtn repairBtn;

        [Header("Refs.")]
        [ReadOnlyInspector] public BuildingManager _buildingManager;
        [ReadOnlyInspector] public HealthSystem _healthSystem;

        #region Get.
        public abstract p_BuildingTypeEnum GetBuildingType();

        public abstract BuildingSO GetBuildingSO();
        #endregion

        #region Setup.
        public abstract void Setup();

        protected void BaseFunc_PlayBuildFinishedParticle()
        {
            buildFinishedParticle.gameObject.SetActive(true);
        }

        protected void BaseFunc_SetupRepairButton()
        {
            repairBtn.Setup(_healthSystem);
        }

        protected abstract void SubscribeEvent_OnHeal();

        protected abstract void SubscribeEvent_OnDamaged();

        protected abstract void SubscribeEvent_OnDied();
        #endregion
        
        #region OnHeal Actions.
        protected void HideRepairButton()
        {
            repairBtn.HideButton();
        }
        #endregion

        #region OnDamaged Actions.
        protected void ShowRepairButton()
        {
            repairBtn.ShowButton();
        }

        protected void PlayBuildingDamagedSound()
        {
            SoundManager.singleton.PlaySound_BuildingDamaged();
        }
        #endregion

        #region OnDied Actions.
        protected void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        protected void Instantiate_BuildingDestroyed_Particle()
        {
            Instantiate(_buildingManager.buildingDestroyedParticle, transform.position, Quaternion.identity);
        }

        protected void ShakeCamera()
        {
            CinemachineShake.singleton.ShakeCamera(7f, 0.2f);
        }

        protected void AISessionManager_OnDied()
        {
            AISessionManager.singleton.OnDied();
        }

        protected void BuildingManager_OnDied()
        {
            _buildingManager.OnDied();
        }

        protected void MainHudManager_OnDied()
        {
            MainHudManager.singleton.OnDied();
        }

        protected void ShowGameOverScreen()
        {
            GameOverScreenUI.singleton.ShowScreen();
        }
        
        protected void PlayBuildingDestroySound()
        {
            SoundManager.singleton.PlaySound_BuildingDestroyed();
        }

        protected void PlayGameOverSound()
        {
            SoundManager.singleton.PlaySound_GameOver();
        }
        #endregion

        #region OnDied & OnDamaged
        protected void PlayChromaticAberrationEffect()
        {
            ChromaticAberrationEffect.singleton.SetWeight(1f);
        }
        #endregion
    }
}