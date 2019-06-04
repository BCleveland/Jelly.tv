using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {

    [SerializeField]
    public SpriteRenderer FaceSpriteRenderer;
    [SerializeField]
    public SpriteRenderer ShapeSpriteRenderer;

    [SerializeField] private float m_slimeSpeed = 1.0f;

    private string m_playerID = "";
    private string m_state = "";
    private StateMachine<Slime> m_stateMachine = null;
    private Claw m_claw = null;

    public float SlimeSpeed { get => m_slimeSpeed; }
    public Claw Claw { get => m_claw; set => m_claw = value; }

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
        State = "Wander";
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
        //Play scared animation or something
    }

    public override void Exit() { }

}

class WanderState<T> : State<T> where T : Slime {
    private Vector3 m_newLocation;
    private float m_startDistance;
    private float m_lerpAlpha;

    public WanderState(T owner) : base(owner) { }

    public override void Enter() { }
    public override void Update() {
        if (Owner.transform.position == m_newLocation) {
            float newX = Random.Range(Lobby.Instance.StartingPosition.x - (Lobby.Instance.WanderRange.x * 0.5f), Lobby.Instance.StartingPosition.x + (Lobby.Instance.WanderRange.x * 0.5f));
            float newY = Random.Range(Lobby.Instance.StartingPosition.y - (Lobby.Instance.WanderRange.y * 0.5f), Lobby.Instance.StartingPosition.y + (Lobby.Instance.WanderRange.y * 0.5f));
            m_newLocation = new Vector3(newX, newY, 0.0f);
            float dist = Vector3.Distance(Owner.transform.position, m_newLocation);
            m_startDistance = dist == 0.0f ? 0.01f : dist;
            m_lerpAlpha = 0.0f;
        } else {
            m_lerpAlpha += (Owner.SlimeSpeed / m_startDistance) * Time.deltaTime;
            Owner.transform.position = Vector3.Lerp(Owner.transform.position, m_newLocation, m_lerpAlpha);
        }
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
        // Change to move to a set offset from the claw's transform
        if (Owner.Claw) Owner.transform.position = Owner.Claw.transform.position;
    }

    public override void Exit() {
        Owner.Claw = null;
    }

}