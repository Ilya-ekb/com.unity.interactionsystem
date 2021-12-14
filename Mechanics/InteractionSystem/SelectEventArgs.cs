using Mechanics.InteractionSystem.Interactables;
using Mechanics.InteractionSystem.Interactors;

namespace Mechanics.InteractionSystem
{
    public readonly struct SelectEventArgs : IInteractionEventArgs
    {
        public IInteractor Interactor { get; }
        public IInteractable Interactable { get; }
        public SelectEventArgs(IInteractor interactor, IInteractable interactable)
        {
            Interactor = interactor;
            Interactable = interactable;
        }
    }
}
