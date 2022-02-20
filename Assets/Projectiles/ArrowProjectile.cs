using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ArrowProjectile : MonoBehaviour
    {
        [Header("Status.")]
        public float moveSpeed = 20f;
        public float timeToDisappear = 2f;
        public int damage = 10;

        [Header("Refs.")]
        [ReadOnlyInspector] public Collider2D _cur_targetCollider;
        [ReadOnlyInspector] public Transform _cur_targetTransform;
        [ReadOnlyInspector] public Vector3 _lastMoveDir;

        #region Callbacks.
        private void Update()
        {
            if (_cur_targetCollider && !_cur_targetCollider.enabled)
                _cur_targetTransform = null;

            if (_cur_targetTransform != null)
            {
                // if targeting enemy sill exists.
                MoveTowardAIEnemy();
            }
            else
            {
                // Otherwise just keep going toward current aiming direction.
                MoveTowardAimingDirection();

                timeToDisappear -= Time.deltaTime;
                if (timeToDisappear < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy._healthSystem.Damage(damage);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Tick.
        void MoveTowardAIEnemy()
        {
            /// Position
            Vector3 moveDir = (_cur_targetTransform.position - transform.position).normalized;
            _lastMoveDir = moveDir;

            transform.position += moveDir * moveSpeed * Time.deltaTime;

            /// Rotation
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));
        }

        void MoveTowardAimingDirection()
        {
            /// Position
            transform.position += _lastMoveDir * moveSpeed * Time.deltaTime;
        }
        #endregion
    }
}