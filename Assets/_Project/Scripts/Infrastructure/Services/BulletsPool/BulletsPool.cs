using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : IBulletsPool
{
    private readonly IStaticDataService _staticData;

    private Dictionary<BulletType, Queue<Bullet>> _bullets;

    public BulletsPool(IStaticDataService staticData)
    {
        _bullets = new();
        _staticData = staticData;
    }

    public Bullet Get(BulletType type)
    {
        if (_bullets.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            Bullet bullet = queue.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        return CreateNew(type);
    }

    public void Return(Bullet bullet)
    {
        if (!_bullets.ContainsKey(bullet.Type))
            _bullets[bullet.Type] = new Queue<Bullet>();

        bullet.gameObject.SetActive(false);
        _bullets[bullet.Type].Enqueue(bullet);
    }

    private Bullet CreateNew(BulletType type)
    {
        if (!_bullets.ContainsKey(type))
            _bullets[type] = new Queue<Bullet>();

        Bullet bullet = Object.Instantiate(
    _staticData.GetBullet(type));

        bullet.Construct(this);

        bullet.gameObject.SetActive(true);

        return bullet;
    }
}
