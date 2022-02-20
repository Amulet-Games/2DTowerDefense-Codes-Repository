using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SpritePositionSortingOrder : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        [Header("Precision.")]
        public float positionOffsetY;

        private void Start()
        {
            RefreshSortingOrder();
        }

        void RefreshSortingOrder()
        {
            ///* 5 is the precision detail amount.
            /// The higher the value is, the more precise.
            spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffsetY) * 5);
        }
    }
}