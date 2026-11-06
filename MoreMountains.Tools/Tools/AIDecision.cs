using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000178 RID: 376
	public abstract class AIDecision : MonoBehaviour
	{
		// Token: 0x060008DB RID: 2267
		public abstract bool Decide();

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00029666 File Offset: 0x00027866
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0002966E File Offset: 0x0002786E
		public virtual bool DecisionInProgress { get; set; }

		// Token: 0x060008DE RID: 2270 RVA: 0x00029677 File Offset: 0x00027877
		protected virtual void Awake()
		{
			this._brain = base.gameObject.GetComponentInParent<AIBrain>();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002968A File Offset: 0x0002788A
		public virtual void Initialization()
		{
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002968C File Offset: 0x0002788C
		public virtual void OnEnterState()
		{
			this.DecisionInProgress = true;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00029695 File Offset: 0x00027895
		public virtual void OnExitState()
		{
			this.DecisionInProgress = false;
		}

		// Token: 0x04000A7C RID: 2684
		[Tooltip("a label you can set to organize your AI Decisions, not used by anything else")]
		public string Label;

		// Token: 0x04000A7E RID: 2686
		protected AIBrain _brain;
	}
}
