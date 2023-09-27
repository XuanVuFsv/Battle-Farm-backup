using System.Collections;
using System;
public interface IGameEvent
{
    string GameEventName
    {
        get;
        set;
    }

    // Attach an observer to the subject.
    void Subscribe(IGameObserver observer);

    // Detach an observer from the subject.
    void UnSubscribe(IGameObserver observer);

    // Notify all observers about an event.
    void Notify();
}