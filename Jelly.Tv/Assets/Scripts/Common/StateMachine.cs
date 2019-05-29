﻿using System.Collections.Generic;

public class StateMachine<T> {
    private Dictionary<string, State<T>> m_states = new Dictionary<string, State<T>>();
    private State<T> m_state = null;

    public void Update() { if (m_state != null) m_state.Update(); }

    public void AddState(string stateID, State<T> state) { m_states[stateID] = state; }

    public bool SetState(string stateID) {
        if (!m_states.ContainsKey(stateID)) return false;
        State<T> state = m_states[stateID];
        if (state != m_state) {
            if (m_state != null) {
                m_state.Exit();
            }
            m_state = state;
            m_state.Enter();
            m_state.Update();
            return true;
        }
        return false;
    }
}
