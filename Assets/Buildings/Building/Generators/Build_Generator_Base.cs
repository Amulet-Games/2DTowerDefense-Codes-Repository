using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class Build_Generator_Base : Base_Building
    {
        [Header("Gen Overlay (Drops).")]
        public ResourceGeneratorOverlay generatorOverlay;

        [Header("Gen Status.")]
        [ReadOnlyInspector] public float timerMax;
        [ReadOnlyInspector] float timer;
        [ReadOnlyInspector] float normalizedTimer;
        [ReadOnlyInspector] float amountPerSecond;

        [Header("Gen Refs.")]
        [ReadOnlyInspector] public Generator_BuildingSO _referBuildingType;

        protected abstract void AddResource();

        private void Update()
        {
            MonitorAddResourceTimer();

            MonitorOverlayInfos();
        }

        #region Tick.
        void MonitorAddResourceTimer()
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                timer = 0;
                AddResource();
            }
        }

        void MonitorOverlayInfos()
        {
            UpdateOverlayInfos();
        }
        #endregion

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
            SetupRefs_Generator();

            SetupHealthSystem_Generator();

            SetupGenerateRateByNearbyResource();

            SetupOverlayInfo();

            BaseFunc_PlayBuildFinishedParticle();

            BaseFunc_SetupRepairButton();

            SubscribeEvent_OnHeal();

            SubscribeEvent_OnDamaged();

            SubscribeEvent_OnDied();
        }

        void SetupRefs_Generator()
        {
            _buildingManager = BuildingManager.singleton;
            _referBuildingType = _buildingManager._temp_GeneratorType;
        }

        void SetupHealthSystem_Generator()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem._temp_b_hp = _referBuildingType.b_hp;
            _healthSystem.Setup();
        }

        void SetupGenerateRateByNearbyResource()
        {
            _buildingManager.FindAllResourcesNearyby(_referBuildingType, transform);
            
            // Clamp the Amount to not go over Max Generate rate.
            int _nearbyResourceAmount = _buildingManager.nearbyResourceAmount;
            _nearbyResourceAmount = _nearbyResourceAmount > _referBuildingType.maxGenRateAmount ? _referBuildingType.maxGenRateAmount : _nearbyResourceAmount;
            
            if (_nearbyResourceAmount == 0)
            {
                // No resource nodes nearby
                // Disable resource generator
                enabled = false;
            }
            else
            {
                timerMax = (_referBuildingType.generateRate / 2f) +
                    _referBuildingType.generateRate *
                    (1 - (float)_nearbyResourceAmount / _referBuildingType.maxGenRateAmount);
            }

            /// Debug.Log("nearbyResourceAmount: " + nearbyResourceAmount + "; timerMax: " + timerMax);
        }
        
        void SetupOverlayInfo()
        {
            SetupInitOverlayInfos();
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

        #region Overlay Infos.
        void UpdateOverlayInfos()
        {
            // Setup Sprite.
            generatorOverlay.overlayIconSpriteRenderer.sprite = _referBuildingType.resourceSprite;

            // Setup Progress Bar.
            SetNormalizedTimer();
            generatorOverlay.overlayBarTransform.localScale = new Vector3(1 - normalizedTimer, 1, 1);

            // Setup Text.
            SetAmountPerSecond();
            generatorOverlay.overlayText.text = amountPerSecond.ToString("F1");
        }

        void SetupInitOverlayInfos()
        {
            // Setup Sprite.
            generatorOverlay.overlayIconSpriteRenderer.sprite = _referBuildingType.resourceSprite;

            if (_buildingManager.nearbyResourceAmount != 0)
            {
                // Setup Progress Bar.
                SetNormalizedTimer();
                generatorOverlay.overlayBarTransform.localScale = new Vector3(1 - normalizedTimer, 1, 1);

                // Setup Text.
                SetAmountPerSecond();
                generatorOverlay.overlayText.text = amountPerSecond.ToString("F1");
            }
            else
            {
                generatorOverlay.overlayBarTransform.localScale = new Vector3(0, 1, 1);
                generatorOverlay.overlayText.text = "-";
            }
        }
        #endregion

        #region Utility.
        void SetNormalizedTimer()
        {
            normalizedTimer = timer / timerMax;
        }

        void SetAmountPerSecond()
        {
            amountPerSecond = 1 / timerMax;
        }
        #endregion
    }
}