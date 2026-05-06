using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _camera;

    [SerializeField] private float _zoomSpeed = 2f;
    [SerializeField] private float _minDistance = 2f;
    [SerializeField] private float _maxDistance = 10f;

    private float _currentDistance;
    private IInputService _inputService;

    private void Start()
    {
        _currentDistance = _camera.m_Orbits[1].m_Radius;
    }

    private void Update()
    {
        float scroll = _inputService.ZoomInput;

        _currentDistance -= scroll * _zoomSpeed;
        _currentDistance = Mathf.Clamp(_currentDistance, _minDistance, _maxDistance);

        _camera.m_Orbits[1].m_Radius = _currentDistance;
        _camera.m_Orbits[0].m_Height = _currentDistance;

    }

    public void Construct(IInputService inputService) => 
        _inputService = inputService;
}