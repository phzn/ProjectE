using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000177 RID: 375
	[AddComponentMenu("More Mountains/Tools/AI/AIBrain")]
	public class AIBrain : MonoBehaviour
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x000290A8 File Offset: 0x000272A8
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x000290B0 File Offset: 0x000272B0
		public virtual AIState CurrentState { get; protected set; }

		// Token: 0x060008CC RID: 2252 RVA: 0x000290B9 File Offset: 0x000272B9
		public virtual AIAction[] GetAttachedActions()
		{
			return base.gameObject.GetComponentsInChildren<AIAction>();
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000290C6 File Offset: 0x000272C6
		public virtual AIDecision[] GetAttachedDecisions()
		{
			return base.gameObject.GetComponentsInChildren<AIDecision>();
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000290D3 File Offset: 0x000272D3
		protected virtual void OnEnable()
		{
			if (this.ResetBrainOnEnable)
			{
				this.ResetBrain();
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000290E4 File Offset: 0x000272E4
		protected virtual void Awake()
		{
			foreach (AIState aistate in this.States)
			{
				aistate.SetBrain(this);
			}
			this._decisions = this.GetAttachedDecisions();
			this._actions = this.GetAttachedActions();
			if (this.RandomizeFrequencies)
			{
				this.ActionsFrequency = Random.Range(this.RandomActionFrequency.x, this.RandomActionFrequency.y);
				this.DecisionFrequency = Random.Range(this.RandomDecisionFrequency.x, this.RandomDecisionFrequency.y);
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00029198 File Offset: 0x00027398
		protected virtual void Start()
		{
			if (this.ResetBrainOnStart)
			{
				this.ResetBrain();
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000291A8 File Offset: 0x000273A8
		protected virtual void Update()
		{
			if (!this.BrainActive || this.CurrentState == null || Time.timeScale == 0f)
			{
				return;
			}
			if (Time.time - this._lastActionsUpdate > this.ActionsFrequency)
			{
				this.CurrentState.PerformActions();
				this._lastActionsUpdate = Time.time;
			}
			if (!this.BrainActive)
			{
				return;
			}
			if (Time.time - this._lastDecisionsUpdate > this.DecisionFrequency)
			{
				this.CurrentState.EvaluateTransitions();
				this._lastDecisionsUpdate = Time.time;
			}
			this.TimeInThisState += Time.deltaTime;
			this.StoreLastKnownPosition();
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00029248 File Offset: 0x00027448
		public virtual void TransitionToState(string newStateName)
		{
			this._newState = this.FindState(newStateName);
			AIStateEvent.Trigger(this, this.CurrentState, this._newState);
			if (this.CurrentState == null)
			{
				this.CurrentState = this._newState;
				if (this.CurrentState != null)
				{
					this.CurrentState.EnterState();
				}
				return;
			}
			if (newStateName != this.CurrentState.StateName)
			{
				this.CurrentState.ExitState();
				this.OnExitState();
				this.CurrentState = this._newState;
				if (this.CurrentState != null)
				{
					this.CurrentState.EnterState();
				}
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000292DF File Offset: 0x000274DF
		protected virtual void OnExitState()
		{
			this.TimeInThisState = 0f;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000292EC File Offset: 0x000274EC
		protected virtual void InitializeDecisions()
		{
			if (this._decisions == null)
			{
				this._decisions = this.GetAttachedDecisions();
			}
			AIDecision[] decisions = this._decisions;
			for (int i = 0; i < decisions.Length; i++)
			{
				decisions[i].Initialization();
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002932C File Offset: 0x0002752C
		protected virtual void InitializeActions()
		{
			if (this._actions == null)
			{
				this._actions = this.GetAttachedActions();
			}
			AIAction[] actions = this._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].Initialization();
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002936C File Offset: 0x0002756C
		protected AIState FindState(string stateName)
		{
			foreach (AIState aistate in this.States)
			{
				if (aistate.StateName == stateName)
				{
					return aistate;
				}
			}
			if (stateName != "")
			{
				Debug.LogError(string.Concat(new string[]
				{
					"You're trying to transition to state '",
					stateName,
					"' in ",
					base.gameObject.name,
					"'s AI Brain, but no state of this name exists. Make sure your states are named properly, and that your transitions states match existing states."
				}));
			}
			return null;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00029414 File Offset: 0x00027614
		protected virtual void StoreLastKnownPosition()
		{
			if (this.Target != null)
			{
				this._lastKnownTargetPosition = this.Target.transform.position;
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002943C File Offset: 0x0002763C
		public virtual void ResetBrain()
		{
			this.InitializeDecisions();
			this.InitializeActions();
			this.BrainActive = true;
			base.enabled = true;
			if (this.CurrentState != null)
			{
				this.CurrentState.ExitState();
				this.OnExitState();
			}
			if (this.States.Count > 0)
			{
				this._newState = this.States[0];
				AIStateEvent.Trigger(this, this.CurrentState, this._newState);
				this.CurrentState = this._newState;
				AIState currentState = this.CurrentState;
				if (currentState == null)
				{
					return;
				}
				currentState.EnterState();
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x000294CC File Offset: 0x000276CC
		[ContextMenu("Delete unused actions and decisions")]
		public virtual void DeleteUnusedActionsAndDecisions()
		{
			AIAction[] componentsInChildren = base.gameObject.GetComponentsInChildren<AIAction>();
			AIDecision[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<AIDecision>();
			foreach (AIAction aiaction in componentsInChildren)
			{
				bool flag = false;
				using (List<AIState>.Enumerator enumerator = this.States.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Actions.Contains(aiaction))
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Object.DestroyImmediate(aiaction);
				}
			}
			foreach (AIDecision aidecision in componentsInChildren2)
			{
				bool flag2 = false;
				foreach (AIState aistate in this.States)
				{
					using (IEnumerator<AITransition> enumerator2 = aistate.Transitions.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Decision == aidecision)
							{
								flag2 = true;
							}
						}
					}
				}
				if (!flag2)
				{
					Object.DestroyImmediate(aidecision);
				}
			}
		}

		// Token: 0x04000A68 RID: 2664
		[Header("Debug")]
		[MMReadOnly]
		public GameObject Owner;

		// Token: 0x04000A69 RID: 2665
		public List<AIState> States;

		// Token: 0x04000A6B RID: 2667
		[MMReadOnly]
		public float TimeInThisState;

		// Token: 0x04000A6C RID: 2668
		[MMReadOnly]
		public Transform Target;

		// Token: 0x04000A6D RID: 2669
		[MMReadOnly]
		public Vector3 _lastKnownTargetPosition = Vector3.zero;

		// Token: 0x04000A6E RID: 2670
		[Header("State")]
		public bool BrainActive = true;

		// Token: 0x04000A6F RID: 2671
		public bool ResetBrainOnStart = true;

		// Token: 0x04000A70 RID: 2672
		public bool ResetBrainOnEnable;

		// Token: 0x04000A71 RID: 2673
		[Header("Frequencies")]
		public float ActionsFrequency;

		// Token: 0x04000A72 RID: 2674
		public float DecisionFrequency;

		// Token: 0x04000A73 RID: 2675
		public bool RandomizeFrequencies;

		// Token: 0x04000A74 RID: 2676
		[MMVector(new string[] { "min", "max" })]
		public Vector2 RandomActionFrequency = new Vector2(0.5f, 1f);

		// Token: 0x04000A75 RID: 2677
		[MMVector(new string[] { "min", "max" })]
		public Vector2 RandomDecisionFrequency = new Vector2(0.5f, 1f);

		// Token: 0x04000A76 RID: 2678
		protected AIDecision[] _decisions;

		// Token: 0x04000A77 RID: 2679
		protected AIAction[] _actions;

		// Token: 0x04000A78 RID: 2680
		protected float _lastActionsUpdate;

		// Token: 0x04000A79 RID: 2681
		protected float _lastDecisionsUpdate;

		// Token: 0x04000A7A RID: 2682
		protected AIState _initialState;

		// Token: 0x04000A7B RID: 2683
		protected AIState _newState;
	}
}
