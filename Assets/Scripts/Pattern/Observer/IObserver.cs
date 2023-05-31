using UnityEngine;

public interface IGameObserver
{
    // Receive update from subject
    void Execute(IGameEvent gameEvent);
    void Execute(IGameEvent gameEvent, int val);
    void Execute(IGameEvent gameEvent, float val);
    void Execute(IGameEvent gameEvent, bool val);
    void Execute(IGameEvent gameEvent, RaycastHit hit);
    void Execute(IGameEvent gameEvent, Vector3 point, Vector3 normal);
    void RaiseUnityEvent();
}
