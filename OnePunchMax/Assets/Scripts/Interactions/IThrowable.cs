namespace Interactions
{
    public interface IThrowable : IInteractable
    {
        public void OnThrown(IThrower thrower);
    }
}