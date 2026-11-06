using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public struct ChainIKConstraintData : IAnimationJobData, IChainIKConstraintData
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006692 File Offset: 0x00004892
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000669A File Offset: 0x0000489A
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000066A3 File Offset: 0x000048A3
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000066AB File Offset: 0x000048AB
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000066B4 File Offset: 0x000048B4
		// (set) Token: 0x0600014C RID: 332 RVA: 0x000066BC File Offset: 0x000048BC
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000066C5 File Offset: 0x000048C5
		// (set) Token: 0x0600014E RID: 334 RVA: 0x000066CD File Offset: 0x000048CD
		public float chainRotationWeight
		{
			get
			{
				return this.m_ChainRotationWeight;
			}
			set
			{
				this.m_ChainRotationWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000066DB File Offset: 0x000048DB
		// (set) Token: 0x06000150 RID: 336 RVA: 0x000066E3 File Offset: 0x000048E3
		public float tipRotationWeight
		{
			get
			{
				return this.m_TipRotationWeight;
			}
			set
			{
				this.m_TipRotationWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000066F1 File Offset: 0x000048F1
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000066F9 File Offset: 0x000048F9
		public int maxIterations
		{
			get
			{
				return this.m_MaxIterations;
			}
			set
			{
				this.m_MaxIterations = Mathf.Clamp(value, 1, 50);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000670A File Offset: 0x0000490A
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00006712 File Offset: 0x00004912
		public float tolerance
		{
			get
			{
				return this.m_Tolerance;
			}
			set
			{
				this.m_Tolerance = Mathf.Clamp(value, 0f, 0.01f);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000672A File Offset: 0x0000492A
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00006732 File Offset: 0x00004932
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000673B File Offset: 0x0000493B
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00006743 File Offset: 0x00004943
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000674C File Offset: 0x0000494C
		string IChainIKConstraintData.chainRotationWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_ChainRotationWeight");
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00006758 File Offset: 0x00004958
		string IChainIKConstraintData.tipRotationWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_TipRotationWeight");
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006764 File Offset: 0x00004964
		bool IAnimationJobData.IsValid()
		{
			if (this.m_Root == null || this.m_Tip == null || this.m_Target == null)
			{
				return false;
			}
			int num = 1;
			Transform transform = this.m_Tip;
			while (transform != null && transform != this.m_Root)
			{
				transform = transform.parent;
				num++;
			}
			return transform == this.m_Root && num > 2;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000067E0 File Offset: 0x000049E0
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_Root = null;
			this.m_Tip = null;
			this.m_Target = null;
			this.m_ChainRotationWeight = 1f;
			this.m_TipRotationWeight = 1f;
			this.m_MaxIterations = 15;
			this.m_Tolerance = 0.0001f;
			this.m_MaintainTargetPositionOffset = false;
			this.m_MaintainTargetRotationOffset = false;
		}

		// Token: 0x040000AB RID: 171
		internal const int k_MinIterations = 1;

		// Token: 0x040000AC RID: 172
		internal const int k_MaxIterations = 50;

		// Token: 0x040000AD RID: 173
		internal const float k_MinTolerance = 0f;

		// Token: 0x040000AE RID: 174
		internal const float k_MaxTolerance = 0.01f;

		// Token: 0x040000AF RID: 175
		[SerializeField]
		private Transform m_Root;

		// Token: 0x040000B0 RID: 176
		[SerializeField]
		private Transform m_Tip;

		// Token: 0x040000B1 RID: 177
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_Target;

		// Token: 0x040000B2 RID: 178
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_ChainRotationWeight;

		// Token: 0x040000B3 RID: 179
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_TipRotationWeight;

		// Token: 0x040000B4 RID: 180
		[NotKeyable]
		[SerializeField]
		[Range(1f, 50f)]
		private int m_MaxIterations;

		// Token: 0x040000B5 RID: 181
		[NotKeyable]
		[SerializeField]
		[Range(0f, 0.01f)]
		private float m_Tolerance;

		// Token: 0x040000B6 RID: 182
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainTargetPositionOffset;

		// Token: 0x040000B7 RID: 183
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainTargetRotationOffset;
	}
}
