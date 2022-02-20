using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class ResourceNearbyOverlay : MonoBehaviour
    {
        [Header("Overlay Refs (Drops).")]
        public SpriteRenderer overlayIconSpriteRenderer;
        public TMP_Text overlayText;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public BuildingGhost _referedGhost;
        [ReadOnlyInspector] public BuildingManager _buildingManager;

        #region Tick.
        public void UpdatePercentageText()
        {
            // Text
            overlayText.text = _buildingManager.GetActiveBuildingTypeGeneratePerc(_referedGhost.transform).ToString() + "%";
        }
        #endregion

        #region Show / Hide.
        public void Show()
        {
            gameObject.SetActive(true);

            // Sprite
            overlayIconSpriteRenderer.sprite = _buildingManager._activeGeneratorType.resourceSprite;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Setup.
        public void Setup(BuildingGhost buildingGhost)
        {
            _referedGhost = buildingGhost;
            _buildingManager = _referedGhost._buildingManager;

            Hide();
        }
        #endregion
    }
}