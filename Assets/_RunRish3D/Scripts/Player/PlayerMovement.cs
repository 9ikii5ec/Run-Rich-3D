using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.Splines;

namespace ButchersGames
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Spline")]
        [SerializeField] private SplineContainer splineContainer;

        [Header("Forward")]
        [SerializeField] private float forwardSpeed = 10f;

        [Header("Side")]
        [SerializeField] private float roadWidth = 3.5f;

        [Header("Events")]
        [SerializeField] private GameEvents gameEvents;

        private bool _isGameStarted;
        private bool _isHolding;
        private Vector2 _touchStartPosition;
        private float _sideOffset;
        private float _sideOffsetAtTouchStart;
        private float _splineProgress;
        private float _distanceTraveled;

        private InputAction _pointerPressAction;
        private InputAction _pointerPositionAction;

        private void Awake()
        {
            _pointerPressAction = new InputAction(binding: "<Pointer>/press");
            _pointerPositionAction = new InputAction(binding: "<Pointer>/position");

            _pointerPressAction.performed += ctx => OnPointerDown();
            _pointerPressAction.canceled += ctx => OnPointerUp();
        }

        private void Start()
        {
            if (splineContainer == null) return;

            splineContainer.Spline.Evaluate(0f, out float3 pos, out float3 tan, out _);
            Transform st = splineContainer.transform;
            transform.position = st.TransformPoint(new Vector3(pos.x, pos.y, pos.z));
            transform.rotation = Quaternion.LookRotation(st.TransformDirection(new Vector3(tan.x, tan.y, tan.z)), Vector3.up);
            _splineProgress = 0f;
        }

        private void OnEnable()
        {
            _pointerPressAction.Enable();
            _pointerPositionAction.Enable();
        }

        private void OnDisable()
        {
            _pointerPressAction.Disable();
            _pointerPositionAction.Disable();
        }

        private void OnDestroy()
        {
            _pointerPressAction.performed -= ctx => OnPointerDown();
            _pointerPressAction.canceled -= ctx => OnPointerUp();
        }

        private void OnPointerDown()
        {
            Vector2 pos = _pointerPositionAction.ReadValue<Vector2>();

            if (!_isGameStarted)
            {
                _isGameStarted = true;
                _touchStartPosition = pos;
                _isHolding = true;
                InitSplineProgress();
                gameEvents?.GameStarted();
                return;
            }

            _isHolding = true;
            _touchStartPosition = pos;
            _sideOffsetAtTouchStart = _sideOffset;
        }

        private void InitSplineProgress()
        {
            if (splineContainer == null) return;

            float closestT = 0f;
            float closestDist = float.MaxValue;
            Vector3 playerPos = transform.position;

            for (int i = 0; i <= 100; i++)
            {
                float t = i / 100f;
                splineContainer.Spline.Evaluate(t, out float3 p, out _, out _);
                float dist = Vector3.Distance(playerPos, new Vector3(p.x, p.y, p.z));
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestT = t;
                }
            }

            _splineProgress = closestT;
        }

        private void OnPointerUp()
        {
            _isHolding = false;
        }

        private void Update()
        {
            if (splineContainer == null) return;
            if (!_isGameStarted) return;

            HandleInput();
            MoveForward();
            ApplyPosition();
        }

        private void HandleInput()
        {
            if (!_isHolding) return;

            Vector2 currentPos = _pointerPositionAction.ReadValue<Vector2>();
            float deltaX = currentPos.x - _touchStartPosition.x;
            _sideOffset = _sideOffsetAtTouchStart + deltaX / (Screen.width * 0.5f) * roadWidth;
            _sideOffset = Mathf.Clamp(_sideOffset, -roadWidth, roadWidth);
        }

        private void MoveForward()
        {
            float splineLength = splineContainer.CalculateLength();
            float prevProgress = _splineProgress;
            _splineProgress += (forwardSpeed * Time.deltaTime) / splineLength;

            if (_splineProgress >= 1f)
            {
                _splineProgress = 1f;
            }

            _distanceTraveled += (_splineProgress - prevProgress) * splineLength;
            gameEvents?.DistanceChanged((int)_distanceTraveled);
        }

        private void ApplyPosition()
        {
            splineContainer.Spline.Evaluate(
                _splineProgress,
                out float3 position,
                out float3 tangent,
                out float3 up
            );

            Transform st = splineContainer.transform;
            Vector3 splinePos = st.TransformPoint(new Vector3(position.x, position.y, position.z));
            Vector3 splineTangent = st.TransformDirection(new Vector3(tangent.x, tangent.y, tangent.z)).normalized;
            Vector3 splineRight = Vector3.Cross(Vector3.up, splineTangent).normalized;

            transform.position = splinePos + splineRight * _sideOffset;
            transform.rotation = Quaternion.LookRotation(splineTangent, Vector3.up);

            Physics.SyncTransforms();
        }

        public void StopRunning()
        {
            _isGameStarted = false;
        }
    }
}
