using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent", fileName = "New Game Event")]
public class GameEvent : ScriptableObject, IGameEvent
{
    [SerializeField]
    private string _gameEventName;
    public string GameEventName
    {
        get => _gameEventName;
        set => _gameEventName = value;
    }

    // List of subscribers. In real life, the list of subscribers can be
    // stored more comprehensively (categorized by event type, etc.).
    private HashSet<IGameObserver> gameObservers = new HashSet<IGameObserver>();
    // The subscription management methods.
    public void Subscribe(IGameObserver observer)
    {
        this.gameObservers.Add(observer);
        //Debug.Log($"Add {observer}");
    }

    public void UnSubscribe(IGameObserver observer)
    {
        this.gameObservers.Remove(observer);
        //Debug.Log($"Remove {observer}");
    }

    // Trigger an update in each subscriber.
    public void Notify()
    {
        foreach (var observer in gameObservers)
        {
            //Debug.Log($"Execute {this}");
            observer.Execute(this);
        }
    }

    public void Notify(int val)
    {
        foreach (var observer in gameObservers)
        {
            //Debug.Log($"Execute {this} {observer} in {gameObservers.Count}");
            observer.Execute(this, val);
        }
    }

    public void Notify(float val)
    {
        foreach (var observer in gameObservers)
        {
            //Debug.Log($"Execute {this} {observer} in {gameObservers.Count}");
            observer.Execute(this, val);
        }
    }

    public void Notify(bool val)
    {
        foreach (var observer in gameObservers)
        {
            //Debug.Log($"Execute {this} {observer} in {gameObservers.Count}");
            observer.Execute(this, val);
        }
    }

    public void Notify(RaycastHit hit)
    {
        foreach (var observer in gameObservers)
        {
            //Debug.Log($"Execute {this} {observer} in {gameObservers.Count}");
            observer.Execute(this, hit);
        }
    }

    public void Notify(Vector3 point, Vector3 normal)
    {
        foreach (var observer in gameObservers)
        {
            //Debug.Log($"Execute {this} {observer} in {gameObservers.Count}");
            observer.Execute(this, point, normal);
        }
    }

    //public void Notify<T>(T hit)
    //{
    //    foreach (var observer in gameObservers)
    //    {
    //        //Debug.Log($"Execute {this} {observer} in {gameObservers.Count}");
    //        observer.Execute(this, hit);
    //    }
    //}
}