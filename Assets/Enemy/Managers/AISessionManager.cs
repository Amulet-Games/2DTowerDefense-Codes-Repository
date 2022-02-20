using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AISessionManager : MonoBehaviour
    {
        [Header("New Wave Rate.")]
        public float start_WaitRate;
        public float b_waitRate;
        public float waitDepleteRate;
        public float least_WaitRate;
        [ReadOnlyInspector] public float newWaveTimer;
        [ReadOnlyInspector] public bool isPausingWaveTimer;

        [Header("Enemy Spawn Rate.")]
        /// Base Enemy Spawn Rate.
        public float b_MinEnemySpawnRate;
        public float b_MaxEnemySpawnRate;
        public float least_MaxEnemySpawnRate;

        /// Enemy Spawn Deplete Rate.
        public float maxSpawnRateDepleteRate;

        /// Cur Enemy Spawn Rate.
        [ReadOnlyInspector] public float cur_maxEnemySpawnRate;

        [Header("Wave Status.")]
        public int baseEnemyCount = 5;
        public int enemyCountPerWave = 3;
        [ReadOnlyInspector] public int waveNumber;

        [Header("Next Enemy Spawn Rate.")]
        [ReadOnlyInspector] public float nextEnemySpawnRate;
        [ReadOnlyInspector] public float nextEnemySpawnTimer;
        [ReadOnlyInspector] public float remainingEnemySpawnAmount;

        [Header("Enemy Speed.")]
        public float moveSpeed = 6f;

        [Header("Enemy Search.")]
        public float searchRadius = 10f;
        public float maxSearchRate = 0.25f;
        public float minSearchRate = 0.2f;

        [Header("Enemy Health.")]
        public int b_hp = 20;
        public int fir_hp_increaseThershold;
        public int sec_hp_increaseThershold;
        public int hp_Amt_after1stThershold = 30;
        public int hp_Amt_after2ndThershold = 30;

        [Header("Enemy Prefab.")]
        public Enemy enemyPrefab;

        [Header("Wave Spawn Transform.")]
        public Transform[] spawnPositionTransform;

        [Header("Indicator Transform.")]
        public Transform spawnPosIndicatorTransform;

        [Header("UI")]
        public EnemyWaveUI enemyWaveUI;

        [Header("Refs.")]
        [ReadOnlyInspector] public Vector3 _cur_spawnPosition;
        [ReadOnlyInspector] public Transform _HQ_BuildingTransform;
        [ReadOnlyInspector] public LayerManager _layerManager;
        
        #region Callbacks.
        public static AISessionManager singleton;
        private void Awake()
        {
            #region Init Singleton.
            if (singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
            }
            #endregion
        }

        private void Update()
        {
            Tick();
        }
        #endregion

        #region Tick.
        void Tick()
        {
            MonitorNewWave();

            MonitorEnemySpawning();

            enemyWaveUI.Tick();
        }
        #endregion

        #region Start New Wave.
        void MonitorNewWave()
        {
            if (!isPausingWaveTimer)
            {
                newWaveTimer -= Time.deltaTime;
                if (newWaveTimer < 0)
                {
                    StartNewWave();
                }

                UpdateNextWaveSecondsMessage();
            }
        }

        void StartNewWave()
        {
            remainingEnemySpawnAmount = baseEnemyCount + enemyCountPerWave * waveNumber; //remainingEnemySpawnAmount = 50;
            waveNumber++;
            UpdateCurrentWaveNumberText();
            SetIsPausingWaveTimerToTrue();
        }
        #endregion

        #region Create Enemy
        void MonitorEnemySpawning()
        {
            if (isPausingWaveTimer)
            {
                // if we have enemies to be spawned.
                nextEnemySpawnTimer += Time.deltaTime;
                if (nextEnemySpawnTimer > nextEnemySpawnRate)
                {
                    // Randomize next spawn rate.
                    RandomizeEnemySpawnRateAndTimer();

                    // if the timer reached the rate
                    Create(_cur_spawnPosition + UtilsClass.GetRandomDir() * Random.Range(0f, 10f));
                    remainingEnemySpawnAmount--;

                    if (remainingEnemySpawnAmount == 0)
                    {
                        OnEnemySpawnFinished();
                    }
                }
            }
        }

        void RandomizeEnemySpawnRateAndTimer()
        {
            nextEnemySpawnRate = Random.Range(b_MinEnemySpawnRate, cur_maxEnemySpawnRate);
            nextEnemySpawnTimer = 0;
        }

        void Create(Vector3 position)
        {
            Enemy _enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            _enemy.Setup(this);
        }
        #endregion

        #region On Enemy Spawn Finished.
        void OnEnemySpawnFinished()
        {
            SetIsPausingWaveTimerToFalse();
            RandomizeEnemySpawnPosition();
            SetNewWaveWaitTimer();
            SetNewEnemySpawnRate();
            IncreaseEnemyBaseHealth();
        }

        void RandomizeEnemySpawnPosition()
        {
            _cur_spawnPosition = spawnPositionTransform[Random.Range(0, spawnPositionTransform.Length)].position;
            spawnPosIndicatorTransform.position = _cur_spawnPosition;
        }

        void SetNewWaveWaitTimer()
        {
            newWaveTimer = b_waitRate - (waveNumber * waitDepleteRate);
            newWaveTimer = newWaveTimer < least_WaitRate ? least_WaitRate : newWaveTimer;
        }

        void SetNewEnemySpawnRate()
        {
            cur_maxEnemySpawnRate = b_MaxEnemySpawnRate - (waveNumber * maxSpawnRateDepleteRate);
            cur_maxEnemySpawnRate = cur_maxEnemySpawnRate < least_MaxEnemySpawnRate ? least_MaxEnemySpawnRate : cur_maxEnemySpawnRate;
        }

        void IncreaseEnemyBaseHealth()
        {
            if (waveNumber == fir_hp_increaseThershold)
            {
                b_hp = hp_Amt_after1stThershold;
            }
            else if (waveNumber == sec_hp_increaseThershold)
            {
                b_hp = hp_Amt_after2ndThershold;
            }
        }
        #endregion

        #region On Enemy Won.
        public void OnDied()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Set Wave Messages / Text.
        void UpdateNextWaveSecondsMessage()
        {
            enemyWaveUI.SetWaveMessageText(StrBuilderClass.Build_NextWaveSeconds(newWaveTimer));
        }
        
        void UpdateCurrentWaveNumberText()
        {
            enemyWaveUI.SetWaveNumberText(StrBuilderClass.Build_WaveNumberText(waveNumber));
        }
        #endregion
        
        #region Set Status.
        void SetIsPausingWaveTimerToTrue()
        {
            isPausingWaveTimer = true;
            enemyWaveUI.HideWaveMessageText();
        }

        void SetIsPausingWaveTimerToFalse()
        {
            isPausingWaveTimer = false;
            enemyWaveUI.ShowWaveMessageText();
        }
        #endregion

        #region Enemy Setup.
        public float GetRandomizedSearchRate()
        {
            return Random.Range(minSearchRate, maxSearchRate);
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupGetRefs();

            SetupWaveWaitRate();

            SetupGetFirstSpawnPosition();

            SetupSetWaveText();

            SetupEnemyWaveUI();
        }

        void SetupGetRefs()
        {
            _layerManager = LayerManager.singleton;
        }

        void SetupWaveWaitRate()
        {
            newWaveTimer = start_WaitRate;
            nextEnemySpawnTimer = 1;
        }

        void SetupEnemySpawnRate()
        {
            cur_maxEnemySpawnRate = b_MaxEnemySpawnRate;
        }

        void SetupGetFirstSpawnPosition()
        {
            RandomizeEnemySpawnPosition();
        }

        void SetupSetWaveText()
        {
            UpdateCurrentWaveNumberText();
        }

        void SetupEnemyWaveUI()
        {
            enemyWaveUI.Setup();
        }
        #endregion
    }
}