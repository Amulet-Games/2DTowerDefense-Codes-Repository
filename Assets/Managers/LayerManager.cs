using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LayerManager : MonoBehaviour
    {
        [Header("Layer.")]
        public int buildingLayer;
        public int constructionLayer;

        [Header("LayerMask.")]
        public LayerMask buildingMask;
        public LayerMask constructionMask;
        public LayerMask enemyMask;

        public static LayerManager singleton;
        public void Awake()
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
    }
}