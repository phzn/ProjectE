using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000056 RID: 86
	[Serializable]
	public struct TwoBoneIKConstraintData : IAnimationJobData, ITwoBoneIKConstraintData
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00007750 File Offset: 0x00005950
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00007758 File Offset: 0x00005958
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

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00007761 File Offset: 0x00005961
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00007769 File Offset: 0x00005969
		public Transform mid
		{
			get
			{
				return this.m_Mid;
			}
			set
			{
				this.m_Mid = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00007772 File Offset: 0x00005972
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0000777A File Offset: 0x0000597A
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00007783 File Offset: 0x00005983
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000778B File Offset: 0x0000598B
		public Transform target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				this.m_Target = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00007794 File Offset: 0x00005994
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000779C File Offset: 0x0000599C
		public Transform hint
		{
			get
			{
				return this.m_Hint;
			}
			set
			{
				this.m_Hint = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000077A5 File Offset: 0x000059A5
		// (set) Token: 0x06000220 RID: 544 RVA: 0x000077AD File Offset: 0x000059AD
		public float targetPositionWeight
		{
			get
			{
				return this.m_TargetPositionWeight;
			}
			set
			{
				this.m_TargetPositionWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000077BB File Offset: 0x000059BB
		// (set) Token: 0x06000222 RID: 546 RVA: 0x000077C3 File Offset: 0x000059C3
		public float targetRotationWeight
		{
			get
			{
				return this.m_TargetRotationWeight;
			}
			set
			{
				this.m_TargetRotationWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000223 RID: 547 RVA: 0x000077D1 File Offset: 0x000059D1
		// (set) Token: 0x06000224 RID: 548 RVA: 0x000077D9 File Offset: 0x000059D9
		public float hintWeight
		{
			get
			{
				return this.m_HintWeight;
			}
			set
			{
				this.m_HintWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000077E7 File Offset: 0x000059E7
		// (set) Token: 0x06000226 RID: 550 RVA: 0x000077EF File Offset: 0x000059EF
		public bool maintainTargetPositionOffset
		{
			get
			{
				return this.m_MaintainTargetPositionOffset;
			}
			set
			{
				this.m_MaintainTargetPositionOffset = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000077F8 File Offset: 0x000059F8
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00007800 File Offset: 0x00005A00
		public bool maintainTargetRotationOffset
		{
			get
			{
				return this.m_MaintainTargetRotationOffset;
			}
			set
			{
				this.m_MaintainTargetRotationOffset = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00007809 File Offset: 0x00005A09
		string ITwoBoneIKConstraintData.targetPositionWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_TargetPositionWeight");
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00007815 File Offset: 0x00005A15
		string ITwoBoneIKConstraintData.targetRotationWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_TargetRotationWeight");
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00007821 File Offset: 0x00005A21
		string ITwoBoneIKConstraintData.hintWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_HintWeight");
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00007830 File Offset: 0x00005A30
		bool IAnimationJobData.IsValid()
		{
			return this.m_Tip != null && this.m_Mid != null && this.m_Root != null && this.m_Target != null && this.m_Tip.IsChildOf(this.m_Mid) && this.m_Mid.IsChildOf(this.m_Root);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000789C File Offset: 0x00005A9C
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_Root = null;
			this.m_Mid = null;
			this.m_Tip = null;
			this.m_Target = null;
			this.m_Hint = null;
			this.m_TargetPositionWeight = 1f;
			this.m_TargetRotationWeight = 1f;
			this.m_HintWeight = 1f;
			this.m_MaintainTargetPositionOffset = false;
			this.m_MaintainTargetRotationOffset = false;
		}

		// Token: 0x040000EC RID: 236
		[SerializeField]
		private Transform m_Root;

		// Token: 0x040000ED RID: 237
		[SerializeField]
		private Transform m_Mid;

		// Token: 0x040000EE RID: 238
		[SerializeField]
		private Transform m_Tip;

		// Token: 0x040000EF RID: 239
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_Target;

		// Token: 0x040000F0 RID: 240
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_Hint;

		// Token: 0x040000F1 RID: 241
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_TargetPositionWeight;

		// Token: 0x040000F2 RID: 242
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_TargetRotationWeight;

		// Token: 0x040000F3 RID: 243
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_HintWeight;

		// Token: 0x040000F4 RID: 244
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainTargetPositionOffset;

		// Token: 0x040000F5 RID: 245
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainTargetRotationOffset;
	}
}
