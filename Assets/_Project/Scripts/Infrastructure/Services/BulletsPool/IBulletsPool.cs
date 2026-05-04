public interface IBulletsPool : IService
{
    Bullet Get(BulletType type);
    void Return(Bullet bullet);
}