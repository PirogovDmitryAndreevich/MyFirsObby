using UnityEngine;
using UnityEngine.EventSystems;

public class InputReader : MonoBehaviour
{
    // Start is called before the first frame update

    private float _movementX;
    private float _movementY;

    public Vector2 Direction { get; private set; }

    private void Update()
    {
        _movementX = Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis);
        _movementY = Input.GetAxis(ConstantsData.InputAxis.VerticalAxis);

        Direction = new Vector2(_movementX, _movementY);
    }
}
