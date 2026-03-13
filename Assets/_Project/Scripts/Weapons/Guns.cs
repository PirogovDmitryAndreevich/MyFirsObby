using UnityEngine;

public abstract class Guns : MonoBehaviour
{
    [SerializeField] protected Transform Muzzle;
    [SerializeField] protected Bullet Bullet;
    [SerializeField] protected int MagazineCount;
    [SerializeField] protected float BulletForce = 500f;
    [SerializeField] protected int Damage = 5;
    [SerializeField] private float _speedShot = 5f;

    protected bool CanShot = true;

    public bool CanShotNow => CanShot;

    public abstract void Shot();

    public abstract void Reload();
}
