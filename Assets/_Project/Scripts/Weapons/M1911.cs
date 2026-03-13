using UnityEngine;

public class M1911 : Guns
{
    private int ammunition;

    private void OnEnable()
    {
        ammunition = MagazineCount;
    }

    public override void Shot()
    {
        if (ammunition == 0)
            return;

        ammunition--;
        var bullet = Instantiate(Bullet,Muzzle.position,Muzzle.rotation);

        Vector3 force = Muzzle.forward * BulletForce;

        bullet.Play(Muzzle.forward * BulletForce);
    }

    public override void Reload()
    {
        ammunition = MagazineCount;
    }    
}
