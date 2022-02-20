using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ResourceManager : MonoBehaviour
    {
        [Header("Starter Amounts.")]
        public int starter_wood_Amt;
        public int starter_stone_Amt;
        public int starter_gold_Amt;

        [Header("Status.")]
        [ReadOnlyInspector] public int cur_woodAmt;
        [ReadOnlyInspector] public int cur_stoneAmt;
        [ReadOnlyInspector] public int cur_goldAmt;

        [Header("Refs.")]
        [ReadOnlyInspector] public MainHudManager _mainHudManager;
        
        public static ResourceManager singleton;
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

        #region Add Resources
        public void AddResource_Wood(int amount)
        {
            cur_woodAmt += amount;
            _mainHudManager.UpdateWoodText();
        }

        public void AddResource_Stone(int amount)
        {
            cur_stoneAmt += amount;
            _mainHudManager.UpdateStoneText();
        }

        public void AddResource_Gold(int amount)
        {
            cur_goldAmt += amount;
            _mainHudManager.UpdateGoldText();
        }
        #endregion

        #region Remove Resources
        public void RemoveResource_Wood(int amount)
        {
            cur_woodAmt -= amount;
            _mainHudManager.UpdateWoodText();
        }

        public void RemoveResource_Stone(int amount)
        {
            cur_stoneAmt -= amount;
            _mainHudManager.UpdateStoneText();
        }

        public void RemoveResource_Gold(int amount)
        {
            cur_goldAmt -= amount;
            _mainHudManager.UpdateGoldText();
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupGetRefs();
            SetupStarterResources();
        }

        void SetupGetRefs()
        {
            _mainHudManager = MainHudManager.singleton;
        }

        void SetupStarterResources()
        {
            cur_woodAmt = starter_wood_Amt;
            cur_stoneAmt = starter_stone_Amt;
            cur_goldAmt = starter_gold_Amt;
        }
        #endregion
    }
}