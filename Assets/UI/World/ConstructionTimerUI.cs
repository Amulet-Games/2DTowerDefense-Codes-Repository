using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class ConstructionTimerUI : MonoBehaviour
    {
        [Header("Building Construction (Drops).")]
        public BuildingConstruction buildingConstruction;

        [Header("Image (Drops).")]
        public Image progressImage;

        private void Update()
        {
            progressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalized();
        }
    }
}