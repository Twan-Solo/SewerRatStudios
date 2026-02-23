/// <summary>
/// Interface for anything the player can interact with.
/// </summary>
public interface IInteractable
{
    void Interact();
    string GetPromptText();
}