using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HealthSystem : MonoBehaviour
    {
        [Header("Health Bar (Drops).")]
        public HealthBar healthBar;

        [Header("Health.")]
        [ReadOnlyInspector] public int _temp_b_hp;
        [ReadOnlyInspector] public int _cur_hp;
        [ReadOnlyInspector] public bool _isDead;

        public event EventHandler OnHeal;
        public event EventHandler OnDamaged;
        public event EventHandler OnDied;

        public void Damage(int damageAmount)
        {
            _cur_hp -= damageAmount;
            _cur_hp = Mathf.Clamp(_cur_hp, 0, _temp_b_hp);

            OnDamaged?.Invoke(this, EventArgs.Empty);

            if (_cur_hp == 0)
            {
                _isDead = true;
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(int healAmount)
        {
            _cur_hp += healAmount;
            _cur_hp = _cur_hp > _temp_b_hp ? _temp_b_hp : _cur_hp;

            OnHeal?.Invoke(this, EventArgs.Empty);
        }

        public void HealFull()
        {
            _cur_hp = _temp_b_hp;

            OnHeal?.Invoke(this, EventArgs.Empty);
        }

        public float GetNormalizedHp()
        {
            return (float)_cur_hp / _temp_b_hp;
        }

        #region Setup.
        public void Setup()
        {
            SetupResetStatus();

            SetupHealthBar();
        }

        void SetupResetStatus()
        {
            _cur_hp = _temp_b_hp;
            _isDead = false;
        }

        void SetupHealthBar()
        {
            healthBar.Setup(this);
        }
        #endregion
    }
}