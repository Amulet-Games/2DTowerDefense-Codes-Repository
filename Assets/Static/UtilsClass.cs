using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UtilsClass
    {
        [Header("Camera (Drops).")]
        [ReadOnlyInspector, SerializeField] static Camera _mainCamera;

        public static void Setup()
        {
            _mainCamera = Camera.main;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            return mouseWorldPos;
        }

        public static Vector3 GetRandomDir()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        public static float GetAngleFromVector(Vector3 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }
    }
}