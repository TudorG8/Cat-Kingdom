using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Place the labels for the Transitions in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum Transition {
	NullTransition = 0, // Use this transition to represent a non-existing transition in your system
	DetectedEnemy = 1,
	LostEnemies = 2,
	GotToCastle = 3
}

/// <summary>
/// Place the labels for the States in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum StateID {
	NullStateID = 0, // Use this ID to represent a non-existing State in your system
	MovingToCastleId = 1,
	AttackingUnitsId = 2,
	AttackingCastleId = 3
}

public class EnemyBehaviour : MonoBehaviour {
	[SerializeField] FSMSystem fsm;
	[SerializeField] SelectableObject player;
	[SerializeField] MovementModule movementModule;
	[SerializeField] AttackModule attackModule;
	[SerializeField] bool stopped;

	public FSMSystem Fsm {
		get {
			return this.fsm;
		}
		set {
			fsm = value;
		}
	}

	public SelectableObject Player {
		get {
			return this.player;
		}
		set {
			player = value;
		}
	}

	public MovementModule MovementModule {
		get {
			return this.movementModule;
		}
		set {
			movementModule = value;
		}
	}

	public AttackModule AttackModule {
		get {
			return this.attackModule;
		}
		set {
			attackModule = value;
		}
	}

	void Start () {
		MakeFSM();
	}

	private void MakeFSM() {
		fsm = new FSMSystem();

		PursueCastle    pursueCastleState = new PursueCastle    (this);
		AttackUnitState attackUnitState   = new AttackUnitState (this);
		AttackCastle    attackCastleState = new AttackCastle    (this);

		pursueCastleState.AddTransition (Transition.DetectedEnemy, StateID.AttackingUnitsId);
		pursueCastleState.AddTransition (Transition.GotToCastle  , StateID.AttackingCastleId);

		attackCastleState.AddTransition (Transition.DetectedEnemy, StateID.AttackingUnitsId);

		attackUnitState  .AddTransition (Transition.LostEnemies  , StateID.MovingToCastleId);

		fsm.AddState(pursueCastleState);
		fsm.AddState(attackCastleState);
		fsm.AddState(attackUnitState  );

	}

	public void SetTransition(Transition t) { fsm.PerformTransition(t); }

	public void Stop() {
		player.StopCurrentAction ();
	}

	void Update () {
		if (!stopped) {
			fsm.CurrentState.Reason (gameObject);
			fsm.CurrentState.Act (gameObject);
		}
	}

	public void OnDeath() {
		EnemySpawner.Instance.Enemies.Remove (this);
	}
}

public class PursueCastle : FSMState {
	[SerializeField] EnemyBehaviour behaviour;
	[SerializeField] bool started;

	public PursueCastle(EnemyBehaviour behaviour) {
		this.behaviour = behaviour;
		started = false;
		stateID = StateID.MovingToCastleId;
	}
	public override void Reason(GameObject npc) {
		AttackableUnit nearbiestEnemy = behaviour.AttackModule.GetNextClosestEnemy (behaviour.transform.position);

		if (nearbiestEnemy != null) {
			behaviour.Fsm.PerformTransition (Transition.DetectedEnemy);
			behaviour.MovementModule.StopMoving ();
			started = false;
		}

		Building castle = BuildingManager.Instance.Castle;

		float tiles = BuildingManager.Instance.Castle.Recipe.Tiles / 2f + 0.2f;
		float attackingRange = Mathf.Sqrt (tiles * tiles + tiles * tiles);

		if (Vector3.Distance (castle.transform.position, behaviour.transform.position) < attackingRange) {
			behaviour.Fsm.PerformTransition (Transition.GotToCastle);
			behaviour.MovementModule.StopMoving ();
			started = false;
		}
	}
		
	public override void Act(GameObject npc) {
		if (!started) {
			Building castle = BuildingManager.Instance.Castle;
			behaviour.MovementModule.MoveTowards (castle.transform.position);
			started = true;
		}
	}	
}

public class AttackCastle : FSMState {
	[SerializeField] EnemyBehaviour behaviour;
	[SerializeField] bool started;

	public AttackCastle(EnemyBehaviour behaviour) {
		this.behaviour = behaviour;
		started = false;
		stateID = StateID.AttackingCastleId;
	}
	public override void Reason(GameObject npc) {
		AttackableUnit nearbiestEnemy = behaviour.AttackModule.GetNextClosestEnemy (behaviour.transform.position);

		if (nearbiestEnemy != null) {
			behaviour.Fsm.PerformTransition (Transition.DetectedEnemy);
			started = false;
		}
	}

	public override void Act(GameObject npc) {
		if (!started) {
			AttackableUnit castle = BuildingManager.Instance.Castle.GetComponent<AttackableUnit>();

			float tiles = BuildingManager.Instance.Castle.Recipe.Tiles / 2f + 0.2f;
			float attackingRange = Mathf.Sqrt (tiles * tiles + tiles * tiles);

			behaviour.AttackModule.AttackTarget(castle, 
				behaviour.Player.UnitStats.UnitClass.MinDamage, 
				behaviour.Player.UnitStats.UnitClass.MaxDamage,
				attackingRange
			);
			started = true;
		}
	}	
}

public class AttackUnitState : FSMState {
	[SerializeField] EnemyBehaviour behaviour;
	[SerializeField] bool started;
	public AttackUnitState(EnemyBehaviour behaviour) {
		this.behaviour = behaviour;
		started = false;
		stateID = StateID.AttackingUnitsId;
	}
	public override void Reason(GameObject npc) {
		if (!behaviour.AttackModule.IsPursuing) {
			behaviour.Fsm.PerformTransition (Transition.LostEnemies);
			started = false;
		}
	}


	public override void Act(GameObject npc) {
		if (!started) {
			AttackableUnit nearbiestEnemy = behaviour.AttackModule.GetNextClosestEnemy (behaviour.transform.position);

			if (nearbiestEnemy != null) {
				behaviour.AttackModule.AttackTarget (nearbiestEnemy, 
					behaviour.Player.UnitStats.UnitClass.MinDamage, 
					behaviour.Player.UnitStats.UnitClass.MaxDamage,
					behaviour.Player.UnitStats.UnitClass.Range
				);
			}
			started = true;
		}
	}	
}

