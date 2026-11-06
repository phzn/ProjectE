using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200017C RID: 380
	[Serializable]
	public class AIState
	{
		// Token: 0x060008E7 RID: 2279 RVA: 0x000296FA File Offset: 0x000278FA
		public virtual void SetBrain(AIBrain brain)
		{
			this._brain = brain;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00029704 File Offset: 0x00027904
		public virtual void EnterState()
		{
			foreach (AIAction aiaction in this.Actions)
			{
				aiaction.OnEnterState();
			}
			foreach (AITransition aitransition in this.Transitions)
			{
				if (aitransition.Decision != null)
				{
					aitransition.Decision.OnEnterState();
				}
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002979C File Offset: 0x0002799C
		public virtual void ExitState()
		{
			foreach (AIAction aiaction in this.Actions)
			{
				aiaction.OnExitState();
			}
			foreach (AITransition aitransition in this.Transitions)
			{
				if (aitransition.Decision != null)
				{
					aitransition.Decision.OnExitState();
				}
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00029834 File Offset: 0x00027A34
		public virtual void PerformActions()
		{
			if (this.Actions.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.Actions.Count; i++)
			{
				if (this.Actions[i] != null)
				{
					this.Actions[i].PerformAction();
				}
				else
				{
					Debug.LogError(string.Concat(new string[]
					{
						"An action in ",
						this._brain.gameObject.name,
						" on state ",
						this.StateName,
						" is null."
					}));
				}
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x000298D0 File Offset: 0x00027AD0
		public virtual void EvaluateTransitions()
		{
			if (this.Transitions.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.Transitions.Count; i++)
			{
				if (this.Transitions[i].Decision != null)
				{
					if (this.Transitions[i].Decision.Decide())
					{
						if (!string.IsNullOrEmpty(this.Transitions[i].TrueState))
						{
							this._brain.TransitionToState(this.Transitions[i].TrueState);
							return;
						}
					}
					else if (!string.IsNullOrEmpty(this.Transitions[i].FalseState))
					{
						this._brain.TransitionToState(this.Transitions[i].FalseState);
						return;
					}
				}
			}
		}

		// Token: 0x04000A83 RID: 2691
		public string StateName;

		// Token: 0x04000A84 RID: 2692
		[MMReorderableAttribute(null, "Action", null)]
		public AIActionsList Actions;

		// Token: 0x04000A85 RID: 2693
		[MMReorderableAttribute(null, "Transition", null)]
		public AITransitionsList Transitions;

		// Token: 0x04000A86 RID: 2694
		protected AIBrain _brain;
	}
}
