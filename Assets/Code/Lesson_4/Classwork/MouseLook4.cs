using UnityEngine;
using UnityEngine.Networking;

public class MouseLook4 : NetworkBehaviour
{
    public Camera PlayerCamera => _camera;
    private Camera _camera;

    [Range(0.1f, 10)] [SerializeField] private float _sensitivity = 2;
    [Range(-90, 0)] [SerializeField] private float _minVert = -45;
    [Range(0, 90)] [SerializeField] private float _maxVert = 45;

    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        var rigidbody = GetComponentInChildren<Rigidbody>();

        if (rigidbody != null) rigidbody.freezeRotation = true;
    }

    public void Rotation()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * _sensitivity;
        _rotationY += Input.GetAxis("Mouse X") * _sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);
        transform.rotation = Quaternion.Euler(0, _rotationY, 0);
        _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    }
}
