using Hud;
using Injection;
using Inventory;
using Inventory.Hotbar;
using Inventory.ItemPlacement;
using PanelCore;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls
{
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private PanelService _panelService;
        
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _gravity = -9.81f;

        [Header("Look")]
        [SerializeField] private float _mouseSensitivity = 1f;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Camera _camera;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerInputActions _inputActions;

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector3 _velocity;
        private float _xRotation = 0f;
        
        //References
        private HotBarController _hotbar;
        private ItemPlacementController _itemPlacementController;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _inputActions = new PlayerInputActions();
            _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
            _inputActions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Look.canceled += ctx => _lookInput = Vector2.zero;
            _inputActions.Player.Jump.performed += ctx => Jump();
            _inputActions.Player.Inventory.performed += ctx => OpenInventory();
            _inputActions.Player.Scroll.performed += ctx => OnScroll(ctx.ReadValue<Vector2>().y);
            _inputActions.Player.Hotbar.performed += ctx => SelectSlot(ctx);
            _inputActions.Player.Fire.performed += ctx => LeftClickPerformed(ctx);
        }

        private void LeftClickPerformed(InputAction.CallbackContext ctx)
        {
            
        }

        private void Start()
        {
            _hotbar = ServiceLocator.Resolve<HotBarController>();
            _itemPlacementController = ServiceLocator.Resolve<ItemPlacementController>();
            
            _panelService.IsAnyPanelOpen
                .Subscribe(isOpen =>
                {
                    Cursor.lockState = isOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
                });
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
        }

        private void HandleMovement()
        {
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            var move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            _controller.Move(move * _moveSpeed * Time.deltaTime);

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            if (_panelService.IsAnyPanelOpen.Value) return;
            
            var mouseX = _lookInput.x * _mouseSensitivity;
            var mouseY = _lookInput.y * _mouseSensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void Jump()
        {
            if (_controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
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
            if (_panelService.IsAnyPanelOpen.Value) return;
            if (Mathf.Abs(scroll) < 0.1f) return;

            var direction = scroll > 0 ? -1 : 1;

            _hotbar.Scroll(direction);
        }

        private void SelectSlot(InputAction.CallbackContext slot)
        {
            if (!int.TryParse(slot.control.name, out var numKeyValue))
                return;

            _hotbar.SelectSlot(numKeyValue - 1);
        }
    }
}
