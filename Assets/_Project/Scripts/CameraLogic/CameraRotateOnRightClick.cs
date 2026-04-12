using UnityEngine;
using Cinemachine;

public class CameraRotateOnRightClick : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _camera;

    private float _activeXSpeed;
    private float _activeYSpeed;

    private IInputService _inputService;

    private void Start()
    {
        _activeXSpeed = _camera.m_XAxis.m_MaxSpeed;
        _activeYSpeed = _camera.m_YAxis.m_MaxSpeed;
    }

    private void Update()
    {
        bool isRotating = _inputService.IsCameraMoving;

        _camera.m_XAxis.m_MaxSpeed = isRotating ? _activeXSpeed : 0f;
        _camera.m_YAxis.m_MaxSpeed = isRotating ? _activeYSpeed : 0f;
    }

    public void Construct(IInputService inputService) =>
        _inputService = inputService;
}