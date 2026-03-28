public interface IInteractable
{
    void SetInteractable(IInteractObject interact);
}

public interface IInteractObject
{
    void Interact();
}