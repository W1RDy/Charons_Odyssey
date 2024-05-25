using UnityEngine;
using Zenject;

public abstract class SubscribableClass : ISubscribable
{
    private SubscribeController _subscribeController;

    [Inject]
    public void Construct(SubscribeController subscribeController)
    {
        _subscribeController = subscribeController;
        _subscribeController.AddSubscribable(this);
    }

    public abstract void Subscribe();
    public abstract void Unsubscribe();
}