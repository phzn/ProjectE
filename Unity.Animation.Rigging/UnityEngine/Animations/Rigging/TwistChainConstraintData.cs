using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000052 RID: 82
	[Serializable]
	public struct TwistChainConstraintData : IAnimationJobData, ITwistChainConstraintData
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007520 File Offset: 0x00005720
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00007528 File Offset: 0x00005728
		public Transform root
		{
			get
			{
				return this.m_Root;
			}
			set
			{
				this.m_Root = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00007531 File Offset: 0x00005731
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00007539 File Offset: 0x00005739
		public Transform tip
		{
			get
			{
				return this.m_Tip;
			}
			set
			{
				this.m_Tip = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00007542 File Offset: 0x00005742
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000754A File Offset: 0x0000574A
		public Transform rootTarget
		{
			get
			{
				return this.m_RootTarget;
			}
			set
			{
				this.m_RootTarget = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00007553 File Offset: 0x00005753
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000755B File Offset: 0x0000575B
		public Transform tipTarget
		{
			get
			{
				return this.m_TipTarget;
			}
			set
			{
				this.m_TipTarget = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00007564 File Offset: 0x00005764
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000756C File Offset: 0x0000576C
		public AnimationCurve curve
		{
			get
			{
				return this.m_Curve;
			}
			set
			{
				this.m_Curve = value;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007578 File Offset: 0x00005778
		bool IAnimationJobData.IsValid()
		{
			return !(this.root == null) && !(this.tip == null) && this.tip.IsChildOf(this.root) && !(this.rootTarget == null) && !(this.tipTarget == null) && this.curve != null;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000075DC File Offset: 0x000057DC
		void IAnimationJobData.SetDefaultValues()
		{
			this.root = (this.tip = (this.rootTarget = (this.tipTarget = null)));
			this.curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		private Transform m_Root;

		// Token: 0x040000E5 RID: 229
		[SerializeField]
		private Transform m_Tip;

		// Token: 0x040000E6 RID: 230
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_RootTarget;

		// Token: 0x040000E7 RID: 231
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_TipTarget;

		// Token: 0x040000E8 RID: 232
		[SerializeField]
		private AnimationCurve m_Curve;
	}
}
