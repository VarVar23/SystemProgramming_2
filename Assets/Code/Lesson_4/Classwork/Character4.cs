using System;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(CharacterController))]
public abstract class Character4 : NetworkBehaviour
{
    protected Action onUpdateAction { get; set; }
    //protected abstract FireAction fireAction { get; set; }

    [SyncVar] protected Vector3 serverPosition;

    protected virtual void Initiate()
    {
        onUpdateAction += Movement;
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnUpdate()
    {
        onUpdateAction?.Invoke();
    }

    [Command]
    protected void CmdUpdatePosition(Vector3 position)
    {
        serverPosition = position;
    }

    public abstract void Movement();
}
