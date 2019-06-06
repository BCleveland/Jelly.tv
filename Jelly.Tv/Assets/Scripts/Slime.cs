using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slime : MonoBehaviour {

    [SerializeField]
    public SpriteRenderer FaceSpriteRenderer;
    [SerializeField]
    public SpriteRenderer ShapeSpriteRenderer;

    [SerializeField] private float m_slimeSpeed = 1.0f;
    [SerializeField] private TextMeshPro m_textDisplay = null;

    private string m_playerID = "";
    private string m_state = "";
    private StateMachine<Slime> m_stateMachine = null;
    private Claw m_claw = null;

    public float SlimeSpeed { get => m_slimeSpeed; }
    public Claw Claw { get => m_claw; set => m_claw = value; }
    public TextMeshPro TextDisplay { get => m_textDisplay; set => m_textDisplay = value; }

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
        m_stateMachine.AddState("Flee", new FleeState<Slime>(this));
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
        float tolerance = 0.0001f;
        if (Vector3.Distance(Owner.transform.position, m_newLocation) < tolerance) {
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
		Vector3 pos = Owner.transform.position + 
			(Vector3.down * 5.0f * Time.deltaTime);
		pos.y = Mathf.Max(pos.y, -4.5f);
		Owner.transform.position = pos;
	}

	public override void Exit() { }

}

class ClawState<T> : State<T> where T : Slime {

    public ClawState(T owner) : base(owner) { }

    public override void Enter() { }
    public override void Update() {
        // Change to move to a set offset from the claw's transform
        if (Owner.Claw) Owner.transform.position = Owner.Claw.m_GrabPosition.position;
    }

    public override void Exit() {
        Owner.Claw = null;
    }

}

class FleeState<T> : State<T> where T : Slime {
    private Vector3 m_fleeLocation;
    private float m_startDistance;
    private float m_lerpAlpha;

    public FleeState(T owner) : base(owner) { }

    public override void Enter() {
        m_fleeLocation = Lobby.Instance.FleeLocations[Random.Range(0, Lobby.Instance.FleeLocations.Count)];
        m_lerpAlpha = 0.0f;
        float dist = Vector3.Distance(Owner.transform.position, m_fleeLocation);
        m_startDistance = dist == 0.0f ? 0.01f : dist;
    }

    public override void Update() {
        m_lerpAlpha += (Owner.SlimeSpeed*5 / m_startDistance) * Time.deltaTime;
        Owner.transform.position = Vector3.Lerp(Owner.transform.position, m_fleeLocation, m_lerpAlpha);
    }

    public override void Exit() {
        
    }

}