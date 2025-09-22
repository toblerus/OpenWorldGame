using Hud;
using Injection;
using Inventory;
using Inventory.Hotbar;
using PanelCore;
using UnityEngine;

namespace PlayerControls
{
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private PanelService _panelService;
        
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;

        [Header("Look")]
        public float mouseSensitivity = 1f;
        public Transform cameraTransform;

        private CharacterController controller;
        private PlayerInputActions inputActions;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 velocity;
        private float xRotation = 0f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            inputActions = new PlayerInputActions();
            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
            inputActions.Player.Jump.performed += ctx => Jump();
            inputActions.Player.Inventory.performed += ctx => OpenInventory();
            inputActions.Player.Scroll.performed += ctx => OnScroll(ctx.ReadValue<Vector2>().y);
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
        }

        private void HandleMovement()
        {
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            var move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(move * moveSpeed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            if (_panelService.IsPanelOpen<InventoryPanelView>()) return;
            
            var mouseX = lookInput.x * mouseSensitivity;
            var mouseY = lookInput.y * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void Jump()
        {
            if (controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        private void OpenInventory()
        {
            if (_panelService.IsPanelOpen<InventoryPanelView>())
            {
                _panelService.ClosePanel<InventoryPanelView>();
            }
            else
            {
                _panelService.OpenPanel<InventoryPanelView>();
            }
        }
        
        private void OnScroll(float scroll)
        {
            if (_panelService.IsAnyPanelOpen()) return;
            if (Mathf.Abs(scroll) < 0.1f) return;

            var direction = scroll > 0 ? -1 : 1;

            var hotbar = ServiceLocator.Resolve<HotBarController>();
            hotbar.Scroll(direction);
        }

    }
}
