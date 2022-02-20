using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SA
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Cinemachine.")]
        public CinemachineVirtualCamera cinemachineVirtualCamera;

        [Header("Move Speed.")]
        public float camMinMoveSpeed = 26f;
        public float camMaxMoveSpeed = 50f;

        [Header("Zoom.")]
        public float zoomAmount = 2f;
        public float maxZoomOutSize = 30f;
        public float maxZoomInSize = 9f;

        [Header("Bounds.")]
        public float cam_l_bounds;
        public float cam_r_bounds;
        public float cam_u_bounds;
        public float cam_b_bounds;

        [Header("Edge Scrolling.")]
        public float edgeScrollingSize = 30;
        [ReadOnlyInspector] public bool _isEdgeScrollingEnabled;

        [Header("Status.")]
        [ReadOnlyInspector] public float cur_camMoveSpeed;
        [ReadOnlyInspector] public float cur_orthographicSize;
        [ReadOnlyInspector] public float tar_orthograpchSize;

        private void Start()
        {
            cur_orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
            tar_orthograpchSize = cur_orthographicSize;

            SessionManager _sessionManager = SessionManager.singleton;

            if (_sessionManager._hasExitFromGame)
            {
                _isEdgeScrollingEnabled = _sessionManager.saveProfile._isEdgeScrolling;
            }
            else
            {
                _isEdgeScrollingEnabled = false;
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        void HandleMovement()
        {
            float h;
            float v;
            Vector3 moveDir;

            MoveCameraByInput();
            MoveCameraByMouse();

            SetMoveDirFromCurrentValues();
            SetSpeedByCamOrthoSize();

            transform.position += moveDir * cur_camMoveSpeed * Time.deltaTime;
            ValidateCameraPosWithinBounds();

            void MoveCameraByInput()
            {
                h = Input.GetAxisRaw("Horizontal");
                v = Input.GetAxisRaw("Vertical");
            }

            void MoveCameraByMouse()
            {
                if (_isEdgeScrollingEnabled)
                {
                    if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
                    {
                        h += 1f;
                    }
                    else if (Input.mousePosition.x < edgeScrollingSize)
                    {
                        h -= 1f;
                    }

                    if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
                    {
                        v += 1f;
                    }
                    else if (Input.mousePosition.y < edgeScrollingSize)
                    {
                        v -= 1f;
                    }
                }
            }

            void SetMoveDirFromCurrentValues()
            {
                moveDir = new Vector2(h, v).normalized;
            }

            void SetSpeedByCamOrthoSize()
            {
                float t = cur_orthographicSize / maxZoomOutSize;
                cur_camMoveSpeed = (t * (camMaxMoveSpeed - camMinMoveSpeed)) + camMinMoveSpeed;
            }

            void ValidateCameraPosWithinBounds()
            {
                Vector3 _pos = transform.position;

                if (h != 0)
                    _pos.x = Mathf.Clamp(_pos.x, cam_l_bounds, cam_r_bounds);

                if (v != 0)
                    _pos.y = Mathf.Clamp(_pos.y, cam_b_bounds, cam_u_bounds);

                transform.position = _pos;
            }
        }

        void HandleZoom()
        {
            tar_orthograpchSize += Input.mouseScrollDelta.y * zoomAmount;

            cur_orthographicSize = Mathf.Lerp(cur_orthographicSize, tar_orthograpchSize, Time.deltaTime * 5);

            cur_orthographicSize = Mathf.Clamp(tar_orthograpchSize, maxZoomInSize, maxZoomOutSize);

            tar_orthograpchSize = cur_orthographicSize;
            cinemachineVirtualCamera.m_Lens.OrthographicSize = cur_orthographicSize;
        }

        public void SetEdgeScrolling()
        {
            _isEdgeScrollingEnabled = !_isEdgeScrollingEnabled;
        }
    }
}