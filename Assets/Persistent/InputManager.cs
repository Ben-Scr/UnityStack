using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BenScr.UnityStack
{
    /*
     * Custom adjustable global input manager for unities new input system
     */

    public class InputManager : MonoBehaviour
    {
        public static Action<bool> OnClick;
        public static Action OnMouseMove;

        public static bool isMouseDown;
        public static Vector2 pressWorldMousePosition;

        [SerializeField] private InputActionReference pressReference;
        [SerializeField] private InputActionReference releaseReference;

        [SerializeField] private float moveThreshold = 0.05f;

        private bool movedCursorSincePress;
        private Vector3 pressMousePosition;
        private Vector3 lastMousePos;

        private void Awake()
        {
            isMouseDown = false;
        }

        private void Update()
        {
            if (!movedCursorSincePress && Vector2.Distance(Input.mousePosition, pressMousePosition) > moveThreshold)
            {
                movedCursorSincePress = true;
            }

            if (isMouseDown)
            {
                OnClick?.Invoke(true);
            }

            Vector3 mousePosition = CameraUtility.GetMousePosition();

            if (mousePosition != lastMousePos)
            {
                OnMouseMove?.Invoke();
            }
        }

        private void OnEnable()
        {
            releaseReference.action.Enable();
            pressReference.action.Enable();

            releaseReference.action.performed += OnReleaseActionTriggered;
            pressReference.action.performed += OnClickActionTriggered;
        }

        private void OnDisable()
        {
            releaseReference.action.performed -= OnReleaseActionTriggered;
            pressReference.action.performed -= OnClickActionTriggered;

            pressReference.action.Disable();
            releaseReference.action.Disable();
        }

        private void OnClickActionTriggered(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            pressMousePosition = Input.mousePosition;
            pressWorldMousePosition = CameraUtility.GetMousePosition();
            movedCursorSincePress = false;
            isMouseDown = true;
        }

        private void OnReleaseActionTriggered(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            isMouseDown = false;

            if (!movedCursorSincePress)
            {
                OnClick?.Invoke(false);
            }
        }
    }
}
