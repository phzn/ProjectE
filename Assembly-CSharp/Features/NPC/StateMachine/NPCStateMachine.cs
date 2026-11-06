using System;
using System.Collections.Generic;
using Features.NPC.StateMachine.States;
using UnityEngine;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F0D RID: 3853
	public class NPCStateMachine : MonoBehaviour
	{
		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x060073EA RID: 29674 RVA: 0x002F2B98 File Offset: 0x002F0D98
		// (remove) Token: 0x060073EB RID: 29675 RVA: 0x002F2BD0 File Offset: 0x002F0DD0
		public event Action<NPCStateType, NPCStateType, string> OnStateChanged;

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x060073EC RID: 29676 RVA: 0x002F2C05 File Offset: 0x002F0E05
		public NPCStateType CurrentStateType
		{
			get
			{
				return this._currentStateType;
			}
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x060073ED RID: 29677 RVA: 0x002F2C0D File Offset: 0x002F0E0D
		public INPCState CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x060073EE RID: 29678 RVA: 0x002F2C15 File Offset: 0x002F0E15
		public NPCFSMContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x060073EF RID: 29679 RVA: 0x002F2C1D File Offset: 0x002F0E1D
		public bool IsInitialized
		{
			get
			{
				return this._context != null && this._currentState != null;
			}
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x002F2C32 File Offset: 0x002F0E32
		private void Awake()
		{
			this._context = new NPCFSMContext(base.gameObject);
			this.RegisterStates();
		}

		// Token: 0x060073F1 RID: 29681 RVA: 0x002F2C4B File Offset: 0x002F0E4B
		private void OnEnable()
		{
			NPCSimulationPause.RegisterParticipant(this);
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x060073F2 RID: 29682 RVA: 0x002F2C6F File Offset: 0x002F0E6F
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
		}

		// Token: 0x060073F3 RID: 29683 RVA: 0x002F2C84 File Offset: 0x002F0E84
		private void Start()
		{
			INPCState inpcstate;
			if (this._states.TryGetValue(this._currentStateType, out inpcstate))
			{
				this._currentState = inpcstate;
				this._currentState.OnEnter(this._context, null);
				return;
			}
			Debug.LogError(string.Format("[FSM] {0}: Initial state {1} not registered!", base.gameObject.name, this._currentStateType));
		}

		// Token: 0x060073F4 RID: 29684 RVA: 0x002F2CE8 File Offset: 0x002F0EE8
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			if (this._context != null)
			{
				this._context.StateEnteredTime += NPCSimulationPause.FrameTimeLoss;
			}
			if (NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this._currentState == null)
			{
				return;
			}
			if (this._pendingState != null)
			{
				this.ExecuteTransition(this._pendingState.Value, this._pendingOwner);
				this._pendingState = null;
				this._pendingOwner = null;
			}
			NPCStateType? npcstateType = this._currentState.OnUpdate(this._context);
			if (npcstateType != null && npcstateType.Value != this._currentStateType)
			{
				this.RequestState(npcstateType.Value, "StateUpdate");
			}
		}

		// Token: 0x060073F5 RID: 29685 RVA: 0x002F2DB2 File Offset: 0x002F0FB2
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060073F6 RID: 29686 RVA: 0x002F2DC4 File Offset: 0x002F0FC4
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x060073F7 RID: 29687 RVA: 0x002F2DE4 File Offset: 0x002F0FE4
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			if (this._context != null)
			{
				this._context.StateEnteredTime += num;
			}
			this._simulationPauseActive = false;
		}

		// Token: 0x060073F8 RID: 29688 RVA: 0x002F2E34 File Offset: 0x002F1034
		private void RegisterStates()
		{
			this.RegisterState(new IdleState());
			this.RegisterState(new WanderingState());
			this.RegisterState(new PerformingState());
			this.RegisterState(new LookingAtState());
			this.RegisterState(new StopAndStareState());
			this.RegisterState(new SearchingState());
			this.RegisterState(new ConfusionState());
			this.RegisterState(new StumblingState());
			this.RegisterState(new TaseredState());
			this.RegisterState(new DetectedState());
			this.RegisterState(new FrozenState());
		}

		// Token: 0x060073F9 RID: 29689 RVA: 0x002F2EBC File Offset: 0x002F10BC
		private void RegisterState(INPCState state)
		{
			if (this._states.ContainsKey(state.StateType))
			{
				Debug.LogWarning(string.Format("[FSM] State {0} already registered, overwriting.", state.StateType));
			}
			this._states[state.StateType] = state;
		}

		// Token: 0x060073FA RID: 29690 RVA: 0x002F2F08 File Offset: 0x002F1108
		public bool RequestState(NPCStateType targetState, string owner = "Unknown")
		{
			if (this._currentState == null)
			{
				this.Log(string.Format("RequestState({0}) rejected - no current state", targetState));
				return false;
			}
			if (targetState == this._currentStateType)
			{
				return true;
			}
			if (!this._currentState.CanTransitionTo(targetState))
			{
				this.Log(string.Format("RequestState({0}) rejected by {1}.CanTransitionTo()", targetState, this._currentStateType));
				return false;
			}
			if (!this._states.ContainsKey(targetState))
			{
				Debug.LogError(string.Format("[FSM] {0}: State {1} not registered!", base.gameObject.name, targetState));
				return false;
			}
			this._pendingState = new NPCStateType?(targetState);
			this._pendingOwner = owner;
			this.Log(string.Format("RequestState({0}) queued by {1}", targetState, owner));
			return true;
		}

		// Token: 0x060073FB RID: 29691 RVA: 0x002F2FCC File Offset: 0x002F11CC
		public void ForceState(NPCStateType targetState, string owner = "Force")
		{
			if (!this._states.ContainsKey(targetState))
			{
				Debug.LogError(string.Format("[FSM] {0}: State {1} not registered!", base.gameObject.name, targetState));
				return;
			}
			this.Log(string.Format("ForceState({0}) by {1}", targetState, owner));
			this.ExecuteTransition(targetState, owner);
		}

		// Token: 0x060073FC RID: 29692 RVA: 0x002F3028 File Offset: 0x002F1228
		private void ExecuteTransition(NPCStateType targetState, string owner)
		{
			INPCState inpcstate;
			if (!this._states.TryGetValue(targetState, out inpcstate))
			{
				Debug.LogError(string.Format("[FSM] {0}: State {1} not found!", base.gameObject.name, targetState));
				return;
			}
			NPCStateType currentStateType = this._currentStateType;
			INPCState currentState = this._currentState;
			if (currentState != null)
			{
				currentState.OnExit(this._context, inpcstate);
			}
			this._currentState = inpcstate;
			this._currentStateType = targetState;
			this._context.CurrentOwner = owner;
			this._currentState.OnEnter(this._context, currentState);
			Action<NPCStateType, NPCStateType, string> onStateChanged = this.OnStateChanged;
			if (onStateChanged == null)
			{
				return;
			}
			onStateChanged(currentStateType, targetState, owner);
		}

		// Token: 0x060073FD RID: 29693 RVA: 0x002F30C3 File Offset: 0x002F12C3
		public bool CanTransitionTo(NPCStateType targetState)
		{
			INPCState currentState = this._currentState;
			return currentState != null && currentState.CanTransitionTo(targetState);
		}

		// Token: 0x060073FE RID: 29694 RVA: 0x002F30D8 File Offset: 0x002F12D8
		public INPCState GetState(NPCStateType stateType)
		{
			INPCState inpcstate;
			this._states.TryGetValue(stateType, out inpcstate);
			return inpcstate;
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x060073FF RID: 29695 RVA: 0x002F30F5 File Offset: 0x002F12F5
		public bool IsInNormalState
		{
			get
			{
				return this._currentStateType.IsNormalState();
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06007400 RID: 29696 RVA: 0x002F3102 File Offset: 0x002F1302
		public bool IsInAlertState
		{
			get
			{
				return this._currentStateType.IsAlertState();
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06007401 RID: 29697 RVA: 0x002F310F File Offset: 0x002F130F
		public bool IsInCriticalState
		{
			get
			{
				return this._currentStateType.IsCriticalState();
			}
		}

		// Token: 0x06007402 RID: 29698 RVA: 0x002F311C File Offset: 0x002F131C
		private void Log(string message)
		{
			bool flag = this.enableLogging;
		}

		// Token: 0x040076DE RID: 30430
		[Header("Debug")]
		[SerializeField]
		private bool enableLogging = true;

		// Token: 0x040076DF RID: 30431
		[SerializeField]
		private NPCStateType _currentStateType = NPCStateType.Idle;

		// Token: 0x040076E0 RID: 30432
		private NPCFSMContext _context;

		// Token: 0x040076E1 RID: 30433
		private INPCState _currentState;

		// Token: 0x040076E2 RID: 30434
		private readonly Dictionary<NPCStateType, INPCState> _states = new Dictionary<NPCStateType, INPCState>();

		// Token: 0x040076E3 RID: 30435
		private NPCStateType? _pendingState;

		// Token: 0x040076E4 RID: 30436
		private string _pendingOwner;

		// Token: 0x040076E5 RID: 30437
		private bool _simulationPauseActive;

		// Token: 0x040076E6 RID: 30438
		private float _simulationPauseStartedAt;
	}
}
