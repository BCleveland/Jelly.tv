using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> {
    private T m_owner;

    public T Owner { get => m_owner; }

    public State(T owner) => m_owner = owner;

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
