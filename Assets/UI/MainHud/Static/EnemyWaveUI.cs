using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class EnemyWaveUI : MonoBehaviour
    {
        [Header("Texts (Drops.)")]
        public TMP_Text waveNumberText;
        public TMP_Text waveMessageText;
        public RectTransform waveDirImageRect;
        public RectTransform closetEnemyImageRect;

        [Header("Update Closet Enemy Rate.")]
        public float updateClosetEnemyRate;
        [ReadOnlyInspector] public float updateClosetEnemyTimer;

        [Header("Refs.")]
        [ReadOnlyInspector] public Camera _mainCamera;
        [ReadOnlyInspector] public AISessionManager _aiSessionManager;

        #region Tick.
        public void Tick()
        {
            UpdateWaveDirRotation();

            MonitorClosetEnemyTimer();
        }
        #endregion

        #region Wave Dircection Image.
        void UpdateWaveDirRotation()
        {
            /// Modiflying the indicator's Position & Rotation
            Vector3 dirToNextSpawnPosition = (_aiSessionManager._cur_spawnPosition - _mainCamera.transform.position).normalized;

            waveDirImageRect.anchoredPosition = dirToNextSpawnPosition * 300f;
            waveDirImageRect.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));

            /// Modiflying the indicator's Visibility.
            float disToNextSpawnPosition = Vector3.SqrMagnitude(_aiSessionManager._cur_spawnPosition - _mainCamera.transform.position);
            waveDirImageRect.gameObject.SetActive(disToNextSpawnPosition > (_mainCamera.orthographicSize * _mainCamera.orthographicSize * 3f));
        }

        void MonitorClosetEnemyTimer()
        {
            updateClosetEnemyTimer += Time.deltaTime;
            if (updateClosetEnemyTimer > updateClosetEnemyRate)
            {
                updateClosetEnemyTimer = 0;
                UpdateClosetEnemyRotation();
            }
        }

        void UpdateClosetEnemyRotation()
        {
            Vector3 _closetEnemyPos;

            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(_mainCamera.transform.position, 9999f, _aiSessionManager._layerManager.enemyMask);

            int foundLength = collider2DArray.Length;
            if (foundLength > 0)
            {
                FindClosetTarget();
                UpdateRotationBaseOnTarget();
            }
            else
            {
                NoTargetDeactivateImage();
            }
            
            void FindClosetTarget()
            {
                float _closetDistance = 100000000;
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

                _closetEnemyPos = _closetBuildingCol.transform.position;
            }

            void UpdateRotationBaseOnTarget()
            {
                /// Modiflying the indicator's Position & Rotation
                Vector3 dirToClosetEnemy = (_closetEnemyPos - _mainCamera.transform.position).normalized;

                closetEnemyImageRect.anchoredPosition = dirToClosetEnemy * 250f;
                closetEnemyImageRect.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosetEnemy));

                /// Modiflying the indicator's Visibility.
                float disToClosetEnemy = Vector3.SqrMagnitude(_closetEnemyPos - _mainCamera.transform.position);
                closetEnemyImageRect.gameObject.SetActive(disToClosetEnemy > (_mainCamera.orthographicSize * _mainCamera.orthographicSize * 2f));
            }

            void NoTargetDeactivateImage()
            {
                closetEnemyImageRect.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Message Text.
        public void SetWaveMessageText(string message)
        {
            waveMessageText.text = message;
        }

        public void SetWaveNumberText(string text)
        {
            waveNumberText.text = text;
        }

        public void ShowWaveMessageText()
        {
            waveMessageText.gameObject.SetActive(true);
        }

        public void HideWaveMessageText()
        {
            waveMessageText.gameObject.SetActive(false);
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupGetRefs();

            SetupGetMainCamera();
        }

        void SetupGetRefs()
        {
            _aiSessionManager = AISessionManager.singleton;
        }

        void SetupGetMainCamera()
        {
            _mainCamera = Camera.main;
        }
        #endregion
    }
}