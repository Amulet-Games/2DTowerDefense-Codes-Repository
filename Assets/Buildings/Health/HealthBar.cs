using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Bar Transform (Drops).")]
        public Transform barTransform;

        [Header("Bar Separator (Drops).")]
        public float barSize = 3f;
        public GameObject barSeparatorObj;
        public Transform barSeparatorContainerTrans;

        [Header("Refs.")]
        [ReadOnlyInspector] public HealthSystem _healthSystem;

        #region Update HealthBar.
        void UpdateBar()
        {
            barTransform.localScale = new Vector3(_healthSystem.GetNormalizedHp(), 1, 1);
        }

        void RefillHealthBar_Full()
        {
            barTransform.localScale = new Vector3(1, 1, 1);
        }
        #endregion

        #region Show / Hide HealthBar.
        void HideHealthBar()
        {
            gameObject.SetActive(false);
        }

        void ShowHealthBar()
        {
            gameObject.SetActive(true);
        }
        #endregion

        #region Show / Hide Bar Separator
        void HideBarSeparator()
        {
            barSeparatorObj.SetActive(false);
        }

        void ShowBarSeparator()
        {
            barSeparatorObj.SetActive(true);
        }

        void DuplicateSeparatorByHealthAmount()
        {
            Vector3 _tempVector3 = new Vector3(0, 0, 0);

            // Bar size is the width of the whole bar.
            int healthAmountPerSeparator = 10;
            float barOneHealthAmountSize = barSize / _healthSystem._temp_b_hp;
            int healthSeparatorCount = Mathf.FloorToInt(_healthSystem._temp_b_hp / healthAmountPerSeparator);

            for (int i = 1; i < healthSeparatorCount; i++)
            {
                GameObject newBarSeparatorObj = Instantiate(barSeparatorObj, barSeparatorContainerTrans);
                newBarSeparatorObj.SetActive(true);
                _tempVector3.x = i * barOneHealthAmountSize * healthAmountPerSeparator;
                newBarSeparatorObj.transform.localPosition = _tempVector3;
            }
        }
        #endregion

        #region Setup.
        public void Setup(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;

            //SetupHideHealthBar();

            SetupBarSeparator();

            SubscribileEvent_OnHeal();

            SubscribileEvent_OnDamaged();
        }

        void SetupHideHealthBar()
        {
            HideHealthBar();
        }

        void SetupBarSeparator()
        {
            HideBarSeparator();

            DuplicateSeparatorByHealthAmount();
        }

        void SubscribileEvent_OnHeal()
        {
            _healthSystem.OnHeal += HealthSystem_OnHeal;
        }

        void SubscribileEvent_OnDamaged()
        {
            _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }
        #endregion

        #region OnHeal Actions.
        void HealthSystem_OnHeal(object sender, System.EventArgs e)
        {
            HideHealthBar();
            RefillHealthBar_Full();
        }
        #endregion

        #region OnDamaged Actions.
        void HealthSystem_OnDamaged(object sender, System.EventArgs e)
        {
            ShowHealthBar();
            UpdateBar();
        }
        #endregion
    }
}