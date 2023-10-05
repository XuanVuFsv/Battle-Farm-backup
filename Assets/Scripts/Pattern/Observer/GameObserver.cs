#region oldClass
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class GameObserver : MonoBehaviour, IGameObserver
//{
//    [SerializeField] protected GameEvent gameEvent;
//    [SerializeField] protected UnityEvent unityEvent;

//    public GameEvent GEvent
//    {
//        get
//        {
//            return gameEvent;
//        }
//    }

//    public virtual void Awake()
//    {
//        gameEvent?.Subscribe(this);
//        MyDebug.Instance.Log("Game Observer Awake");
//    }

//    public virtual void OnDestroy()
//    {
//        gameEvent?.UnSubscribe(this);
//    }

//    public virtual void Execute(IGameEvent gameEvent)
//    {
//        MyDebug.Instance.Log($"Execute {this} in base class");
//    }

//    public virtual void RaiseUnityEvent()
//    {
//        unityEvent.Invoke();
//    }
//}
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameObserver : MonoBehaviour, IGameObserver
{
    protected Dictionary<string, GameEvent> gameEvents = new Dictionary<string, GameEvent>();
    [SerializeField] protected UnityEvent unityEvent;

    public GameEvent GetGameEvent(string gEventName)
    {
        return gameEvents[gEventName];
    }

    public void AddGameEventToObserver(GameEvent gEvent)
    {
        if (!gameEvents.ContainsValue(gEvent))
        {
            gameEvents.Add(gEvent.GameEventName, gEvent);
            gEvent.Subscribe(this);
        }
        else MyDebug.Instance.Log($"{gEvent} existed");
    }

    public void RemoveGameEventFromObserver(GameEvent gEvent)
    {
        if (gameEvents.ContainsValue(gEvent))
        {
            gameEvents.Remove(gEvent.GameEventName);
            gEvent.UnSubscribe(this);
        }
        //else MyDebug.Instance.Log($"{gEvent} not existed");
    }

    public virtual void Execute(IGameEvent gEvent)
    {
        //MyDebug.Instance.Log($"Execute {this} in base class");
    }

    public virtual void Execute(IGameEvent gEvent, int val)
    {
        //MyDebug.Instance.Log($"Execute by {this} in base class with value: {val}");
    }

    public virtual void Execute(IGameEvent gEvent, float val)
    {
        //MyDebug.Instance.Log($"Execute by {this} in base class with value: {val}");
    }

    public virtual void Execute(IGameEvent gEvent, bool val)
    {
        //MyDebug.Instance.Log($"Execute by {this} in base class with value: {val}");
    }

    public virtual void Execute(IGameEvent gEvent, object obj)
    {
        //MyDebug.Instance.Log($"Execute by {this} in base class with value: {point} {normal}");
    }

    public virtual void Execute(IGameEvent gEvent, RaycastHit hit)
    {
        //MyDebug.Instance.Log($"Execute by {this} in base class with value: {hit}");
    }

    public virtual void Execute(IGameEvent gEvent, Vector3 point, Vector3 normal)
    {
        //MyDebug.Instance.Log($"Execute by {this} in base class with value: {point} {normal}");
    }

    public virtual void RaiseUnityEvent()
    {
        unityEvent.Invoke();
    }
}