using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class BuildingManager : MonoBehaviour
    {
        [Header("HQ Building Type (Drops).")]
        public Hybird_BuildingSO hqBuildingType;

        [Header("Building Ghost.")]
        public BuildingGhost buildingGhost;

        [Header("Building Construction.")]
        public BuildingConstruction buildingConstructionPrefab;

        [Header("Particles (Drops).")]
        public ParticleSystem buildingDestroyedParticle;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isNoBuildingSelected;

        [Header("Active Type.")]
        [ReadOnlyInspector] public BuildingSO _activeBuildingType;
        [ReadOnlyInspector] public Generator_BuildingSO _activeGeneratorType;
        [ReadOnlyInspector] public Protector_BuildingSO _activeProtectorType;

        [Header("Refs.")]
        [ReadOnlyInspector] public ResourceManager _resourceManager;
        [ReadOnlyInspector] public MainHudManager _mainHudManager;
        [ReadOnlyInspector] public TooltipHandler _tooltipHandler;
        [ReadOnlyInspector] public LayerManager _layerManager;

        #region Privates.

        #region Temps.
        [HideInInspector] public Collider2D[] foundResources;
        [HideInInspector] public int nearbyResourceAmount;
        [HideInInspector] public Generator_BuildingSO _temp_GeneratorType;
        #endregion

        #endregion

        #region Callbacks.
        public static BuildingManager singleton;
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

        private void Start()
        {
            Setup();

            UtilsClass.Setup();

            StrBuilderClass.Setup();

            _resourceManager.Setup();

            _mainHudManager.Setup();

            AISessionManager.singleton.Setup();
            
            LateSetup_SetActiveBuildingType();
        }

        private void Update()
        {
            Tick();
        }
        #endregion

        #region Tick.
        void Tick()
        {
            InstantiateBuildingByMouse();

            buildingGhost.Tick();

            _tooltipHandler.Tick();
        }

        void InstantiateBuildingByMouse()
        {
            // if one of the building type is selected
            if (!isNoBuildingSelected)
            {
                // if player pressed mouse button when cursor is on the map.
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    // if the player has the cost to build it.
                    if (CanAffordToBuild())
                    {
                        // if the area to be placed on don't have other objects and it's far enough to the same type of building.
                        if (CanSpawnBuilding(_activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                        {
                            //CreateBuilding();
                            CreateConstruction();
                            PlayBuildingPlacedSound();
                        }
                        else
                        {
                            _tooltipHandler.Show(errorMessage, true);
                        }
                    }
                    else
                    {
                        _tooltipHandler.Show(StrBuilderClass.Build_CannotAffordCost(_activeBuildingType), true);
                    }
                }
            }
        }

        bool CanAffordToBuild()
        {
            ResourceCost[] _costArray = _activeBuildingType.costArray;
            for (int i = 0; i < _costArray.Length; i++)
            {
                if (!_costArray[i].CheckIsEnoughResource())
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Building Construction.
        void CreateConstruction()
        {
            BuildingConstruction _construction = Instantiate(buildingConstructionPrefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
            _construction.Setup();
        }

        void PlayBuildingPlacedSound()
        {
            SoundManager.singleton.PlaySound_BuildingPlaced();
        }
        #endregion

        #region Set Active Building Type.
        public void SetActiveBuildingType_Arrow()
        {
            _mainHudManager.SetSelectImage_Arrow();

            OnSelectArrow();
        }

        public void SetActiveBuildingType_Wood()
        {
            _mainHudManager.SetSelectImage_Wood();

            OnSelectGenerator();
        }

        public void SetActiveBuildingType_Stone()
        {
            _mainHudManager.SetSelectImage_Stone();

            OnSelectGenerator();
        }

        public void SetActiveBuildingType_Gold()
        {
            _mainHudManager.SetSelectImage_Gold();

            OnSelectGenerator();
        }

        public void SetActiveBuildingType_Tower()
        {
            _mainHudManager.SetSelectImage_Tower();

            OnSelectProtector();
        }

        void OnSelectArrow()
        {
            isNoBuildingSelected = true;
            buildingGhost.SetGhost_Arrow();
        }

        void OnSelectGenerator()
        {
            isNoBuildingSelected = false;
            buildingGhost.SetGhost_Generator();
        }

        void OnSelectProtector()
        {
            isNoBuildingSelected = false;
            buildingGhost.SetGhost_Protector();
        }
        #endregion

        #region Check Can Spawn Building.
        bool CanSpawnBuilding(BuildingSO buildingSO, Vector3 position, out string errorMessage)
        {
            BoxCollider2D boxCollider2D = buildingSO.prefab.buildingCol;
            Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

            // If the area is clear, meaning no buildings / resources.
            if (collider2DArray.Length == 0)
            {
                #region Limit building to be spawned when too close to each other.
                // Collliders inside the construction radius
                collider2DArray = Physics2D.OverlapCircleAll(position, buildingSO.minConstructionRadius, _layerManager.constructionMask);
                for (int i = 0; i < collider2DArray.Length; i++)
                {
                    if (collider2DArray[i].gameObject.layer == _layerManager.buildingLayer)
                    {
                        #region Get Component on Base Building.
                        // Theres already a building of this type within the construction radius!
                        if (buildingSO.buildingType == collider2DArray[i].GetComponent<Base_Building>().GetBuildingType())
                        {
                            errorMessage = "Too close to another building of the same type!";
                            return false;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Get Component on Building Construction.
                        // Theres already a building of this type within the construction radius!
                        if (buildingSO.buildingType == collider2DArray[i].GetComponent<BuildingConstruction>()._constructionType.buildingType)
                        {
                            errorMessage = "Too close to another building of the same type!";
                            return false;
                        }
                        #endregion
                    }
                }
                #endregion

                #region Limit building to be spawned in far distance.
                float maxConstructionRadius = 25f;
                collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius, _layerManager.constructionMask);
                if (collider2DArray.Length > 0)
                {
                    errorMessage = "";
                    return true;
                }
                else
                {
                    errorMessage = "Too far from any other building!";
                    return false;
                }
                #endregion
            }
            else
            {
                errorMessage = "Area is not clear!";
                return false;
            }
        }
        #endregion

        #region Set Building's Generate Rate.
        public void FindAllResourcesNearyby(Generator_BuildingSO _generatorBuildingType, Transform _buildingTransform)
        {
            foundResources = Physics2D.OverlapCircleAll(_buildingTransform.position, _generatorBuildingType.generateRadius);

            RefreshNearbyResourceAmountByType();

            void RefreshNearbyResourceAmountByType()
            {
                nearbyResourceAmount = 0;

                switch (_generatorBuildingType.generatorType)
                {
                    case p_GeneratorTypeEnum.Wood_Gen:
                    case p_GeneratorTypeEnum.HQ:

                        #region Count Wood Resources.
                        for (int i = 0; i < foundResources.Length; i++)
                        {
                            WoodResourceNodes resourceNode = foundResources[i].GetComponent<WoodResourceNodes>();
                            if (resourceNode != null)
                            {
                                nearbyResourceAmount++;
                            }
                        }
                        #endregion
                        break;

                    case p_GeneratorTypeEnum.Stone_Gen:

                        #region Count Stone Resources.
                        for (int i = 0; i < foundResources.Length; i++)
                        {
                            StoneResourceNodes resourceNode = foundResources[i].GetComponent<StoneResourceNodes>();
                            if (resourceNode != null)
                            {
                                nearbyResourceAmount++;
                            }
                        }
                        #endregion
                        break;

                    case p_GeneratorTypeEnum.Gold_Gen:

                        #region Count Gold Resources.
                        for (int i = 0; i < foundResources.Length; i++)
                        {
                            GoldResourceNodes resourceNode = foundResources[i].GetComponent<GoldResourceNodes>();
                            if (resourceNode != null)
                            {
                                nearbyResourceAmount++;
                            }
                        }
                        #endregion
                        break;
                }
            }
        }

        public void FindAllResourcesNearyby_Hybird(Hybird_BuildingSO _hybirdBuildingType, Transform _buildingTransform)
        {
            foundResources = Physics2D.OverlapCircleAll(_buildingTransform.position, _hybirdBuildingType.generateRadius);

            RefreshNearbyResourceAmountByType();

            void RefreshNearbyResourceAmountByType()
            {
                nearbyResourceAmount = 0;

                switch (_hybirdBuildingType.generatorType)
                {
                    case p_GeneratorTypeEnum.Wood_Gen:
                    case p_GeneratorTypeEnum.HQ:

                        #region Count Wood Resources.
                        for (int i = 0; i < foundResources.Length; i++)
                        {
                            WoodResourceNodes resourceNode = foundResources[i].GetComponent<WoodResourceNodes>();
                            if (resourceNode != null)
                            {
                                nearbyResourceAmount++;
                            }
                        }
                        #endregion
                        break;

                    case p_GeneratorTypeEnum.Stone_Gen:

                        #region Count Stone Resources.
                        for (int i = 0; i < foundResources.Length; i++)
                        {
                            StoneResourceNodes resourceNode = foundResources[i].GetComponent<StoneResourceNodes>();
                            if (resourceNode != null)
                            {
                                nearbyResourceAmount++;
                            }
                        }
                        #endregion
                        break;

                    case p_GeneratorTypeEnum.Gold_Gen:

                        #region Count Gold Resources.
                        for (int i = 0; i < foundResources.Length; i++)
                        {
                            GoldResourceNodes resourceNode = foundResources[i].GetComponent<GoldResourceNodes>();
                            if (resourceNode != null)
                            {
                                nearbyResourceAmount++;
                            }
                        }
                        #endregion
                        break;
                }
            }
        }

        public float GetActiveBuildingTypeGeneratePerc(Transform _ghostTransform)
        {
            FindAllResourcesNearyby(_activeGeneratorType, _ghostTransform);

            return Mathf.RoundToInt((float)nearbyResourceAmount / _activeGeneratorType.maxGenRateAmount * 100f);
        }
        #endregion

        #region Setup.
        void Setup()
        {
            SetupRefs();
            SetupCreateHQ();
            SetupGhost();
        }

        void SetupRefs()
        {
            _resourceManager = ResourceManager.singleton;

            _mainHudManager = MainHudManager.singleton;
            _mainHudManager._buildingManager = this;

            _tooltipHandler = TooltipHandler.singleton;

            _layerManager = LayerManager.singleton;
        }

        void SetupCreateHQ()
        {
            hqBuildingType.CreateBuilding_immediate();
        }

        void SetupGhost()
        {
            buildingGhost.Setup();
        }

        void LateSetup_SetActiveBuildingType()
        {
            SetActiveBuildingType_Arrow();
        }
        #endregion

        #region On Died.
        public void OnDied()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}