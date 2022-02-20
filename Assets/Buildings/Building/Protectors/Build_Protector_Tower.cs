using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Build_Protector_Tower : Base_Building
    {
        [Header("Arrow Spawn Point (Drops).")]
        public Transform arrowSpawnPoint;

        [Header("Prot Status.")]
        [ReadOnlyInspector] float searchTimer;
        [ReadOnlyInspector] float shootTimer;
        [ReadOnlyInspector] float _finalizedShootRate;

        [Header("Prot Refs.")]
        [ReadOnlyInspector] public Transform _cur_targetTransform;
        [ReadOnlyInspector] public Collider2D _cur_targetCollider;

        [Header("Protector Refs.")]
        [ReadOnlyInspector] public Protector_BuildingSO _referBuildingType;
        
        private void Update()
        {
            MonitorSearchTimer();

            MonitorShootTimer();
        }

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

        #region Setup.
        public override void Setup()
        {
            SetupRefs_Protect();

            SetupHealthSystem_Protect();

            SetupFinalizedShootRate();

            BaseFunc_PlayBuildFinishedParticle();

            BaseFunc_SetupRepairButton();

            SubscribeEvent_OnHeal();

            SubscribeEvent_OnDamaged();

            SubscribeEvent_OnDied();
        }

        void SetupRefs_Protect()
        {
            _buildingManager = BuildingManager.singleton;
            _referBuildingType = _buildingManager._activeProtectorType;
        }

        void SetupHealthSystem_Protect()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem._temp_b_hp = _referBuildingType.b_hp;
            _healthSystem.Setup();
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
            PlayBuildingDestroySound();
        }
        #endregion
    }
}