using UnityEngine;

public class StandaloneInputService : InputService
{
    public override Vector2 Axis
    {
        get
        {
            Vector2 axis = SimpleInputAxis();

            if (axis == Vector2.zero)
                axis = UnityAxis();

            return axis;
        }
    }

    public override bool IsShoot
    {
        get
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("StandaloneInput");
                ShootPosition = Input.mousePosition;
                return true;
            }

            return false;
        }
    }

    protected override bool IsAttackStateButtonDown()
    {
        if (AttackState())
            return true;
        else
            return Input.GetKeyDown(KeyCode.E);
    }

    private Vector2 UnityAxis() =>
        new Vector2
        (
            Input.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            Input.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );
}