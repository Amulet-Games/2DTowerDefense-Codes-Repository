using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Enemy : MonoBehaviour
    {
        [Header("Rb (Drops).")]
        public Rigidbody2D rb;

        [Header("Enemy Visual & Collider")]
        public GameObject enemyHidableObjs;
        public Collider2D enemyCollider;

        [Header("Explosion Particle.")]
        public ParticleSystem enemyExplosionParticle;

        [Header("Health System (Drops).")]
        public HealthSystem _healthSystem;

        [Header("Status.")]
        [ReadOnlyInspector] public float _finalizedSearchRate;
        [ReadOnlyInspector] float searchTimer;

        [Header("Target Transform.")]
        [ReadOnlyInspector] public Transform _base_TargetTransform;
        [ReadOnlyInspector] public Transform _cur_targetTransform;
        [ReadOnlyInspector] public bool hasChangedTargetOnce;

        [Header("Refs.")]
        [ReadOnlyInspector] public AISessionManager _aiSessionManager;

        private void Update()
        {
            MoveTowardTarget();

            MonitorSearchTimer();
        }

        #region Movement.
        void MoveTowardTarget()
        {
            Vector3 moveDir;

            if (_cur_targetTransform != null)
            {
                moveDir = (_cur_targetTransform.position - transform.position).normalized;
                rb.velocity = moveDir * _aiSessionManager.moveSpeed;
            }
            else if (_base_TargetTransform != null)
            {
                moveDir = (_base_TargetTransform.position - transform.position).normalized;
                rb.velocity = moveDir * _aiSessionManager.moveSpeed;
            }
        }
        #endregion

        #region Attack / On Collusion Enter.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Base_Building building = collision.gameObject.GetComponent<Base_Building>();

            if (building != null)
            {
                building._healthSystem.Damage(10);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Find Target.
        void MonitorSearchTimer()
        {
            if (hasChangedTargetOnce)
                return;

            searchTimer += Time.deltaTime;
            if (searchTimer >= _finalizedSearchRate)
            {
                searchTimer = 0;
                LookForTargets();
            }
        }

        void LookForTargets()
        {
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, _aiSessionManager.searchRadius, _aiSessionManager._layerManager.buildingMask);

            int foundLength = collider2DArray.Length;
            if (foundLength > 1)
            {
                FindClosetTarget_Multi();
            }
            else
            {
                FindClosetTarget_NoResult();
            }

            void FindClosetTarget_Multi()
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

                _cur_targetTransform = _closetBuildingCol.transform;
                hasChangedTargetOnce = true;
            }

            void FindClosetTarget_NoResult()
            {
                _cur_targetTransform = _base_TargetTransform;
            }
        }
        #endregion

        #region Setup.
        public void Setup(AISessionManager aiSessionManager)
        {
            _aiSessionManager = aiSessionManager;

            SetupGetTargetTransform();

            SetupInitHealthSystem();

            SetupFinalizedSearchRate();

            SubscribeEvent_OnDamaged();

            SubscribeEvent_OnDied();
        }

        void SetupGetTargetTransform()
        {
            _base_TargetTransform = _aiSessionManager._HQ_BuildingTransform;
            LookForTargets();
            _cur_targetTransform = _base_TargetTransform;
        }

        void SetupInitHealthSystem()
        {
            _healthSystem._temp_b_hp = _aiSessionManager.b_hp;
            _healthSystem.Setup();
        }

        void SetupFinalizedSearchRate()
        {
            _finalizedSearchRate = _aiSessionManager.GetRandomizedSearchRate();
        }

        void SubscribeEvent_OnDamaged()
        {
            _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }

        void SubscribeEvent_OnDied()
        {
            _healthSystem.OnDied += HealthSystem_OnDied;
        }
        #endregion

        #region Events.
        void HealthSystem_OnDamaged(object sender, System.EventArgs e)
        {
            ShakeCamera_OnDamaged();
            PlayChromaticAberrationEffect();
            PlayEnemyDamagedSound();
        }

        void HealthSystem_OnDied(object sender, System.EventArgs e)
        {
            HideEnemy();
            PlayExplosionParticle();
            ShakeCamera_OnDied();
            PlayChromaticAberrationEffect();
            PlayEnemyDieSound();
        }
        #endregion

        #region OnDamaged Actions.
        void PlayEnemyDamagedSound()
        {
            SoundManager.singleton.PlaySound_EnemyHit();
        }

        void ShakeCamera_OnDamaged()
        {
            CinemachineShake.singleton.ShakeCamera(3f, 0.15f);
        }
        #endregion

        #region OnDied Actions.
        void HideEnemy()
        {
            enemyHidableObjs.SetActive(false);
            enemyCollider.enabled = false;
        }

        void PlayExplosionParticle()
        {
            enemyExplosionParticle.gameObject.SetActive(true);
        }

        void ShakeCamera_OnDied()
        {
            CinemachineShake.singleton.ShakeCamera(7f, 0.2f);
        }

        void PlayEnemyDieSound()
        {
            SoundManager.singleton.PlaySound_EnemyDie();
        }
        #endregion

        #region OnDied & OnDamaged
        protected void PlayChromaticAberrationEffect()
        {
            ChromaticAberrationEffect.singleton.SetWeight(0.5f);
        }
        #endregion
    }
}