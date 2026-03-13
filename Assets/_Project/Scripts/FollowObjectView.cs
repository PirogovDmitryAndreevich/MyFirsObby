using UnityEngine;

public class FollowObjectView : MonoBehaviour
{
    [SerializeField] private Transform _transformView;

    private void Update()
    {
        transform.position = _transformView.position;
    }
}
