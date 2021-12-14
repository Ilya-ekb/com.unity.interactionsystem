using Mechanics.InteractionSystem.Interactables;
using Mechanics.InteractionSystem.Interactors;

namespace Mechanics.InteractionSystem
{
    public readonly struct DeselectEventArgs : IInteractionEventArgs
    {
        public IInteractor Interactor { get; }
        public IInteractable Interactable { get; }

        public DeselectEventArgs(IInteractor interactor, IInteractable interactable)
        {
            Interactor = interactor;
            Interactable = interactable;
        }
    }
}
