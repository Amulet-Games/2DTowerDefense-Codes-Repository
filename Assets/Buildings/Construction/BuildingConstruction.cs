using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BuildingConstruction : MonoBehaviour
    {
        [Header("Drag and Drops.")]
        public BoxCollider2D constructorCol;
        public SpriteRenderer constructionSpriteRenderer;
        public ParticleSystem constructionParticle;

        [Header("Timers Status.")]
        [ReadOnlyInspector] public float _constructionTimer;

        [Header("Refs.")]
        [ReadOnlyInspector] public BuildingSO _constructionType;
        [ReadOnlyInspector] public BuildingManager _buildingManager;
        [ReadOnlyInspector] public Generator_BuildingSO _constructionGeneratorType;
        [ReadOnlyInspector] public Material _constructionMateral;

        int _progressPropertyID;

        private void Update()
        {
            _constructionTimer -= Time.deltaTime;
            if (_constructionTimer < 0f)
            {
                CreateBuilding();
                PlayBuildingPlacedSound();
                Destroy(gameObject);
            }

            UpdateProgressShader();
        }

        void CreateBuilding()
        {
            _buildingManager._temp_GeneratorType = _constructionGeneratorType;
            _constructionType.CreateBuilding_AfterConstruction(transform.position);
        }

        void PlayBuildingPlacedSound()
        {
            SoundManager.singleton.PlaySound_BuildingPlaced();
        }

        void UpdateProgressShader()
        {
            _constructionMateral.SetFloat(_progressPropertyID, GetConstructionTimerNormalized());
        }

        public float GetConstructionTimerNormalized()
        {
            return 1 - _constructionTimer / _constructionType.constructionTimerMax;
        }

        public void Setup()
        {
            // Set Status
            _buildingManager = BuildingManager.singleton;
            _constructionType = _buildingManager._activeBuildingType;
            _constructionGeneratorType = _buildingManager._activeGeneratorType;

            _constructionTimer = _constructionType.constructionTimerMax;
            constructionSpriteRenderer.sprite = _constructionType.build_Sprite;
            _constructionType.DepleteResources();

            // Shader.
            _constructionMateral = constructionSpriteRenderer.material;
            _progressPropertyID = Shader.PropertyToID("_Progress");

            // Collider.
            BoxCollider2D _collider2D = _constructionType.prefab.buildingCol;
            constructorCol.offset = _collider2D.offset;
            constructorCol.size = _collider2D.size;

            // Particles.
            constructionParticle.gameObject.SetActive(true);
        }
    }
}