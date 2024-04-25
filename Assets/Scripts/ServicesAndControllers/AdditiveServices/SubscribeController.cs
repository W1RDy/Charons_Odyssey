using System.Collections.Generic;
using UnityEngine;

public class SubscribeController : MonoBehaviour
{
    private List<ISubscribable> _subscribables;

    public void AddSubscribable(ISubscribable subscribable)
    {
        _subscribables.Add(subscribable);
    }

    private void UnsubscribeAll()
    {
        foreach (var subscribable in _subscribables)
        {
            subscribable.Unsubscribe();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeAll();
    }
}
