using UnityEngine;

public class MobileInputService : InputService
{
    public override Vector2 Axis =>
        SimpleInputAxis();

    public override bool IsShoot
    {
        get
        {
            if (Input.touchCount > 0)
            {
                Debug.Log("MobileInput");

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    ShootPosition = touch.position;
                    return true;
                }

                return false;
            }

            return false;
        }
    }

    protected override bool IsAttackStateButtonDown() =>
        AttackState();
}
