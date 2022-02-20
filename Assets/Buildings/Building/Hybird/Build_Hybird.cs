using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Build_Hybird : Base_Building
    {
        [Header("Gen Drops.")]
        public ResourceGeneratorOverlay generatorOverlay;

        [Header("Gen Status.")]
        [ReadOnlyInspector] public float timerMax;
        [ReadOnlyInspector] float timer;
        [ReadOnlyInspector] float normalizedTimer;
        [ReadOnlyInspector] float amountPerSecond;

        [Header("Prot Drops.")]
        public Transform arrowSpawnPoint;

        [Header("Prot Status.")]
        [ReadOnlyInspector] float searchTimer;
        [ReadOnlyInspector] float shootTimer;
        [ReadOnlyInspector] float _finalizedShootRate;

        [Header("Prot Target.")]
        [ReadOnlyInspector] public Transform _cur_targetTransform;
        [ReadOnlyInspector] public Collider2D _cur_targetCollider;

        [Header("Hybird Refs.")]
        [ReadOnlyInspector] public Hybird_BuildingSO _referBuildingType;

        private void Update()
        {
            MonitorAddResourceTimer();

            MonitorOverlayInfos();

            MonitorSearchTimer();

            MonitorShootTimer();
        }

        #region Tick.

        /// Generator.

        #region Add Resources.
        void MonitorAddResourceTimer()
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                timer = 0;
                AddResource();
            }
        }

        void AddResource()
        {
            _buildingManager._resourceManager.AddResource_Wood(1);
        }
        #endregion

        #region Overlay Infos.
        void MonitorOverlayInfos()
        {
            UpdateOverlayInfos();
        }
        #endregion

        /// Protector.
        
        #region Shoot Target.
        void MonitorShootTimer()
        {
            if (_cur_targetTransform != null)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= _finalizedShootRate)
                {
                    shootTimer = 0;
                    ShootCurrentTarget();
                }
            }
        }

        void ShootCurrentTarget()
        {
            ArrowProjectile _arrow = Instantiate(_referBuildingType._arrowProj, arrowSpawnPoint.position, Quaternion.identity);

            SetupArrow();

            void SetupArrow()
            {
                _arrow._cur_targetCollider = _cur_targetCollider;
                _arrow._cur_targetTransform = _cur_targetTransform;
            }
        }
        #endregion
        
        #region Find Target.
        void MonitorSearchTimer()
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= _referBuildingType.searchRate)
            {
                searchTimer = 0;
                LookForTargets();
            }
        }

        void LookForTargets()
        {
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, _referBuildingType.searchRadius, _buildingManager._layerManager.enemyMask);

            int foundLength = collider2DArray.Length;
            if (foundLength > 0)
            {
                FindClosetTarget();
            }

            void FindClosetTarget()
            {
                float _closetDistance = 100000;
                Collider2D _closetBuildingCol = null;

                for (int i = 0; i < foundLength; i++)
                {
                    float _sqrDis = Vector2.SqrMagnitude(collider2DArray[i].transform.position - transform.position);
                    if (_sqrDis < _closetDistance)
                    {
                        _closetBuildingCol = collider2DArray[i];
                        _closetDistance = _sqrDis;
                    }
                }

                _cur_targetCollider = _closetBuildingCol;
                _cur_targetTransform = _closetBuildingCol.transform;
            }
        }
        #endregion

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
            SetupRefs_Hybird();

            SetupHealthSystem_Hybird();

            SetupGenerateRateByNearbyResource();

            SetupOverlayInfo();

            SetupFinalizedShootRate();

            BaseFunc_PlayBuildFinishedParticle();

            BaseFunc_SetupRepairButton();

            SubscribeEvent_OnHeal();

            SubscribeEvent_OnDamaged();

            SubscribeEvent_OnDied();
        }

        void SetupRefs_Hybird()
        {
            _buildingManager = BuildingManager.singleton;
            _referBuildingType = _buildingManager.hqBuildingType;
        }

        void SetupHealthSystem_Hybird()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem._temp_b_hp = _referBuildingType.b_hp;
            _healthSystem.Setup();
        }

        void SetupGenerateRateByNearbyResource()
        {
            _buildingManager.FindAllResourcesNearyby_Hybird(_referBuildingType, transform);

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

        void SetupFinalizedShootRate()
        {
            _finalizedShootRate = Random.Range(_referBuildingType.minShootRate, _referBuildingType.maxShootRate);
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

            AISessionManager_OnDied();

            BuildingManager_OnDied();

            MainHudManager_OnDied();

            PlayGameOverSound();

            ShowGameOverScreen();
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