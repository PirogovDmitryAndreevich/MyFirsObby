using UnityEngine;

public class PauseService : IPauseService
{
    public bool IsPause { get; private set; }

    public void Pause()
    {
        Time.timeScale = 0;
        IsPause = true;
    }

    public void Run()
    {
        Time.timeScale = 1;
        IsPause = false;
    }
}
