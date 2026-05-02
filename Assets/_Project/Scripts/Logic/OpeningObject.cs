using UnityEngine;

public class OpeningObject : MonoBehaviour, IOpenObject
{
    [SerializeField] private OpeningType _type;
    [SerializeField] private Opener _opener;

    public OpeningType Type => _type;
    public Opener Opener => _opener;
    public OpeningType[] NextOpeners { get; private set; }

    public void Construct(OpeningType[] next) =>
        NextOpeners = next;

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.BuildProgress.OpenedObjects.Contains(_type))
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void OpenObject() => 
        gameObject.SetActive(true);

    public void OpenOpener() => 
        _opener.gameObject.SetActive(true);
}
