public interface IPauseService : IService
{
    bool IsPause { get; }

    void Pause();
    void Run();
}
