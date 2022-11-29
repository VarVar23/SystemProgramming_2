using UnityEngine;

public class PlayerCharacter4 : Character4
{
    [Range(0, 100)] [SerializeField] private int _health = 100;
    [Range(0.5f, 10)] [SerializeField] private float _movingSpeed = 8;
    [SerializeField] private float _acceliration = 3;
    private const float _gravity = -9.8f;

    private CharacterController _characterController;
    private MouseLook4 _mouseLook;
    private Vector3 _currentVelocity;
    protected FireAction4 _fireAction { get; set; }

    private void Initiate()
    {
        base.Initiate();
        _fireAction = gameObject.AddComponent<RayShooter4>();
        _fireAction.Reloading();
        _characterController = GetComponentInChildren<CharacterController>();
        _characterController ??= gameObject.AddComponent<CharacterController>();
        _mouseLook = GetComponentInChildren<MouseLook4>();
        _mouseLook ??= gameObject.AddComponent<MouseLook4>();
    }

    public override void Movement()
    {
        if(_mouseLook != null && _mouseLook.PlayerCamera != null)
        {
            _mouseLook.PlayerCamera.enabled = hasAuthority;
        }

        if(hasAuthority)
        {
            var moveX = Input.GetAxis("Horizontal") * _movingSpeed;
            var moveZ = Input.GetAxis("Vertical") * _movingSpeed;
            var movement = new Vector3(moveX, 0, moveZ);

            movement = Vector3.ClampMagnitude(movement, _movingSpeed);
            movement *= Time.deltaTime;

            if(Input.GetKey(KeyCode.LeftShift))
            {
                movement *= _acceliration;
            }

            movement.y = _gravity;
            movement = transform.TransformDirection(movement);

            _characterController.Move(movement);
            _mouseLook.Rotation();

            CmdUpdatePosition(transform.position);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, serverPosition, ref _currentVelocity, _movingSpeed * Time.deltaTime);
        }
    }

    private void Start()
    {
        Initiate();
    }

    private void OnGUI()
    {
        if (Camera.main == null) return;

        var info = $"Health: {_health}\nClip: -";
        var size = 12;
        var bulletCountSize = 50;
        var posX = Camera.main.pixelWidth / 2 - size / 4;
        var posY = Camera.main.pixelHeight / 2 - size / 2;
        var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
        var posYBul = Camera.main.pixelHeight - bulletCountSize;
        GUI.Label(new Rect(posX, posY, size, size), "+");
        GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2, bulletCountSize * 2), info);
    }
}
