using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000176 RID: 374
	public abstract class AIAction : MonoBehaviour
	{
		// Token: 0x060008C1 RID: 2241
		public abstract void PerformAction();

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00029034 File Offset: 0x00027234
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0002903C File Offset: 0x0002723C
		public virtual bool ActionInProgress { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00029048 File Offset: 0x00027248
		protected virtual bool ShouldInitialize
		{
			get
			{
				AIAction.InitializationModes initializationMode = this.InitializationMode;
				return initializationMode == AIAction.InitializationModes.EveryTime || initializationMode != AIAction.InitializationModes.OnlyOnce || !this._initialized;
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00029072 File Offset: 0x00027272
		protected virtual void Awake()
		{
			this._brain = base.gameObject.GetComponentInParent<AIBrain>();
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00029085 File Offset: 0x00027285
		public virtual void Initialization()
		{
			this._initialized = true;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002908E File Offset: 0x0002728E
		public virtual void OnEnterState()
		{
			this.ActionInProgress = true;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00029097 File Offset: 0x00027297
		public virtual void OnExitState()
		{
			this.ActionInProgress = false;
		}

		// Token: 0x04000A63 RID: 2659
		public AIAction.InitializationModes InitializationMode;

		// Token: 0x04000A64 RID: 2660
		protected bool _initialized;

		// Token: 0x04000A65 RID: 2661
		[Tooltip("a label you can set to organize your AI Actions, not used by anything else")]
		public string Label;

		// Token: 0x04000A67 RID: 2663
		protected AIBrain _brain;

		// Token: 0x02000471 RID: 1137
		public enum InitializationModes
		{
			// Token: 0x040020C8 RID: 8392
			EveryTime,
			// Token: 0x040020C9 RID: 8393
			OnlyOnce
		}
	}
}
