using System;
using UnityEngine;

public class Guns : MonoBehaviour
{
    [Header("Gun setting")]
    [SerializeField] protected Transform Muzzle;

    [Header("Bullet setting")]
    [SerializeField] protected Bullet Bullet;
    [SerializeField] protected float BulletForce = 500f;    

    [Header("Gun stats")]
    [SerializeField] protected int MagazineSize;
    [SerializeField] protected int Damage = 5;
    [SerializeField] protected float SpeedShot = 5f;
    [SerializeField] protected float Spread = 0f;

    protected bool CanShot = true;
    protected int Ammunition;

    public event Action AmmunitionChanged;

    public bool CanShotNow => CanShot;
    public int CurrentAmmunition
    {
        get => Ammunition;
        set
        {
            if (Ammunition != value)
            {
                Ammunition = value;
                AmmunitionChanged?.Invoke();
            }
        }
    }
    public int CurrentMagazineSize => MagazineSize;

    private void Awake()
    {
        CurrentAmmunition = MagazineSize;
    }

    public virtual void Shoot()
    {
        if(CurrentAmmunition <= 0)
        {
            Reload();
            return;
        }

        CurrentAmmunition--;
        var bullet = Instantiate(Bullet, Muzzle.position, Muzzle.rotation);

        float x = UnityEngine.Random.Range(-Spread, Spread);
        float y = UnityEngine.Random.Range(-Spread, Spread);

        Vector3 directionWithSpread = Muzzle.forward + new Vector3(x, y, 0);

        Vector3 force = directionWithSpread * BulletForce;

        bullet.Shot(force);
    }

    protected virtual void Reload()
    {
        CanShot = false;
        CurrentAmmunition = MagazineSize;
        CanShot = true;
    }
}
