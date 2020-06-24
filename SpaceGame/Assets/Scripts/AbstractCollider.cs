using System;
using UnityEditor;
using UnityEngine;
public abstract class AbstractCollider : MonoBehaviour
{

    public event Action<Collider> OnPlayerCollide;
    public event Action<Collider> OnTrashCollide;
    public event Action<Collider> OnObstacleCollide;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            HandlePlayerEnter(other);
            OnPlayerCollide?.Invoke(other);
        }
        if (other.CompareTag("Debris"))
        {
            HandleTrashEnter(other);
            OnTrashCollide?.Invoke(other);
        }
        if (other.CompareTag("Obstacles"))
        {
            HandleObstacleEnter(other);
            OnObstacleCollide?.Invoke(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            HandlePlayerStay(other);
        }
        if (other.CompareTag("Debris"))
        {
            HandleTrashStay(other);
        }
        if (other.CompareTag("Obstacles"))
        {
            HandleObstacleStay(other);
        }
    }
    private void OnTriggerLeave(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            HandlePlayerLeave(other);
        }
        if (other.CompareTag("Debris"))
        {
            HandleTrashLeave(other);
        }
        if (other.CompareTag("Obstacles"))
        {
            HandleObstacleLeave(other);
        }
    }

    protected virtual void HandlePlayerEnter(Collider player)
    {
    }
    protected virtual void HandleTrashEnter(Collider player)
    {
    }
    protected virtual void HandleObstacleEnter(Collider player)
    {
    }
    protected virtual void HandlePlayerStay(Collider player)
    {
    }
    protected virtual void HandleTrashStay(Collider player)
    {
    }
    protected virtual void HandleObstacleStay(Collider player)
    {
    }
    protected virtual void HandlePlayerLeave(Collider player)
    {
    }
    protected virtual void HandleTrashLeave(Collider player)
    {
    }
    protected virtual void HandleObstacleLeave(Collider player)
    {
    }
    
}