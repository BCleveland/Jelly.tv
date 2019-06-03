using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {

    [SerializeField]
    public SpriteRenderer FaceSpriteRenderer;
    [SerializeField]
    public SpriteRenderer ShapeSpriteRenderer;

    private string m_playerID = "";
    private string m_state = "";
    private StateMachine<Slime> m_stateMachine = null;

    public string State {
        get => m_state;
        set => m_state = (m_stateMachine.SetState(value) ? value : m_state);
    }
    public string PlayerID { get => m_playerID; }


    void Start() {
        m_stateMachine = new StateMachine<Slime>();
        m_stateMachine.AddState("Soulless", new SoullessState<Slime>(this));
        m_stateMachine.AddState("Wander", new WanderState<Slime>(this));
        m_stateMachine.AddState("Battle", new BattleState<Slime>(this));
        m_stateMachine.AddState("Claw", new ClawState<Slime>(this));
        State = "Soulless";
    }

    void Update() {
        m_stateMachine.Update();
    }


    public void SetPlayer(PlayerManager.Player player) {
        m_playerID = player.Id;
        //SET CUSTOMIZATION HERE
    }
}

class SoullessState<T> : State<T> where T : Slime {

    public SoullessState(T owner) : base(owner) { }

    public override void Enter() { }
    public override void Update() {
        //Example of how to set state
        Owner.State = "Wander";
    }

    public override void Exit() { }

}

class WanderState<T> : State<T> where T : Slime {

    public WanderState(T owner) : base(owner) { }

    public override void Enter() { }
    public override void Update() {

    }

    public override void Exit() { }

}

//Fill out with battle behaviors
class BattleState<T> : State<T> where T : Slime {

    public BattleState(T owner) : base(owner) { }

    public override void Enter() { }
    public override void Update() {

    }

    public override void Exit() { }

}

class ClawState<T> : State<T> where T : Slime {

    public ClawState(T owner) : base(owner) { }

    public override void Enter() { }
    public override void Update() {

    }

    public override void Exit() { }

}