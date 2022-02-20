using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BuildingGhost : MonoBehaviour
    {
        [Header("Sprite (Drops).")]
        public GameObject spriteObj;
        public SpriteRenderer spriteRenderer;

        [Header("Overlay (Drops).")]
        public ResourceNearbyOverlay resourceNearbyOverlay;

        [Header("Status.")]
        [ReadOnlyInspector] public p_GhostTypeEnum _ghostType;

        [Header("Refs.")]
        [ReadOnlyInspector] public BuildingManager _buildingManager;

        #region Tick.
        public void Tick()
        {
            UpdateGhostByType();
        }
        
        void UpdateGhostPosition()
        {
            transform.position = UtilsClass.GetMouseWorldPosition();
        }

        void UpdateGhostByType()
        {
            switch (_ghostType)
            {
                case p_GhostTypeEnum.Generator:
                    UpdateGhostPosition();
                    resourceNearbyOverlay.UpdatePercentageText();
                    break;
                case p_GhostTypeEnum.Protector:
                    UpdateGhostPosition();
                    break;
                case p_GhostTypeEnum.Arrow:
                    break;
            }
        }
        #endregion

        #region Set.
        public void SetGhost_Arrow()
        {
            HideGhost();

            SetGhostType();

            HideNearbyOverlay();
            
            void SetGhostType()
            {
                _ghostType = p_GhostTypeEnum.Arrow;
            }

            void HideNearbyOverlay()
            {
                resourceNearbyOverlay.Hide();
            }
        }

        public void SetGhost_Generator()
        {
            BaseSetGhost();

            SetGhostType();

            ShowNearbyOverlay();

            void SetGhostType()
            {
                _ghostType = p_GhostTypeEnum.Generator;
            }

            void ShowNearbyOverlay()
            {
                resourceNearbyOverlay.Show();
            }
        }

        public void SetGhost_Protector()
        {
            BaseSetGhost();

            SetGhostType();

            HideNearbyOverlay();

            void SetGhostType()
            {
                _ghostType = p_GhostTypeEnum.Protector;
            }

            void HideNearbyOverlay()
            {
                resourceNearbyOverlay.Hide();
            }
        }

        void BaseSetGhost()
        {
            // Ghost.
            spriteObj.SetActive(true);
            spriteRenderer.sprite = _buildingManager._activeBuildingType.build_Sprite;
        }
        #endregion

        #region Show / Hide.
        public void ShowGhost()
        {
            spriteObj.SetActive(true);
        }

        public void HideGhost()
        {
            spriteObj.SetActive(false);
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            _buildingManager = BuildingManager.singleton;

            SetupOverlay();
        }

        void SetupOverlay()
        {
            resourceNearbyOverlay.Setup(this);
        }
        #endregion
    }
}