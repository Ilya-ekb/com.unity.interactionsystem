using Mechanics.InteractionSystem.Interactables;
using Mechanics.InteractionSystem.Interactors;

namespace Mechanics.InteractionSystem
{
    public interface IInteractionEventArgs
    {
        IInteractor Interactor { get; }

        IInteractable Interactable { get; }
    }
}
